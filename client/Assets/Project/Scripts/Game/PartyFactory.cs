using Cysharp.Threading.Tasks;
using Omino.Infra.Master;
using Omino.Infra.Master.Tables;
using UnityEngine;
using VContainer;

public class PartyFactory : IFactory<Party, PartyData>
{
    private PartyDataTable _table;

    [Inject]
    public PartyFactory(MemoryDatabase database)
    {
        _table = database.PartyDataTable;
    }

    public async UniTask<Party> Create(uint id)
    {
        if(_table.TryFindById(id, out PartyData data))
        {
            var prefab = await Resources.LoadAsync<GameObject>("Prefab/Battle/Party") as GameObject;
            var obj = GameObject.Instantiate(prefab);
            var party = obj.GetComponent<Party>();
            await party.Setup(data);
            return party;
        }
        return null;
    }
}