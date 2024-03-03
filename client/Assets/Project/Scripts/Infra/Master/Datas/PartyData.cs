using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace Omino.Infra.Master
{
    [TableName("PartyTable"), MemoryTable("parties"), MessagePackObject(true)]
    public sealed class PartyData
    {
        [PrimaryKey]
        public uint Id { get; set; }
        public IEnumerable<IEnumerable<uint>> Formation { get; set; }
    }
}