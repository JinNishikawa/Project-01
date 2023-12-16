using MessagePack;
using UnityEngine;

namespace Shared.Sample.MessagePack
{
    [MessagePackObject]
    public class Player
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public Vector3 Position { get; set; }

        public Player(int id, Vector3 position)
        {
            Id = id;
            Position = position;
        }
    }
}
