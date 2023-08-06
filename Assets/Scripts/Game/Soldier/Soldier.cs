using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public enum SoldierMesh
    {
        Character,
        Field,
        MaxMesh
    }

    public enum MaterialType
    {
        Lit,
        Unlit
    }

    public class Soldier : MonoBehaviour
    {
        private GameObject[] _mesh;

        /** 当たり判定 */
        [HideInInspector]
        public GameObject _boxCollider;

        // アニメーション
        [HideInInspector]
        public SoldierAnimation _animation;

        /** 状態 */
        private State _state;

        [HideInInspector]
        public Card.Info _info;

        /** 生きてるか */
        [HideInInspector]
        public bool _isAlive;

        /** 描画中 */
        [HideInInspector]
        public bool _isExist;

        [HideInInspector]
        public Soldier _backSoldier = null;

        [HideInInspector]
        public Vector3 _initLocalPosition;

        [HideInInspector]
        public float _characterLocalPositionY;

        [HideInInspector]
        public float _characterLocalScaleY;

        [HideInInspector]
        static public SoldierAnimation.SpriteDir[] _dir = {
                SoldierAnimation.SpriteDir.Right,
                SoldierAnimation.SpriteDir.Left,
                SoldierAnimation.SpriteDir.Front,
                SoldierAnimation.SpriteDir.Back
        };

        // キャラクタースプライトレンダラー
        protected SpriteRenderer[] _characterSpriteRenderer;

        [HideInInspector]
        public GameObject _characterAnchor;


        private GameObject _soldierBase;

        private GameObject _lineBase;

        [HideInInspector]
        public GameObject _iconObj;

        /** タイルマップ位置 */
        [HideInInspector]
        public Vector3 _storePos;

        private Manager.GameMgr _gameMgr = null;
        private Manager.DeckEditorManager _deckEditorMgr = null;

        private void Awake()
        {

            _gameMgr = Manager.GameMgr.Instance;
            _deckEditorMgr = Manager.DeckEditorManager.Instance;

            _initLocalPosition = transform.localPosition;

            _info = gameObject.transform.root.GetComponent<Card.Info>();

            _boxCollider = transform.GetChild(2).gameObject;

            _mesh = new GameObject[(int)SoldierMesh.MaxMesh];
            //for (int i = 0; i < (int)SoldierMesh.MaxMesh; i++)
            //{
            //    _mesh[i] = transform.GetChild(i).gameObject;
            //}
            _characterAnchor = transform.Find("RotAnchor").gameObject;
            _mesh[(int)SoldierMesh.Character] = _characterAnchor.transform.Find("Character").gameObject;
            _mesh[(int)SoldierMesh.Field] = transform.Find("Field").gameObject;

            _soldierBase = _mesh[(int)SoldierMesh.Field].transform.Find("Base").gameObject;
            _lineBase = _mesh[(int)SoldierMesh.Field].transform.Find("Line").gameObject;

            _characterLocalPositionY = _mesh[(int)SoldierMesh.Character].transform.localPosition.y;


            _iconObj = transform.Find("Icon").gameObject;
            _iconObj.SetActive(false);
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

            GameObject mesh = _mesh[(int)SoldierMesh.Character].gameObject;
            _animation.Start();
            _characterSpriteRenderer = new SpriteRenderer[(int)mesh.transform.childCount];
            for (int i = 0; i < mesh.transform.childCount; i++)
            {

                // スプライト設定
                SpriteRenderer renderer = mesh.transform.GetChild(i).GetComponent<SpriteRenderer>();
                _characterSpriteRenderer[i] = renderer;
                _animation.SetRenderer(_dir[i], renderer);
                _animation.SetSprite(_dir[i], 1);

                if (style == Data.SystemSetting.GameStyle.Hori)
                {
                    if (i >= 2)
                    {
                        mesh.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (i < 2)
                    {
                        mesh.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                _characterLocalScaleY = renderer.transform.localScale.y;
            }
            SetVisibleSprite(SoldierMesh.Character, false);

            Change(null);

            InitFlag();
        }

        // Update is called once per frame
        void Update()
        {
            DebugLine();
        }

        public void Change(State state)
        {
            _state?.Fin();
            Destroy(_state);
            _state = null;

            _state = state;
        }

        void LookAtCamera()
        {
            Vector3 camPos = Camera.main.transform.position;
            camPos.y = transform.position.y;
            _mesh[(int)SoldierMesh.Character].transform.LookAt(camPos);
        }

        public void SetVisibleSprite(SoldierMesh type, bool isFlag)
        {
            _mesh[(int)type].SetActive(isFlag);
        }

        public virtual void SetCharacterAlpha(float alpha)
        {
            for (int i = 0; i < _characterSpriteRenderer.Length; i++)
            {
                Color color = _characterSpriteRenderer[i].color;
                color.a = alpha;
                _characterSpriteRenderer[i].color = color;
            }
        }

        public void SetFieldColor(Color color)
        {
            //_mesh[(int)SoldierMesh.Field].GetComponent<MeshRenderer>().material.color = color;
            _soldierBase.GetComponent<MeshRenderer>().material.color = color;
        }

        public void SetFieldMaterial(MaterialType type)
        {
            Material mat = null;
            if(_gameMgr)
            {
                mat = _gameMgr._GameSetting._SystemSetting._Material[(int)type];
            }
            if(_deckEditorMgr)
            {
                mat = _deckEditorMgr._GameSetting._SystemSetting._Material[(int)type];
            }
            _soldierBase.GetComponent<MeshRenderer>().material = mat;
        }

        public void SetLocalPositionY(float posY)
        {
            Vector3 pos = _mesh[(int)SoldierMesh.Character].transform.localPosition;
            pos.y = posY;
            _mesh[(int)SoldierMesh.Character].transform.localPosition = pos;
        }

        public virtual void SetRotateCharacter(float rotY)
        {
            Vector3 rot = _mesh[(int)SoldierMesh.Character].transform.localEulerAngles;
            rot.y = rotY;
            _mesh[(int)SoldierMesh.Character].transform.localEulerAngles = rot;
        }

        public virtual void SetCharacterRotation(Quaternion quaternion)
        {
            _mesh[(int)SoldierMesh.Character].transform.localRotation = quaternion;
        }

        public virtual Quaternion GetCharacterRotation()
        {
            return _mesh[(int)SoldierMesh.Character].transform.localRotation;
        }

        public virtual GameObject GetCharacterObject()
        {
            return _mesh[(int)SoldierMesh.Character].gameObject;
        }

        public virtual void SetCharacterScale(float rate)
        {
            for(int i=0;i<_mesh[(int)SoldierMesh.Character].transform.childCount;i++)
            {
                GameObject obj = _mesh[(int)SoldierMesh.Character].transform.GetChild(i).gameObject;
                obj.transform.localScale = Vector3.one * rate;
            }

            Vector3 pos = _characterAnchor.transform.localPosition;
            pos.y = rate * 0.5f;
            _characterAnchor.transform.localPosition = pos;

            pos.y = rate;
            Vector3 worldPos = transform.TransformPoint(pos);
            _iconObj.transform.position = worldPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Soldier") return;

            Soldier soldier = other.GetComponentInParent<Soldier>();
            if (soldier._info._userType == _info._userType) return;

            soldier.OnDeath();

        }

        public void OnDeath()
        {
            _isAlive = false;

            Card.Card card = GetComponentInParent<Card.Card>();
            foreach(GameObject obj in card.GetSoldiersList())
            {
                Soldier soldier = obj.GetComponent<Soldier>();
                if(soldier._backSoldier == this)
                {
                    soldier._backSoldier = null;
                }
            }

            // 攻撃ステートへ変更
            Attack attack = gameObject.AddComponent<Attack>();
            attack.SetTimer(2.0f);
            Change(attack);

            if(_info._isOnlyVisual)
            {
                SoldierVisual sv = card._soldierVisual;
                attack = null;
                attack = sv.GetComponent<Attack>();
                if (attack == null)
                {
                    attack = sv.gameObject.AddComponent<Attack>();
                    attack.SetTimer(2.0f);
                    sv.Change(attack);
                }
            }
        }

        public virtual void InitFlag()
        {
            _isAlive = true;

            _isExist = true;

            transform.localPosition = _initLocalPosition;

            _boxCollider.SetActive(false);

            SetVisibleSprite(SoldierMesh.Field, true);

            _backSoldier = null;

            SetCharacterScale(_info._soldierScale);

            //SetBaseSprite(false);
        }

        public void CheckFront(Card.Card card)
        {
            //if (_info._userType == Manager.UserType.Player2) return;

            foreach(GameObject obj in card.GetSoldiersList())
            {
                if (obj == gameObject) continue;

                Vector3 vec = transform.localPosition - obj.transform.localPosition;
                if(Vector3.Dot(vec,transform.right * _info._moveDir) < 0.99f) continue;
                if (vec.magnitude > 1.1f) continue;

                _backSoldier = obj.GetComponent<Soldier>();
                break;
            }
        }

        public void SetBaseSprite(bool isVisible)
        {
            _soldierBase.SetActive(!isVisible);
            _lineBase.SetActive(!isVisible);
        }


        private void DebugLine()
        {
            if (!_backSoldier) return;
            if (!_backSoldier._isAlive) return;

            Vector3 start = transform.position;
            start.y += transform.localScale.y * 0.55f;
            Vector3 end = _backSoldier.transform.position;
            end.y += _backSoldier.transform.localScale.y * 0.45f;
            Debug.DrawLine(start, end, Color.red);
        }

        public void DeleteStorePos()
        {
            Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _storePos, null);
        }

    }
}
