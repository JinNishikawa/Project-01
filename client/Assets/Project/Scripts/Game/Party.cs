using DG.Tweening;
using UnityEngine;
using Omino.Infra.Master;
using System.Linq;
using Cysharp.Threading.Tasks;
using VContainer;

public class Party : MonoBehaviour, IFactoryObject<PartyData>
{
    [SerializeField]
    private Transform _charactersRoot = null;

    private PartyData _party;
    private CharacterFactory _charaFactory;

    private readonly float CellSize = 1.0f;

    [Inject]
    public void Inject(CharacterFactory factory)
    {
        _charaFactory = factory;
    }

    public async UniTask Setup(PartyData data)
    {
        if(data == null)
            return;

        _party = data;

        var center = new Vector3()
        {
            x = (float)_party.Formation.CenterIndex() / 2.0f + CellSize,
            y = 0,
            z = (float)_party.Formation.FirstOrDefault().CenterIndex() / 2.0f + CellSize,
        };
        _charactersRoot.transform.position = center;

        for (var i = 0; i < _party.Formation.Count(); i++)
        {
            var row = _party.Formation.ElementAtOrDefault(i);
            for(var j = 0; j < row.Count(); j++)
            {
                var id = row.ElementAtOrDefault(j);
                var character = await _charaFactory.Create(id);
                var pos = new Vector3()
                {
                    x = j * CellSize,
                    y = 0,
                    z = i * CellSize,
                };

                if(character == null)
                    continue;

                character.transform.SetParent(_charactersRoot);
                character.transform.position = pos;
            }
        }
    }

    public void Move()
    {
        this.transform.DOMoveX(CellSize, 1f)
            .SetLoops(-1, LoopType.Incremental);
    }
}
