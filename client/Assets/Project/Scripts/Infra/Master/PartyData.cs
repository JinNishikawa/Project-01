using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace Omino.Infra.Master
{
    [MemoryTable("parties"), MessagePackObject(true)]
    public sealed class PartyData
    {
        [PrimaryKey]
        public uint Id { get; set; }
        public uint[,] Formation { get; set; }
        public IEnumerable<uint> Members { get; set; }
        public float MoveSpeed { get; set; }
    }
}