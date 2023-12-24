using MagicOnion;
using UnityEngine;
using System.Threading.Tasks;

namespace Shared.StreamingHub
{
    public interface IPartyHub : IStreamingHub<IPartyHub, IPartyHubReceiver>
    {
        ValueTask PlaceParty(int partyId, Vector3 position);
    }

    public interface IPartyHubReceiver
    {
        void PlacedParty(int ownerId, int partyId, int uniqueId, Vector3 position);
        void MovedParty(int ownerId, int uniqueId, Vector3 prev, Vector3 current);
    }
}