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
            //=== �O�ʒu�^�C���폜
            Soldier back = _soldier._backSoldier;
            if (back == null || back._isAlive == false)
            {
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos, null);
            }

            //=== ���ʒu�^�C���ݒu
            // �^�C���^�C�v
            int tileType = (int)Field.Battle.TileType.Battle;
            // �i�s����
            int dir = _soldier._info._moveDir;
            // ���̃Z���ʒu
            Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], transform.position);
            // �ړ�
            nextCellPos.x += dir;
            // ���[���h���W�֕ϊ�
            Vector3 nextPos = TilemapFunction.GetWorldPos(Field.Battle.Instance._tiles[tileType], nextCellPos);
            // �^�C���ݒu
            bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[(int)Field.Battle.TileType.Battle], nextPos);
            if (isRange)
            {
                _soldier._storePos = nextPos;
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos);
                // �F�ύX
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