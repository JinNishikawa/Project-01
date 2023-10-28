using MessagePack;
using MessagePack.Resolvers;
using System.IO;
using UnityEditor;
using UnityEngine;

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

            var dummy = new[]
            {
                new CharacterData { Id = 1, Name = "test", ObjectId = "1", Atk = 1, Hp = 1 },
                new CharacterData { Id = 2, Name = "dummy", ObjectId = "2", Atk = 1, Hp = 1 },
            };

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