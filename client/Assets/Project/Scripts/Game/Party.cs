using DG.Tweening;
using UnityEngine;

public class Party : MonoBehaviour
{
    [SerializeField]
    private Character _character;

    private uint[,] _formation;

    private void Awake()
    {
        _formation = new uint[2,2]
        {
            {1,1},
            {1,1},
        };


        for(var i = 0; i < _formation.GetLength(0); i++)
        {
            for(var j = 0; j < _formation.GetLength(1); j++)
            {
                var pos = new Vector3(j * 0.5f, 0, i * 0.5f);
                GameObject.Instantiate(_character, pos, Quaternion.identity, this.transform);
            }
        }
    }

    private void Start()
    {
        Move();
    }

    public void Move()
    {
        this.transform.DOMoveX(0.5f, 1f)
            .SetLoops(-1, LoopType.Incremental);
    }
}
