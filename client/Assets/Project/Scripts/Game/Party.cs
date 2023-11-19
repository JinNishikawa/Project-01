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

        foreach (var column in _party.Formation)
        {
            foreach(var id in column)
            {
                var character = await _charaFactory.Create(id);
                character?.transform.SetParent(_charactersRoot);
            }
        }
    }

    public void Move()
    {
        this.transform.DOMoveX(0.5f, 1f)
            .SetLoops(-1, LoopType.Incremental);
    }
}
