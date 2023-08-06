using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Hand : State
    {
        /** 選択中フラグ */
        private bool _isAction = false;

        //=== 拡縮
        /** 開始スケール */
        private Vector3 _startScale;
        /** 終了スケール */
        private Vector3 _goalScale;

        //=== 位置
        /** 開始位置 */
        private Vector3 _startPos;
        /** 終了位置 */
        private Vector3 _goalPos;

        /** 最小スケール */
        private Vector3 _minScale = Vector3.one;
        /** 最大スケール */
        private Vector3 _maxScale = Vector3.one * 1.5f;
        /** アクション時間 */
        private float _maxTime = 0.5f;
        /** 移動距離 */
        private float _moveRate;

        /** アウトラインエフェクト用オブジェクト */
        private OutLine_eff _outlineEffect;

        //private GameObject _outline;
        //private ParticleSystem _particle;

        public override void Awake()
        {
            base.Awake();

            //GameObject sourceObject = Manager.GameMgr.Instance._GameSetting._EffectData?._OutLine;
            //Vector3 pos = transform.position;
            //_outline = Manager.EffectManager.Instance.StartEffect(sourceObject, _card._surfaceObject.transform.parent);
            ////_outline.transform.localEulerAngles = angle;
            //_particle = _outline.GetComponent<ParticleSystem>();

            _maxScale = Vector3.one * Manager.GameMgr.Instance._GameSetting._MaxScaleRate;
            _maxTime = Manager.GameMgr.Instance._GameSetting._ActionTime;
            _moveRate = Manager.GameMgr.Instance._GameSetting._MoveDistanceRate;
            _card._initPos = transform.position;
            Data.SystemSetting.GameStyle style = Manager.GameMgr.Instance._GameSetting._SystemSetting._Style;

            Vector3 angle = Vector3.zero;
            if (style == Data.SystemSetting.GameStyle.Hori)
            {
                angle = new Vector3(60.0f, 0.0f, 0.0f);           
                if (_info._userType == Manager.UserType.Player2)
                {
                    angle.x = 0;
                    angle.y = 180;
                }
            }
            else
            {
                angle = new Vector3(60.0f, 90.0f, 90.0f);
                if (_info._userType == Manager.UserType.Player2)
                {
                    angle.x = 0;
                    angle.y = -90;
                }
            }
            
            transform.eulerAngles = angle;

            //アウトライン用エフェクト
            GameObject sourceObject = Resources.Load<GameObject>("Prototype/Base/Surface");
            GameObject OutlineObj = Instantiate(sourceObject, transform.position, transform.rotation, _card._surfaceObject.transform);
            OutlineObj.transform.localPosition = _card._surfaceObject.transform.localPosition;
            OutlineObj.transform.localRotation = _card._surfaceObject.transform.localRotation;
            OutlineObj.transform.localScale = Vector3.one;
            _outlineEffect = OutlineObj.GetComponent<OutLine_eff>();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            //_outlineEffect.OnVisible(true);
            //_outline.SetActive(false);
            
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            float time = Time.deltaTime * Manager.GameMgr.Instance._gameSpeed;
            float rate = UpdateRate(time);

            UpdateScale(rate);

            UpdatePosition(rate);
        }

        public override void Fin()
        {
            base.Fin();

            transform.localScale = _minScale;

            _outlineEffect.Fin();
            Destroy(_outlineEffect.gameObject);
            //Manager.EffectManager.Instance.DeleteList(_outline);
        }

        private float UpdateRate(float deltaTime)
        {
            if (!_isAction) return 0.0f;

            // 経過時間加算
            _timer += deltaTime;

            // 割合計算
            float rate = Mathf.Clamp01(_timer / _maxTime);
            return rate;
        }

        private void UpdateScale(float rate)
        {
            if (!_isAction) return;

            // 拡縮
            transform.localScale = Vector3.Lerp(_startScale, _goalScale, rate);

            if (rate >= 1.0f)
            {
                transform.localScale = _goalScale;
                rate = 1.0f;
            }
        }

        private void UpdatePosition(float rate)
        {
            if (!_isAction) return;

            // 位置
            transform.position = Vector3.Lerp(_startPos, _goalPos, rate);

            if (rate >= 1.0f)
            {
                _isAction = false;
                rate = 1.0f;
            }
        }

        public void StartSelect(bool isSelect)
        {
            _isAction = true;
            _timer = 0.0f;

            _startScale = transform.localScale;
            _startPos = transform.position;
            // 選択中
            if (isSelect)
            {
                _goalScale = _maxScale;
                _goalPos = _card._initPos + transform.right * _moveRate;

                _outlineEffect.OnVisible(true);
                //_outline.SetActive(true);
                //_particle.Play();
            }
            else
            {
                _goalScale = _minScale;
                _goalPos = _card._initPos;

                _outlineEffect.OnVisible(false);
                //_outline.SetActive(false);
                //_particle.Stop();
            }
        }

        public void SetNormalScale()
        {
            transform.localScale = _minScale;
            _isAction = false;
        }
    }
}