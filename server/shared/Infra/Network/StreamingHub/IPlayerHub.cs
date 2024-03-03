using MagicOnion;
using System.Threading.Tasks;
using UnityEngine;

namespace Omino.Infra.StreamingHub
{
    public interface IPlayerHub : IStreamingHub<IPlayerHub, IPlayerHubReceiver>
    {
        ValueTask<int> JoinAsync();
        ValueTask<int[]> FetchCurrentPlayerAsync();
        ValueTask LeaveAsync();
    }

    public interface IPlayerHubReceiver
    {
        void OnPlayerJoined(int id);
        void OnPlayerLeaved(int id);
    }
}
