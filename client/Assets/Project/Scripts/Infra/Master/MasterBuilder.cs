#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MessagePack;
using MessagePack.Resolvers;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Omino.Infra.Master
{
    internal static class MasterBuilder
    {
        private const string TABLE_PATH = "Assets/Project/Data";
        private const string OUTPUT_PATH = "Assets/Project/Resources/MasterTable";

        [MenuItem("Tools/MasterBuild")]
        public static void Build()
        {
            try
            {
                StaticCompositeResolver.Instance.Register(new[]
                    {
                    MasterMemoryResolver.Instance,
                    GeneratedResolver.Instance,
                    StandardResolver.Instance
                    }
                );

                var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
                MessagePackSerializer.DefaultOptions = options;
            }
            catch (Exception e)
            { 
                Debug.LogWarning(e.Message);
            }

            var binary = CreateDatabaseAsset();
            var dir = Path.GetDirectoryName(OUTPUT_PATH);

            Directory.CreateDirectory(dir);

            using (var stream = new FileStream(OUTPUT_PATH, FileMode.Create))
            {
                stream.Write(binary, 0, binary.Length);
            }

            AssetDatabase.Refresh();

            Debug.Log("MasterTable出力終わり");
        }

        private static byte[] CreateDatabaseAsset()
        {
            var builder = new DatabaseBuilder();

            builder.Append(DeserializeTable<CharacterData>());
            builder.Append(DeserializeTable<PartyData>());

            return builder.Build();
        }

        private static IEnumerable<T> DeserializeTable<T>()
        {
            var attribute = Assembly.GetAssembly(typeof(T))
                            .GetTypes()
                            .FirstOrDefault(type => type == typeof(T))
                            .GetCustomAttribute<TableNameAttribute>();
            
            if(attribute == null || string.IsNullOrEmpty(attribute.TableName))
            {
                Debug.LogErrorFormat("{0}はTableName属性を持っていないため読み込まない", nameof(T));
                return Enumerable.Empty<T>();
            }

            var path = Path.Combine(TABLE_PATH, attribute.TableName);
            using var reader = new StreamReader(path);
            var text = reader.ReadToEnd();
            var deserializer = new DeserializerBuilder()
                                .IgnoreUnmatchedProperties()
                                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                                .Build();
            return deserializer.Deserialize<List<T>>(text);
        }
    }

}

#endif