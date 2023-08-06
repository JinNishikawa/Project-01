using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class Wait : State
    {
        public override void Awake()
        {
            base.Awake();

            if (_soldier is SoldierVisual)
            {

            }
            else
            {
                bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[(int)Field.Battle.TileType.Battle], transform.position);
                if (isRange)
                {
                    _soldier._storePos = transform.position;
                    Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos);
                    Color color = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)_soldier._info._userType];
                    Field.Battle.Instance.SetColorTile(Field.Battle.TileType.Battle, _soldier._storePos, color);
                }
            }

            _soldier._animation.SetAnimationType(SoldierAnimation.AnimType.Wait, true);
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            if (_soldier is SoldierVisual)
            {

            }
            else
            {
                CheckAttack();
            }         
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            _soldier._animation.Update();
        }

        public override void Fin()
        {
            //if(_soldier is SoldierVisual)
            //{

            //}
            //else if(_soldier is Soldier)
            //{
            //    Soldier back = _soldier._backSoldier;
            //    if (back == null || back._isAlive == false)
            //    {
            //        //=== 前位置タイル削除
            //        Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos, null);
            //    }
            //}

            base.Fin();
        }

        private bool CheckAttack()
        {
            if (!_soldier._isAlive) return false;
            if (!_soldier._isExist) return false;

            UserAttack attack = GetComponent<UserAttack>();
            if (attack) return false;

            //=== 次のセル
            // 進行方向
            int dir = _soldier._info._moveDir;
            // 次のセル位置
            Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[(int)Field.Battle.TileType.Battle], transform.position);
            // 移動
            nextCellPos.x += dir;
            Vector3 nextPos = TilemapFunction.GetWorldPos(Field.Battle.Instance._tiles[(int)Field.Battle.TileType.Battle], nextCellPos);

            bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[(int)Field.Battle.TileType.Battle], nextPos);

            // 範囲外
            if (!isRange)
            {
                //=== 前位置タイル削除
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos, null);
                _soldier._boxCollider.SetActive(false);

                attack = gameObject.AddComponent<UserAttack>();
                attack.SetTimer(1.0f);
                _soldier.Change(attack);

                return true;
            }

            return false;
        }
    }
}