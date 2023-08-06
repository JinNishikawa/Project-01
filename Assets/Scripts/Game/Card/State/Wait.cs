using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Wait : State
    {
        /** 生存中か */
        private bool _isAlive = false;

        /** カード終了か */
        private bool _isFin = false;

        /** 自陣色 */
        private Color myColor;

        /** 敵陣色 */
        private Color enemyColor;

        public override void Awake()
        {
            base.Awake();

            // 待ち時間設定
            _timer = _info._soldierWaitTime;

            // 生存中か
            _isAlive = false;
            _isFin = false;

            Vector3 pos = transform.position;
            pos.y = 0.0f;
            transform.position = pos;

            myColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)_info._userType];
            Manager.UserType enemyType = Manager.UserType.Player1;
            if(enemyType == _info._userType)
            {
                enemyType = Manager.UserType.Player2;
            }
            enemyColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)enemyType];
        }

        public override void Start()
        {
            base.Start();

            _isAlive = CheckFinCard();
            if(_isFin)
            {
                _timer = 0.0f;
            }

            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                if (!soldier._isAlive) continue;

                Soldier.Wait wait = soldier.gameObject.AddComponent<Soldier.Wait>();
                soldier.Change(wait);
            }

            if(_info._isOnlyVisual)
            {
                Soldier.Wait wait = _card._soldierVisual.gameObject.AddComponent<Soldier.Wait>();
                _card._soldierVisual.Change(wait);
            }

            //Manager.UserManager.Instance._characterInfo[(int)_info._userType].CheckFrontPosition();
            Manager.UserManager.Instance._characterInfo[(int)_info._userType].CheckFrontPositions();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            float time = Time.deltaTime * Manager.GameMgr.Instance._gameSpeed;
            UpdateTimer(time);
        }

        public override void Fin()
        {
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
               
                if(soldier.GetComponent<Soldier.Attack>())
                {
                    continue;
                }
                if(soldier.GetComponent<Soldier.UserAttack>())
                {
                    continue;
                }
                if (!soldier._isExist)
                {
                    soldier.Change(null);
                    continue;
                }
                if (_card.GetExistCount() <= 0)
                {
                    soldier.Change(null);
                    continue;
                }

                Soldier.Move move = soldier.gameObject.AddComponent<Soldier.Move>();
                soldier.Change(move);
            }

            if (_info._isOnlyVisual)
            {
                Soldier.Move move = _card._soldierVisual.gameObject.AddComponent<Soldier.Move>();
                _card._soldierVisual.Change(move);
            }

            base.Fin();
        }

        private void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer < 0.0f)
            {
                float time = 0.0f;
                if (!_isFin && CheckNextCell()) return;

                // 移動ステート追加
                Move move = gameObject.AddComponent<Move>();
                Vector3 nextPos;
                if (_isFin)
                {
                    Manager.UserManager.Instance._characterInfo[(int)_info._userType].RemoveFieldList(gameObject);
                    // 全滅
                    if (!_isAlive)
                    {
                        _info._next = CardState.None;
                        _card.Change(null);
                        Destroy(gameObject);
                        return;
                    }

                    // 兵士生存中
                    nextPos = _characterInfo._deck.transform.position;

                    // スケール
                    float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierCardSize;
                    _card.SetSoldierScale(soldierSize);
                    time = Manager.GameMgr.Instance._GameSetting._MoveToDeckTime;
                    _info._next = CardState.Deck;
                    _card.transform.GetChild(0).gameObject.SetActive(true);

                }
                else
                {
                    int tileType = (int)Field.Battle.TileType.Battle;

                    // 進行方向
                    int dir = _card._info._moveDir;
                    // 次のセル位置
                    Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], transform.position);
                    // 移動
                    nextCellPos.x += dir;
                    // ワールド座標へ変換
                    nextPos = TilemapFunction.GetWorldPos(Field.Battle.Instance._tiles[tileType], nextCellPos);
                    time = Manager.GameMgr.Instance._GameSetting._FieldMoveTime;

                    _info._next = CardState.Wait;
                }

                move.StartMove(nextPos, time);
                _card.Change(move);
            }
        }

        private bool CheckFinCard()
        {
            // 兵士数
            if (_card.GetExistCount() > 0) return false;

            _isFin = true;
            _timer = 0.0f;
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                if (!soldier._isAlive) continue;
                return true;
            }

            return false;
        }

        private bool CheckNextCell()
        {
            // タイルタイプ
            int tileType = (int)Field.Battle.TileType.Battle;
            // 進行方向
            int dir = _card._info._moveDir;

            foreach (GameObject obj in _card.GetSoldiersList())
            {
                // 次の位置
                Vector3 pos = obj.transform.localPosition * dir + transform.position;

                // 次のセル位置
                Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], pos);
                // 移動
                nextCellPos.x += dir;

                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier._iconObj.SetActive(false);

                // 次の位置のタイルなし
                if (!Field.Battle.Instance._tiles[tileType].HasTile(nextCellPos)) continue;

                // 同じカード
                if (CheckSameCard(obj, nextCellPos)) continue;

                // 敵のカード
                if (CheckEnemyCard(nextCellPos)) continue;


                bool isDebug = Manager.DebugManager.Instance.GetFlag();
                if (isDebug)
                {
                    soldier._iconObj.SetActive(true);
                }

                // 次のカードが存在
                return true;
            }

            // 次のカードなし
            return false;
        }

        private bool CheckSameCard(GameObject checkObj,Vector3Int nextCellPos)
        {
            // タイルタイプ
            int tileType = (int)Field.Battle.TileType.Battle;
            // 進行方向
            int dir = _card._info._moveDir;

            // 同じカードかどうか
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                if (checkObj == obj) continue;
                Vector3 otherPos = obj.transform.localPosition * dir + transform.position;
                Vector3Int otherCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], otherPos);
                if (otherCellPos != nextCellPos) continue;

                // 同じカード
                return true;
            }

            return false;
        }

        private bool CheckEnemyCard(Vector3Int nextCellPos)
        {
            // タイルタイプ
            int tileType = (int)Field.Battle.TileType.Battle;
            // 進行方向
            int dir = _card._info._moveDir;

            Color nextColor = Field.Battle.Instance._tiles[tileType].GetColor(nextCellPos);

            if (nextColor == enemyColor)
            {
                return true;
            }

            return false;
        }
    }
}