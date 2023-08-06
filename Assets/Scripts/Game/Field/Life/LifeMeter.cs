using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
namespace Field
{
    public class LifeMeter : MonoBehaviour
    {

        /** ユーザータイプ */
        [SerializeField]
        private Manager.UserType _Type;

        /** 目盛り元オブジェクト */
        private GameObject _scaleBase;

        /** ライフ */
        private int _life = 0;

        /** 描画テキスト */
        //private TextMeshPro _text;

        /** 目盛りリスト */
        private List<GameObject> _lifeScale;

        private void Awake()
        {
            //_text = GetComponentInChildren<TextMeshPro>();

            _scaleBase = Resources.Load<GameObject>("Prototype/Field/Life/ScaleSphere");
            _lifeScale = new List<GameObject>();

            CreateMeter();
        }

        private void OnEnable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            // キャラクター情報設定
            Manager.UserManager.Instance._characterInfo[(int)_Type]._lifeMeter = this;
        }

        // Update is called once per frame
        void Update()
        {
            //UpdateMeter();
        }

        public void InitFlag()
        {
            InitLife();
        }

        private void UpdateMeter()
        {
            if (_life == _lifeScale.Count) return;

            GameObject life = _lifeScale[0];
            _lifeScale.Remove(life);
            Destroy(life);

            //_text.text = _life.ToString();
        }

        private void CreateMeter()
        {
            InitScaleList();

            GameObject meter = transform.GetChild(1).gameObject;
            GameObject meterBase = meter.transform.GetChild(0).gameObject;

            GameObject scaleParent = meter.transform.GetChild(1).gameObject;
            float maxSize = meterBase.transform.localScale.y;

            float meterScaleY = maxSize / _life;
            if (meterScaleY > 0.5f) meterScaleY = 0.5f;
            for (int i = 0; i < _life; i++)
            {
                float halfLife = _life / 2.0f;
                GameObject scaleObj = Instantiate(_scaleBase, scaleParent.transform);
                Vector3 pos = scaleObj.transform.localPosition;
                pos.z = -(meterScaleY * (i - halfLife) + meterScaleY * 0.5f);
                scaleObj.transform.localPosition = pos;
                Vector3 scale = scaleObj.transform.localScale;
                scale.x = meterScaleY - 0.2f;
                scale.y = meterScaleY - 0.2f;
                scale.z = meterScaleY - 0.2f;
                scaleObj.transform.localScale = scale;

                _lifeScale.Add(scaleObj);
            }

            if (_lifeScale.Count > 0)
            {
                _lifeScale[_lifeScale.Count - 1].GetComponent<LifeScale>().SetLastFlag();
            }

            //_text.text = _life.ToString();
        }

        private void InitScaleList()
        {
            foreach(GameObject life in _lifeScale)
            {
                Destroy(life);
            }

            _lifeScale.Clear();
        }

        private void SetLife(int life)
        {
            _life = life;
            CreateMeter();
        }

        public int GetLife()
        {
            return _life;
        }

        private void InitLife()
        {
            int life = Manager.UserManager.Instance._CharacterStatus[(int)_Type]._Life;
            // 生存設定
            SetLife(life);
        }

        public void Damage()
        {
            _life--;
            if(_life < 0)
            {
                _life = 0;
            }
        }

        public void Damage(GameObject lifeScale)
        {
            if (lifeScale == null) return;

            _lifeScale.Remove(lifeScale);
            lifeScale.GetComponent<LifeScale>().OnAttack();
            //Destroy(lifeScale);
            _life = _lifeScale.Count;
        }

        public GameObject GetLifeScale()
        {
            int max = _lifeScale.Count;
            for(int i=0;i<max;i++)
            {
                GameObject life = _lifeScale[i];
                LifeScale scale = life.GetComponent<LifeScale>();
                if (!scale.GetExist()) continue;
                scale.SetFlag();
                return life;
            }

            return null;
        }
    }
}