//using MagicOnion;
//using Shared.Sample.MessagePack;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Shared.Sample
//{
//    // NOTE: StreamingHubの分割単位を考える
//    public interface IPlayerHub : IStreamingHub<IPlayerHub, IPlayerHubReceiver>
//    {
//        ValueTask<int> JoinAsync();
//        ValueTask<Player[]> FetchCurrentPlayerAsync();
//        ValueTask LeaveAsync();
//        ValueTask SummonPawn(int typeId, Vector3 position);
//    }

//    public interface IPlayerHubReceiver
//    {
//        void OnPlayerJoined(int id, Vector3 position);
//        void OnPlayerLeaved(int id);
//        void OnPlayerMoved(int id, Vector3 position);
//        void OnSummonedPawn(int ownerId, int pawnId, int typeId, Vector3 position);
//        void OnPawnMoved(int ownerId, int pawnId, Vector3 position);
//        void OnPawnDied(int ownerId, int pawnId);
//    }

//}
