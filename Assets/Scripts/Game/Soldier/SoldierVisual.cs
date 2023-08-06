using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{

    public class SoldierVisual : Soldier
    {
        private GameObject _mesh;

        /** 状態 */
        private State _state;


        private void Awake()
        {
            _info = gameObject.transform.root.GetComponent<Card.Info>();
            _characterAnchor = transform.Find("RotAnchor").gameObject;
            _mesh = _characterAnchor.transform.Find("Character").gameObject;
        }

        private void OnEnable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            Data.SystemSetting.GameStyle style = Data.SystemSetting.GameStyle.Ver;
            if (Manager.GameMgr.Instance)
            {
                style = Manager.GameMgr.Instance._GameSetting._SystemSetting._Style;
            }

            _animation = Instantiate(_info._animation);
            _animation.Start();
            _characterSpriteRenderer = new SpriteRenderer[(int)_mesh.transform.childCount];
            for (int i = 0; i < _mesh.transform.childCount; i++)
            {

                // スプライト設定
                SpriteRenderer renderer = _mesh.transform.GetChild(i).GetComponent<SpriteRenderer>();
                _characterSpriteRenderer[i] = renderer;
                _animation.SetRenderer(_dir[i], renderer);
                _animation.SetSprite(_dir[i], 1);

                if (style == Data.SystemSetting.GameStyle.Hori)
                {
                    if (i >= 2)
                    {
                        _mesh.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (i < 2)
                    {
                        _mesh.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            SetVisibleSprite(false);

            Change(null);

            InitFlag();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetVisibleSprite(bool isFlag)
        {
            _mesh.SetActive(isFlag);
        }

        public override void SetCharacterAlpha(float alpha)
        {
            for (int i = 0; i < _characterSpriteRenderer.Length; i++)
            {
                Color color = _characterSpriteRenderer[i].color;
                color.a = alpha;
                _characterSpriteRenderer[i].color = color;
            }
        }

        public override void SetRotateCharacter(float rotY)
        {
            Vector3 rot = _mesh.transform.localEulerAngles;
            rot.y = rotY;
            _mesh.transform.localEulerAngles = rot;
        }

        public override void SetCharacterRotation(Quaternion quaternion)
        {
            _mesh.transform.localRotation = quaternion;
        }

        public override Quaternion GetCharacterRotation()
        {
            return _mesh.transform.localRotation;
        }

        public override GameObject GetCharacterObject()
        {
            return _mesh.gameObject;
        }

        public override void SetCharacterScale(float rate)
        {
            for(int i=0;i<_mesh.transform.childCount;i++)
            {
                GameObject obj = _mesh.transform.GetChild(i).gameObject;
                obj.transform.localScale = Vector3.one * rate;
            }

            Vector3 pos = _characterAnchor.transform.localPosition;
            pos.y = rate * 0.5f;
            _characterAnchor.transform.localPosition = pos;
        }

        public override void InitFlag()
        {
            SetCharacterScale(_info._soldierScale);
        }

    }
}
