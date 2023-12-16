using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace Core.Sample
{
    public class Pawn
    {
        public int OwnerId;
        public int Id;
        public int TypeId;
        public Vector3 Position;
        public int LifeTime;

        public Pawn(int ownerId, int id, int typeId, int lifeTime)
        {
            Id = id;
            OwnerId = ownerId;
            TypeId = typeId;
            Position = new Vector3();
            LifeTime = lifeTime;
        }

        public void MoveForward()
        {
             Position.z += 1;
             LifeTime -= 1;
        }
    }
}
