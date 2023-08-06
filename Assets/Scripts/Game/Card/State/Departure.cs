using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Departure : State
    {
        private float _maxTime = 0.1f;

        public override void Awake()
        {
            base.Awake();

            if (_card == null) return;

            // 選択用タイルマップのタイル解除
            foreach(GameObject obj in _card.GetSoldiersList())
            {
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Select, obj.transform.position, null);
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier._boxCollider.SetActive(true);
            }

            // 準備場所終了
            _card._preparePosition.EndPrepareCard();
            // 準備場所なし
            _card._preparePosition = null;

            // マウスポインタ判定用コライダーOFF
            GetComponent<BoxCollider>().enabled = false;

            float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierFieldSize;
            if (!_info._isOnlyVisual)
            {
                _card.SetVisibleSoldier(true);
                _card.SetVisibleBase(false);
                _card.SetSoldierScale(soldierSize);
            }
            else
            {
                _card.SetVisibleSoldier(false);
                _card.SetVisibleBase(true);
                foreach (GameObject obj in _card.GetSoldiersList())
                {
                    Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                    soldier.SetCharacterScale(1.0f);
                }

                _card._soldierVisual.SetCharacterScale(_info._soldierScale);
            }

            SetTimer(_maxTime);
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

            CharacterRotate();

            float time = Time.deltaTime * Manager.GameMgr.Instance._gameSpeed;
            UpdateTimer(time);
        }

        public override void Fin()
        {
            base.Fin();
        }

        private void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer < 0.0f)
            {
                Manager.UserManager.Instance._characterInfo[(int)_info._userType].AddFieldList(gameObject);
                // Waitステート追加
                Wait wait = gameObject.AddComponent<Wait>();
                _card.Change(wait);
            }
        }

        private void CharacterRotate()
        {
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier.SetRotateCharacter(0.0f);
            }

            if (_info._isOnlyVisual)
            {
                _card._soldierVisual.SetRotateCharacter(0.0f);
            }
        }
    }
}