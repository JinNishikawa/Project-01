using MessagePack;
using System.Numerics;

namespace Shared.Sample.MessagePack
{
    [MessagePackObject]
    public class Player
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public Vector3Dto Position { get; set; }

        public Player(int id, Vector3Dto position)
        {
            Id = id;
            Position = position;
        }
    }
}
