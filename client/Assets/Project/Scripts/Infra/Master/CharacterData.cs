using MasterMemory;
using MessagePack;

namespace Omino.Infra.Master
{
    [MemoryTable("characters"), MessagePackObject(true)]
    public sealed class CharacterData
    {
        [PrimaryKey]
        public uint Id { get; set; }
        public string Name { get; set; }
        public string ObjectId  { get; set; }
        public uint Hp { get; set; }
        public uint Atk { get; set; }
    }
}