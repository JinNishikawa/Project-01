using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Card
{
    public class Ready : State
    {
        /** 最大時間 */
        private float _maxTime;

        /** 兵士クラス */
        private Soldier.Soldier[] _soldiers;

        /** 兵士α値 */
        private float _alpha;
        /** 兵士位置Y */
        private float _posY;
        /** 兵士ローカル位置Y */
        private float _initLocalPosY;
        /** 兵士ローカルスケールY */
        private float _localScaleY;

        private Material _initMaterial;
        private Material _dissolveMat;
        private string _thresholdName = "_Dissolve";

        private bool _isEffect;

        public override void Awake()
        {
            base.Awake();

            Vector3 pos = transform.position;
            pos.y += 0.03f;
            transform.position = pos;

            // 準備スタート
            _card._preparePosition.StartPrepareCard(_card);

            // 戻り位置設定
            _card._initPos = pos;

            _card.SetCenterSoldiers();
            float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierReadySize;
            _card.SetSoldierScale(soldierSize);

            // 角度
            float x = 90;
            Vector3 cardAngle = transform.eulerAngles;
            cardAngle.x = x;
            transform.rotation = Quaternion.Euler(cardAngle.x, cardAngle.y, cardAngle.z);

            _maxTime = _info._readyTime;
            _timer = _maxTime;

            if (_info._userType == Manager.UserType.Player2)
            {
                _card.SetVisibleReadyCharacter(true);
            }

            //_card.SetCardVisible(false);
            _dissolveMat = Manager.GameMgr.Instance._GameSetting._SystemSetting._DissolveMaterial;
            _card.SetCardMaterial(_dissolveMat);
            for(int i=0;i<_card.MaxRendererCount();i++)
            {
                _card.GetRenderer(i).material.SetFloat(_thresholdName, 1.0f);
            }

            _alpha = 0.0f;
            _posY = -1.6f;

            _isEffect = false;

            
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            // 手札から削除
            _characterInfo._deck.RemoveHand(gameObject);

            // 兵士を準備へ
            List<GameObject> list = _card.GetSoldiersList();
            _soldiers = new Soldier.Soldier[list.Count];
            int cnt = 0;


            foreach (GameObject obj in list)
            {
                Soldier.Ready ready = obj.AddComponent<Soldier.Ready>();
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                // 前取得
                soldier.CheckFront(_card);

                _soldiers[cnt] = soldier;
                soldier.SetFieldMaterial(Soldier.MaterialType.Lit);
                soldier.SetFieldColor(_card._playerColor);
                soldier.Change(ready);
                soldier.SetCharacterAlpha(_alpha);
                soldier.SetLocalPositionY(_posY);
                _initLocalPosY = soldier._characterLocalPositionY;
                _localScaleY = soldier._characterLocalScaleY;
                cnt++;
            }

            if (_info._isOnlyVisual)
            {
                float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierFieldSize;
                _card._soldierVisual.SetCharacterScale(_info._soldierScale);
                Soldier.Ready ready = _card._soldierVisual.gameObject.AddComponent<Soldier.Ready>();
                _card._soldierVisual.Change(ready);
            }

            //_card._prepareTimeText.enabled = false;
            _card._speedStar.gameObject.SetActive(false);
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
            base.Fin();

            Color color = _card._prepareTimeText.color;
            color.a = 1.0f * 255;
            _card._prepareTimeText.color = color;
            _card._prepareTimeText.enabled = true;
            _card._speedStar.gameObject.SetActive(true);
            _card.SetCardVisible(false);

            if (_info._isOnlyVisual)
            {
                float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierFieldSize;
                _card._soldierVisual.SetVisibleSprite(true);
                _card._soldierVisual.SetCharacterScale(_info._soldierScale);
                _card._soldierVisual.Change(null);
            }

            // 兵士状態なしへ
            List<GameObject> list = _card.GetSoldiersList();
            foreach (GameObject obj in list)
            {
                obj.GetComponent<Soldier.Soldier>().Change(null);
            }
        }

        private void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;
            float timeRate = Mathf.Clamp01(_timer / _maxTime);

            UpdateCardDissolve(timeRate);

            OnEffect();

            if (_timer < 0.0f)
            {
                for (int i = 0; i < _card.MaxRendererCount(); i++)
                {
                    _card.GetRenderer(i).material.SetFloat(_thresholdName, 0.0f);
                }

                for (int i = 0; i < _soldiers.Length; i++)
                {
                    _soldiers[i].SetCharacterAlpha(1.0f);
                }

                if (_info._isOnlyVisual)
                {
                    _card._soldierVisual.SetCharacterAlpha(1.0f);
                }
                _info._isReady = true;
                _card._initPos = transform.position;
                _card.Change(null);
            }
        }

        private void UpdateCardDissolve(float timeRate)
        {
            _alpha = 1.0f - timeRate;
            float dissolveRate = Easing.QuadIn(_alpha, 1.0f, 0.0f, 1.0f);
            for (int i = 0; i < _card.MaxRendererCount(); i++)
            {
                _card.GetRenderer(i).material.SetFloat(_thresholdName, dissolveRate);
            }

            Color color = _card._prepareTimeText.color;
            color.a = 1.0f - dissolveRate;
            _card._prepareTimeText.color = color;

            float rate = Easing.CircOut(_alpha, 1.0f, 0.0f, 1.0f);
            _posY = rate * _localScaleY - _localScaleY + _initLocalPosY;

            for (int i = 0; i < _soldiers.Length; i++)
            {
                //_soldiers[i].SetCharacterAlpha(_alpha);
                _soldiers[i].SetLocalPositionY(_posY);
            }
        }

        private void OnEffect()
        {
            if (_isEffect) return;
            if (_alpha < 0.95f) return;

            GameObject explisionObject = Manager.GameMgr.Instance._GameSetting._EffectData?._SoldierAppear;
            Vector3 pos = transform.position;
            Vector3 cameraVec = (Camera.main.transform.position - transform.position).normalized;
            pos += cameraVec;
            Manager.EffectManager.Instance.StartEffect(explisionObject, pos, 2.0f);
            _isEffect = true;

            _card._preparePosition.SetSummonEffect(false);
        }
    }
}