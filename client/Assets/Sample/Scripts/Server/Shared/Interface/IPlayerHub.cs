using MagicOnion;
using Shared.Sample.MessagePack;
using System.Threading.Tasks;

namespace Shared.Sample
{
    // NOTE: StreamingHubの分割単位を考える
    public interface IPlayerHub : IStreamingHub<IPlayerHub, IPlayerHubReceiver>
    {
        ValueTask<int> JoinAsync();
        ValueTask<Player[]> FetchCurrentPlayerAsync();
        ValueTask LeaveAsync();
        ValueTask SummonPawn(int typeId, Vector3Dto position);
    }

    public interface IPlayerHubReceiver
    {
        void OnPlayerJoined(int id, Vector3Dto position);
        void OnPlayerLeaved(int id);
        void OnPlayerMoved(int id, Vector3Dto position);
        void OnSummonedPawn(int ownerId, int pawnId, int typeId, Vector3Dto position);
        void OnPawnMoved(int ownerId, int pawnId, Vector3Dto position);
        void OnPawnDied(int ownerId, int pawnId);
    }

}
