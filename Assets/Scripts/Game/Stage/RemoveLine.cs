using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Stage
{
    public class RemoveLine : MonoBehaviour
    {
        private void Awake()
        {
            int tileType = (int)Field.Battle.TileType.Line;
            // É^ÉCÉãê›íu
            bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[tileType], transform.position);
            if (isRange)
            {
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Line, transform.position, null);
            }

            Destroy(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}