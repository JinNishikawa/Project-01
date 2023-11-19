using Cysharp.Threading.Tasks;
using Omino.Infra.Master;
using Omino.Infra.Master.Tables;
using UnityEngine;
using VContainer;

public class CharacterFactory : IFactory<Character, CharacterData>
{
    private CharacterDataTable _table;

    [Inject]
    public CharacterFactory(MemoryDatabase database)
    {
        _table = database.CharacterDataTable;
    }

    public async UniTask<Character> Create(uint id)
    {
        if(_table.TryFindById(id, out CharacterData data))
        {
            var prefab = await Resources.LoadAsync<GameObject>("Prefab/Battle/Character") as GameObject;
            var obj = GameObject.Instantiate(prefab);
            obj.TryGetComponent<Character>(out Character character);
            await character.Setup(data);
            return character;
        }
        return null;
    }
}