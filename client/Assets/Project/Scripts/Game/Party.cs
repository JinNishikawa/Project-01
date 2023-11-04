using DG.Tweening;
using UnityEngine;
using Omino.Infra.Master;
using System.Linq;
using Cysharp.Threading.Tasks;

public class Party : MonoBehaviour, IFactoryObject<PartyData>
{
    private PartyData _party;

    public async UniTask Setup(PartyData data)
    {
        if(data == null)
            return;

        _party = data;
        var center = new Vector2(
            _party.Formation.First().CenterIndex(),
            _party.Formation.CenterIndex());
        foreach (var column in _party.Formation)
        {
            foreach(var id in column)
            {
                // var pos = new Vector3(j * 0.5f, 0, i * 0.5f);
                // GameObject.Instantiate(_character, pos, Quaternion.identity, this.transform);
            }
        }
    }

    public void Move()
    {
        this.transform.DOMoveX(0.5f, 1f)
            .SetLoops(-1, LoopType.Incremental);
    }
}
