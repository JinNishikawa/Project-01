using UnityEngine;
using VContainer;
using VContainer.Unity;
using Omino.Infra.Master;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TestManager : IAsyncStartable 
{
    private IFactory<Party, PartyData> _partyFactory;

    [Inject]
    public TestManager(PartyFactory factory)
    {
        _partyFactory = factory;
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await _partyFactory.Create(1);
    }
}