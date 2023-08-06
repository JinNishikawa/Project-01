using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
/**
* @file Card.cs
* @brief �J�[�h�N���X
*/

/** �J�[�h�p���O��� */
namespace Card
{
    /** �J�[�h��� */
    public enum CardState
    {
        None,           /**< �Ȃ� */
        Move,           /**< �ړ� */
        Ready,          /**< ���� */
        Hand,           /**< ��D */
        Wait,           /**< �ҋ@ */
        Departure,      /**< �ݒu */
        Deck            /**< �f�b�L */
    }

    /** �J�[�h���x�^�C�v */
    public enum SpeedStep
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,

        MaxStep
    }

    public enum Rarity
    {
        R,
        SR
    }

    public enum MeshType
    {
        Front,
        Line,
        Back,
        Illust,
        
        MaxType
    }

    /** �J�[�h */
    public class Card : MonoBehaviour
    {
        /** ��� */
        private State _state = null;

        /** ���m���X�g */
        private List<GameObject> _soldiersList;

        /** ���m�ۑ��e�I�u�W�F�N�g */
        private GameObject _soldiersParent;

        /** �J�[�h�p�I�u�W�F�N�g */
        private GameObject _cardObject;

        /** �J�[�h�X���[�u�I�u�W�F�N�g */
        [HideInInspector]
        public GameObject _surfaceObject;

        /** �J�[�h��� */
        [HideInInspector]
        public  Info _info;

        /** �����ʒu */
        [HideInInspector]
        public Vector3 _initPos;

        /** �������鏀���ʒu */
        [HideInInspector]
        public Field.PreparePosition _preparePosition;

        /** ���w�F */
        [HideInInspector]
        public Color _playerColor;

        /** �������� */
        [HideInInspector]
        public TextMeshPro _prepareTimeText;

        /** ���x�e�L�X�g */
        [HideInInspector]
        public SettingTexture _speedStar;

        /** �J�[�h���b�V�����X�g */
        private MeshRenderer[] _meshRenderer;

        private Material[] _initMaterials;

        [HideInInspector]
        public float _soldierScale;

        [HideInInspector]
        public Soldier.SoldierVisual _soldierVisual;

        private Manager.GameMgr _gameMgr = null;
        private Manager.DeckEditorManager _deckEditorMgr = null;

        [HideInInspector]
        public bool _isGame;

        private void Awake()
        {
            _gameMgr = Manager.GameMgr.Instance;
            _deckEditorMgr = Manager.DeckEditorManager.Instance;

            //�������\�ʐݒ�
            Data.SystemSetting.GameStyle style = Data.SystemSetting.GameStyle.Ver;
            if (_gameMgr)
            {
                style = _gameMgr._GameSetting._SystemSetting._Style;
            }
            if(_deckEditorMgr)
            {
                style = _deckEditorMgr._GameSetting._SystemSetting._Style;
            }

            if (style == Data.SystemSetting.GameStyle.Ver)
            {
                Vector3 surfaceAngle = Vector3.zero;
                surfaceAngle.z = -90;
                for (int i = 0; i < 2; i++)
                {
                    transform.GetChild(i).localEulerAngles = surfaceAngle;
                }

                // �R���C�_�[�T�C�Y�ύX
                BoxCollider collider = GetComponent<BoxCollider>();
                Vector3 size = collider.size;
                float tmp = size.x;
                size.x = size.y;
                size.y = tmp;
                collider.size = size;
            }


            // ���m���X�g�쐬
            _soldiersList = new List<GameObject>();

            // �J�[�h�I�u�W�F�N�g
            _cardObject = transform.Find("CardSurface").gameObject;

            _info = GetComponent<Info>();

            _prepareTimeText = _cardObject.transform.Find("PrepareTime").GetComponentInChildren<TextMeshPro>();

            _speedStar = _cardObject.transform.Find("SpeedStep").Find("Star").GetComponent<SettingTexture>();

            _surfaceObject = _cardObject.transform.Find("Surface").gameObject;

            _isGame = true;
        }

        private void OnEnable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            InitMeshRenderer();

            // ���X�g�ǉ�
            _soldiersParent = transform.Find("Soldiers").gameObject;
            for (int i = 0; i < _soldiersParent.transform.childCount; i++)
            {
                GameObject soldierObj = _soldiersParent.transform.GetChild(i).gameObject;
                _soldiersList.Add(soldierObj);
            }
            _info._remainCount = _soldiersList.Count;

            if (_info._isOnlyVisual)
            {
                _soldierVisual = transform.Find("SoldierVisual").gameObject.GetComponent<Soldier.SoldierVisual>();
            }

            _playerColor = Color.white;
            if(_gameMgr)
            {
                _playerColor = _gameMgr._GameSetting._PlayerColor[(int)_info._userType];
            }

            if(_deckEditorMgr)
            {
                _playerColor = _deckEditorMgr._GameSetting._PlayerColor[(int)_info._userType];
            }


            // �t���O������
            InitFlag();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Change(State state)
        {
            _state?.Fin();
            Destroy(_state);
            _state = null;

            if (!_isGame) return;

            _state = state;
        }

        void LookAtCamera()
        {
            Vector3 camPos = -Camera.main.transform.position;
            camPos.x = transform.position.x;
            transform.LookAt(camPos);
        }

        public void SetNextState()
        {
            State state = null;
            switch (_info._next)
            {
                case CardState.Ready:
                    {
                        Ready ready = gameObject.AddComponent<Ready>();
                        state = ready;
                    }
                    break;

                case CardState.Departure:
                    {
                        Departure departure = gameObject.AddComponent<Departure>();
                        state = departure;
                    }
                    break;

                case CardState.Hand:
                    {
                        Hand hand = gameObject.AddComponent<Hand>();
                        state = hand;
                    }
                    break;

                case CardState.Wait:
                    {
                        Wait wait = gameObject.AddComponent<Wait>();
                        state = wait;
                    }
                    break;

                case CardState.Deck:
                    {
                        Deck deck = gameObject.AddComponent<Deck>();
                        state = deck;
                    }
                    break;
            }
            Change(state);
        }

        public List<GameObject> GetSoldiersList()
        {
            return _soldiersList;
        }

        public void SetSoldierScale(Vector3 scale)
        {
            _soldiersParent.transform.localScale = scale;
        }

        public void SetSoldierScale(float scaleRate)
        {
            Vector3 scale = Vector3.one * scaleRate;
            SetSoldierScale(scale);
        }

        public void SetCenterSoldiers()
        {
            _soldiersParent.transform.localPosition = Vector3.zero;
        }

        public int GetExistCount()
        {
            int count = 0;
            foreach(GameObject obj in _soldiersList)
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                if (!soldier._isExist) continue;

                count++;
            }

            return count;
        }

        public void InitFlag()
        {

            InitCardMaterial();

            // �����ł��Ă邩
            _info._isReady = false;

            // �����ʒu
            _initPos = transform.position;

            // ���m������
            InitSoldier();

            // �҂�����
            _info._soldierWaitTime = 0.0f;
            if(_gameMgr)
            {
                _info._soldierWaitTime = _gameMgr._GameSetting._SoldierWaitTime[(int)_info._speedStep];
            }
            if(_deckEditorMgr)
            {
                _info._soldierWaitTime = _deckEditorMgr._GameSetting._SoldierWaitTime[(int)_info._speedStep];
            }

            // �������ԃe�L�X�g
            _prepareTimeText.text = _info._readyTime.ToString();

            Texture star = null;
            if (_gameMgr)
            {
                star = _gameMgr._GameSetting._SpeedStar[(int)_info._speedStep];
            }
            if (_deckEditorMgr)
            {
                star = _deckEditorMgr._GameSetting._SpeedStar[(int)_info._speedStep];
            }
            _speedStar.SetTexture(star);
        }

        private void InitSoldier()
        {
            // �ʒu
            Transform cardSurface = transform.Find("CardSurface");
            Transform formation = cardSurface.transform.Find("Formation");
            Vector3 soldierPosition = formation.transform.localPosition;
            Data.SystemSetting.GameStyle style = Data.SystemSetting.GameStyle.Ver;
            if (_gameMgr)
            {
                style = _gameMgr._GameSetting._SystemSetting._Style;
            }
            if (_deckEditorMgr)
            {
                style = _deckEditorMgr._GameSetting._SystemSetting._Style;
            }
            if (style == Data.SystemSetting.GameStyle.Ver)
            {
                float x = soldierPosition.x;
                soldierPosition.x = soldierPosition.y;
                soldierPosition.y = x;
            }
            soldierPosition.z -= 0.001f;
            _soldiersParent.transform.localPosition = soldierPosition;

            // ���[�J�����W
            foreach (GameObject obj in _soldiersList)
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier.InitFlag();
                soldier.transform.SetParent(_soldiersParent.transform, false);
            }

            // �X�P�[��
            _soldierScale = 1.0f;
            if(_gameMgr)
            {
                _soldierScale = _gameMgr._GameSetting._SystemSetting._SoldierCardSize;
            }
            if(_deckEditorMgr)
            {
                _soldierScale = _deckEditorMgr._GameSetting._SystemSetting._SoldierCardSize;
            }
            SetSoldierScale(_soldierScale);
            _soldierVisual?.SetCharacterScale(_soldierScale);

            // �F
            foreach (GameObject obj in _soldiersList)
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier.SetFieldMaterial(Soldier.MaterialType.Unlit);
                soldier.SetFieldColor(_playerColor);
            }
        }


        public void SetCardVisible(bool isVisible)
        {
            _cardObject.SetActive(isVisible);
        }

        public void SetCardMaterial(Material mat)
        {
            for(int i=0;i<_meshRenderer.Length;i++)
            {
                _meshRenderer[i].material = mat;
            }

            SetTexture();
        }

        public int MaxRendererCount()
        {
            return _meshRenderer.Length;
        }

        public MeshRenderer GetRenderer(int index)
        {
            if (index < 0 || _meshRenderer.Length <= index) return null;

            return _meshRenderer[index];
        }

        public void InitCardMaterial()
        {
            for (int i = 0; i < _meshRenderer.Length; i++)
            {
                _meshRenderer[i].material = _initMaterials[i];
            }

            SetTexture();
        }

        public void SetVisibleSoldier(bool isVisible)
        {
            foreach (GameObject obj in _soldiersList)
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier.SetVisibleSprite(Soldier.SoldierMesh.Character, isVisible);
                soldier.SetVisibleSprite(Soldier.SoldierMesh.Field, isVisible);
            }
        }

        public void SetVisibleBase(bool isVisible)
        {
            foreach (GameObject obj in _soldiersList)
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier.SetVisibleSprite(Soldier.SoldierMesh.Field, isVisible);
            }
        }

        public void SetVisibleReadyCharacter(bool isVisible)
        {
            Soldier.Soldier soldier = _soldiersList[0].GetComponent<Soldier.Soldier>();
            soldier.SetVisibleSprite(Soldier.SoldierMesh.Character, isVisible);
            soldier.SetVisibleSprite(Soldier.SoldierMesh.Field, isVisible);
        }

        private void InitMeshRenderer()
        {
            _meshRenderer = new MeshRenderer[(int)MeshType.MaxType];
            _initMaterials = new Material[(int)MeshType.MaxType];

            // �\
            _meshRenderer[(int)MeshType.Front] = _surfaceObject.GetComponent<MeshRenderer>();
            _initMaterials[(int)MeshType.Front] = _meshRenderer[(int)MeshType.Front].material;

            // �g
            _meshRenderer[(int)MeshType.Line] = _surfaceObject.transform.Find("Line").GetComponent<MeshRenderer>();
            _initMaterials[(int)MeshType.Line] = _meshRenderer[(int)MeshType.Line].material;

            // ��
            _meshRenderer[(int)MeshType.Back] = _surfaceObject.transform.Find("Back").GetComponent<MeshRenderer>();
            _initMaterials[(int)MeshType.Back] = _meshRenderer[(int)MeshType.Back].material;

            // �C���X�g
            _meshRenderer[(int)MeshType.Illust] = _cardObject.transform.Find("Illustration").transform.Find("Quad").GetComponent<MeshRenderer>();
            _initMaterials[(int)MeshType.Illust] = _meshRenderer[(int)MeshType.Illust].material;
        }

        private void SetTexture()
        {
            // �\
            Texture surfaceFront = null;
            if(_gameMgr)
            {
                surfaceFront = _gameMgr._GameSetting._CardSurfaceFront[(int)_info._userType];
            }
            if(_deckEditorMgr)
            {
                surfaceFront = _deckEditorMgr._GameSetting._CardSurfaceFront[(int)_info._userType];
            }
            SettingTexture st = _meshRenderer[(int)MeshType.Front].GetComponent<SettingTexture>();
            st.SetTexture(surfaceFront);

            // �g
            Texture lineTexture = null;
            if (_gameMgr)
            {
                lineTexture = _gameMgr._GameSetting._CardLine[(int)_info._userType];
            }
            if (_deckEditorMgr)
            {
                lineTexture = _deckEditorMgr._GameSetting._CardLine[(int)_info._userType];
            }
            st = _meshRenderer[(int)MeshType.Line].GetComponent<SettingTexture>();
            st.SetTexture(lineTexture);

            // ��
            Texture surfaceBack = null;
            if (_gameMgr)
            {
                surfaceBack = _gameMgr._GameSetting._CardSurfaceBack[(int)_info._userType];
            }
            if (_deckEditorMgr)
            {
                surfaceBack = _deckEditorMgr._GameSetting._CardSurfaceBack[(int)_info._userType];
            }
            st = _meshRenderer[(int)MeshType.Back].GetComponent<SettingTexture>();
            st.SetTexture(surfaceBack);

            // �C���X�g
            st = _meshRenderer[(int)MeshType.Illust].GetComponent<SettingTexture>();
            st.SetTexture(_info._illust.texture);
            st.SetScale(Vector2.one);
        }

        public void DestroyState()
        {
            Change(null);
        }

        public void SetCardAlpha(float alpha)
        {
            foreach(MeshRenderer mesh in _meshRenderer)
            {
                Color color = mesh.material.color;
                color.a = alpha;
                mesh.material.color = color;
            }

            MeshRenderer m = _speedStar.GetComponent<MeshRenderer>();
            Color c = m.material.color;
            c.a = alpha;
            m.material.color = c;
        }
    }

}
