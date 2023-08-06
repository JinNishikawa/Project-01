using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Field
{
    public class CharacterInfo : MonoBehaviour
    {
        /** ユーザータイプ */
        public Manager.UserType _Type;

        /** キャラクターテクスチャ */
        private Texture _CharacterTexture;

        /** デッキ */
        [HideInInspector]
        public Deck _deck;

        /** ライフメーター */
        [HideInInspector]
        public LifeMeter _lifeMeter;

        /** 準備場所 */
        [HideInInspector]
        public Prepare _prepare;

        /** 最前位置 */
        private Vector3Int _frontPosition;

        /** 最前位置用配列 */
        private Vector3Int[] _frontPositions;

        private Battle.TileType _checkTileType;
        private Tilemap _checkTile;

        [HideInInspector]
        public List<GameObject> _fieldCard;

        private Vector3Int _minCellPos;
        private int _gridCount;

        private void Awake()
        {
            // ユーザーマネージャー
            Manager.UserManager.Instance._characterInfo[(int)_Type] = this;

            // テクスチャ設定
            _CharacterTexture = Manager.UserManager.Instance._CharacterStatus[(int)_Type]._CharacterTexture;

            _fieldCard = new List<GameObject>();
            _fieldCard.Clear();
        }

        private void OnEnable()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {  
            InitFlag();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void InitFlag()
        {
            // AI消去
            DeleteAI();

            // キャラクター色
            SetCharacterColor(0.0f);

            _gridCount = Manager.GameMgr.Instance._GameSetting._InitGridCount;

            _checkTileType = Battle.TileType.P1Check;
            _minCellPos = Battle.Instance._tiles[(int)_checkTileType].cellBounds.min;
            _minCellPos.x += (_gridCount - 1);
            if (_Type == Manager.UserType.Player2)
            {
                _checkTileType = Battle.TileType.P2Check;
                _minCellPos = Battle.Instance._tiles[(int)_checkTileType].cellBounds.max;
                _minCellPos.x -= _gridCount;
            }
            _checkTile = Battle.Instance._tiles[(int)_checkTileType];

            InitFrontPosition();
            SetCheckTile();

            /** フィールド上カードリストクリア */
            _fieldCard.Clear();
        }

        private void SetFrontPosition(Vector3 pos)
        {
            bool isRange = TilemapFunction.IsRange(_checkTile, pos);
            if (!isRange) return;

            Vector3Int cellPos = TilemapFunction.GetCellsPos(_checkTile, pos);
            cellPos.z = 0;
            if(_Type == Manager.UserType.Player1)
            {
                if(_frontPosition.x < cellPos.x - _gridCount)
                {
                    _frontPosition = cellPos;
                    _frontPosition.x -= _gridCount;
                }
            }
            else if(_Type == Manager.UserType.Player2)
            {
                if (_frontPosition.x > cellPos.x + _gridCount)
                {
                    _frontPosition = cellPos;
                    _frontPosition.x += _gridCount;
                }
            }

            _frontPosition.y = 0;
            _frontPosition.z = 0;
        }

        private void SetFrontPositions(Vector3 pos)
        {
            bool isRange = TilemapFunction.IsRange(_checkTile, pos);
            if (!isRange) return;

            Vector3Int cellPos = TilemapFunction.GetCellsPos(_checkTile, pos);
            cellPos.z = 0;

            Vector3Int minBound = Battle.Instance.GetMinBounds(_checkTileType);
            int index = cellPos.y - minBound.y;
            if (index < 0 || _frontPositions.Length <= index) return;
            if (_Type == Manager.UserType.Player1)
            {
                if (_frontPositions[index].x < cellPos.x - _gridCount)
                {
                    _frontPositions[index] = cellPos;
                    _frontPositions[index].x -= _gridCount;
                }
            }
            else if (_Type == Manager.UserType.Player2)
            {
                if (_frontPositions[index].x > cellPos.x + _gridCount)
                {
                    _frontPositions[index] = cellPos;
                    _frontPositions[index].x += _gridCount;
                }
            }

        }

        private void SetCheckTile()
        {
            // 色変更
            Color color = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)_Type];
            color.a = 1.0f;
            Battle.Instance.SetCheckTile(_checkTileType, _frontPosition, _Type, color);
        }

        private void SetCheckTiles()
        {
            // 色変更
            Color color = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)_Type];
            color.a = 1.0f;
            Battle.Instance.SetCheckTiles(_checkTileType, _frontPositions, _Type, color);
        }

        public void CheckFrontPosition()
        {
            InitFrontPosition();
            foreach (GameObject obj in _fieldCard)
            {
                Card.Card card = obj.GetComponent<Card.Card>();
                if (card == null) continue;
                int dir = card._info._moveDir;
                foreach(GameObject soldierObj in card.GetSoldiersList())
                {
                    Soldier.Soldier soldier = soldierObj.GetComponent<Soldier.Soldier>();
                    if (!soldier._isAlive) continue;
                    SetFrontPosition(soldierObj.transform.position);
                }
            }

            SetCheckTile();
        }

        public void CheckFrontPositions()
        {
            InitFrontPosition();
            foreach (GameObject obj in _fieldCard)
            {
                Card.Card card = obj.GetComponent<Card.Card>();
                if (card == null) continue;
                int dir = card._info._moveDir;
                foreach (GameObject soldierObj in card.GetSoldiersList())
                {
                    Soldier.Soldier soldier = soldierObj.GetComponent<Soldier.Soldier>();
                    if (!soldier._isAlive) continue;
                    SetFrontPositions(soldierObj.transform.position);
                }
            }

            SetCheckTiles();
        }

        public void AddFieldList(GameObject obj)
        {
            _fieldCard.Add(obj);
            //CheckFrontPosition();
            CheckFrontPositions();
        }

        public void RemoveFieldList(GameObject obj)
        {
            _fieldCard.Remove(obj);
            //CheckFrontPosition();
            CheckFrontPositions();
        }

        private void InitFrontPosition()
        {
            _frontPosition = _minCellPos;
            _frontPosition.y = 0;
            _frontPosition.z = 0;

            if(_frontPositions == null)
            {
                Vector3Int maxBound = Battle.Instance.GetMaxBounds(_checkTileType);
                Vector3Int minBound = Battle.Instance.GetMinBounds(_checkTileType);
                _frontPositions = new Vector3Int[maxBound.y - minBound.y];
            }

            for(int i=0;i<_frontPositions.Length;i++)
            {
                _frontPositions[i] = _frontPosition;
            }
        }

        public void SetTexture(Texture characterTexture)
        {
            // テクスチャ設定
            SettingTexture set = transform.GetComponentInChildren<SettingTexture>();
            set?.SetTexture(characterTexture);
            SetCharacterColor(1.0f);
        }

        public void SetCharacterColor(float alpha)
        {
            SettingTexture set = transform.GetComponentInChildren<SettingTexture>();
            Color color = set.GetComponent<MeshRenderer>().material.color;
            color.a = alpha;
            set.GetComponent<MeshRenderer>().material.color = color;
        }

        public void InitAI()
        {
            BehaviourTree behaviourTree = Manager.UserManager.Instance._CharacterStatus[(int)_Type]._behaviourTree;
            if (behaviourTree != null)
            {
                BehaviourTreeRunner BTRunner = GetComponent<BehaviourTreeRunner>();
                if(BTRunner == null)
                {
                    BTRunner = gameObject.AddComponent<BehaviourTreeRunner>();
                }
                BTRunner.SetTree(behaviourTree);
            }
        }

        private void DeleteAI()
        {
            BehaviourTreeRunner BTRunner = GetComponent<BehaviourTreeRunner>();
            Destroy(BTRunner);
        }
    }
}