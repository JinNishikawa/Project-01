using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class Move : State
    {
        GameObject _smokeEffect;

        public override void Awake()
        {
            base.Awake();

            if(_soldier is SoldierVisual)
            {

            }
            else if(_soldier is Soldier)
            {
                SetNextTile();
                StartEffect();
            }
            _soldier._animation.SetAnimationType(SoldierAnimation.AnimType.Walk, true);
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            if(_soldier is SoldierVisual)
            {

            }
            else if(_soldier is Soldier)
            {
                _smokeEffect.transform.position = transform.position;
            }

            _soldier._animation.Update();
        }

        public override void Fin()
        {
            if (_soldier is SoldierVisual)
            {
                
            }
            else if (_soldier is Soldier)
            {
                Destroy(_smokeEffect);
            }
            base.Fin();
        }

        private void SetNextTile()
        {
            //=== 前位置タイル削除
            Soldier back = _soldier._backSoldier;
            if (back == null || back._isAlive == false)
            {
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos, null);
            }

            //=== 次位置タイル設置
            // タイルタイプ
            int tileType = (int)Field.Battle.TileType.Battle;
            // 進行方向
            int dir = _soldier._info._moveDir;
            // 次のセル位置
            Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], transform.position);
            // 移動
            nextCellPos.x += dir;
            // ワールド座標へ変換
            Vector3 nextPos = TilemapFunction.GetWorldPos(Field.Battle.Instance._tiles[tileType], nextCellPos);
            // タイル設置
            bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[(int)Field.Battle.TileType.Battle], nextPos);
            if (isRange)
            {
                _soldier._storePos = nextPos;
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos);
                // 色変更
                Color color = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)_soldier._info._userType];
                Field.Battle.Instance.SetColorTile(Field.Battle.TileType.Battle, _soldier._storePos, color);
            }
        }

        private void StartEffect()
        {
            GameObject effectObj = Manager.GameMgr.Instance._GameSetting._EffectData?._WalkSmoke;
            Vector3 pos = transform.position;
            pos.x -= _soldier._info._moveDir * 0.01f;
            _smokeEffect = Manager.EffectManager.Instance.StartEffect(effectObj, pos, 3.0f);
            Vector3 angle = _smokeEffect.transform.GetChild(0).eulerAngles;
            angle.y *= _soldier._info._moveDir;
            _smokeEffect.transform.GetChild(0).eulerAngles = angle;
        }
    }
}