using System.Collections.Generic;
using System.IO;
using MessagePack;
using MessagePack.Resolvers;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Omino.Infra.Master
{
    public static class MasterBuilder
    {
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
            catch { }

            var reader = new StreamReader("Assets/Project/Data/CharacterTable.yaml");
            var text = reader.ReadToEnd();
            var deserializer = new DeserializerBuilder()
                                // .IgnoreUnmatchedProperties()
                                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                .Build();
            var dummy = deserializer.Deserialize<List<CharacterData>>(text);

            var builder = new DatabaseBuilder();
            builder.Append(dummy);

            var binary = builder.Build();
            var path = "Assets/StreamingAssets/MasterTable";
            var dir = Path.GetDirectoryName(path);

            Directory.CreateDirectory(dir);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                stream.Write(binary, 0, binary.Length);
            }

            AssetDatabase.Refresh();

            var db = new MemoryDatabase(binary);
            var characters = db.CharacterDataTable;

            Debug.Log(characters.FindById(1).Name);
        }
    }
}