using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Field
{
    public class Battle : SingletonMonoBehaviour<Battle>
    {
        /** タイルタイプ */
        public enum TileType
        {
            Line,
            Select,
            Battle,
            P1Check,
            P2Check,

            MAX_TILE
        }

        /** タイルマップ */
        [HideInInspector]
        public Tilemap[] _tiles;

        /** 設置するタイル */
        private TileBase _tileBase;

        [HideInInspector]
        public float _tileScale;


        private void Awake()
        {
            GameObject grid = transform.Find("Grid").gameObject;
            int num = grid.transform.childCount;
            _tiles = new Tilemap[num];
            for (int i=0;i<num;i++)
            {
                _tiles[i] = grid.transform.GetChild(i).GetComponent<Tilemap>();
            }

            // 初期化
            InitFlag();

            _tileBase = Manager.GameMgr.Instance._GameSetting._TileBase;

            _tileScale = _tiles[(int)TileType.Select].transform.localScale.x;
        }

        public void InitFlag()
        {
            GameObject grid = transform.Find("Grid").gameObject;
            int num = grid.transform.childCount;
            for (int i = 0; i < num; i++)
            {
                TilemapFunction.SetClearAllTile(_tiles[i]);
            }

            SetSpriteAlpha(TileType.P1Check, 0.0f);
            SetSpriteAlpha(TileType.P2Check, 0.0f);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void SetTile(TileType type, Vector3 worldPos)
        {
            TilemapFunction.SetTile(_tiles[(int)type], worldPos, _tileBase);
        }

        public void SetTile(TileType type, Vector3 worldPos, TileBase tileBase)
        {
            TilemapFunction.SetTile(_tiles[(int)type], worldPos, tileBase);
        }

        public void SetColorTile(TileType type, Vector3 worldPos, Color color)
        {
            TilemapFunction.SetTileFlagNone(_tiles[(int)type], worldPos, TileFlags.None);
            TilemapFunction.SetTileColor(_tiles[(int)type], worldPos, color);
        }

        public void SetSpriteAlpha(TileType type, float alpha)
        {
            Color color = _tiles[(int)type].color;
            color.a = alpha;
            TilemapFunction.SetSpriteColor(_tiles[(int)type], color);
        }


        public void SetCheckTile(TileType type, Vector3Int frontCellPos, Manager.UserType userType, Color color)
        {
            BoundsInt bound = _tiles[(int)type].cellBounds;

            for (int y = bound.max.y - 1; y >= bound.min.y; --y)
            {
                for (int x = bound.min.x; x < bound.max.x; ++x)
                {
                    bool isColor = true;
                    if (userType == Manager.UserType.Player1)
                    {
                        if (x > frontCellPos.x)
                        {
                            isColor = false;
                        }
                    }
                    else if(userType == Manager.UserType.Player2)
                    {
                        if (x < frontCellPos.x)
                        {
                            isColor = false;
                        }
                    }

                    var pos = new Vector3Int(x, y, 0);
                    if (isColor)
                    {
                        _tiles[(int)type].SetTile(pos, _tileBase);
                        Vector3 worldPos = TilemapFunction.GetWorldPos(_tiles[(int)type], pos);
                        SetColorTile(type, worldPos, color);
                    }
                    else
                    {
                        _tiles[(int)type].SetTile(pos, null);
                    }
                }
            }
        }

        public void SetCheckTiles(TileType type, Vector3Int[] frontCellPos, Manager.UserType userType, Color color)
        {
            BoundsInt bound = _tiles[(int)type].cellBounds;

            for (int y = bound.max.y - 1; y >= bound.min.y; --y)
            {
                for (int x = bound.min.x; x < bound.max.x; ++x)
                {
                    bool isColor = true;
                    int index = y - bound.min.y;
                    if (userType == Manager.UserType.Player1)
                    {
                        if (x > frontCellPos[index].x)
                        {
                            isColor = false;
                        }
                    }
                    else if (userType == Manager.UserType.Player2)
                    {
                        if (x < frontCellPos[index].x)
                        {
                            isColor = false;
                        }
                    }

                    var pos = new Vector3Int(x, y, 0);
                    if (isColor)
                    {
                        _tiles[(int)type].SetTile(pos, _tileBase);
                        Vector3 worldPos = TilemapFunction.GetWorldPos(_tiles[(int)type], pos);
                        SetColorTile(type, worldPos, color);
                    }
                    else
                    {
                        _tiles[(int)type].SetTile(pos, null);
                    }
                }
            }
        }

        public Vector3Int GetMaxBounds(TileType type)
        {
            BoundsInt bound = _tiles[(int)type].cellBounds;
            return bound.max;
        }

        public Vector3Int GetMinBounds(TileType type)
        {
            BoundsInt bound = _tiles[(int)type].cellBounds;
            return bound.min;
        }
    }
}