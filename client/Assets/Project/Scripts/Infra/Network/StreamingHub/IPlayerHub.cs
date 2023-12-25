using MagicOnion;
using UnityEngine;
using Shared.Sample.MessagePack;
using System.Threading.Tasks;

namespace Shared.StreamingHub
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
