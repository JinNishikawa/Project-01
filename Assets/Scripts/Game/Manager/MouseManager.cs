using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Manager
{
    public class MouseManager : SingletonMonoBehaviour<MouseManager>
    {

        /** �J�[�h */
        private Card.Card _card = null;

        /** ���ݒ��� */
        private bool _isGrab = false;

        /** �}�E�X�I���I�u�W�F�N�g�̋��� */
        private float _distance = 0.0f;

        /** �����ʒu */
        private Field.PreparePosition _prePos;

        /** �퓬�t�B�[���h�͈͓��t���O */
        private bool _isRange;
        private Vector3 _fieldPosition;

        /** �J�[�h�ړ����� */
        private float _cardMoveTime;

        [SerializeField]
        private float _MaxDistance = 15;

        [SerializeField]
        private float _CardMaxDistance = 6.0f;

        /** ���w�m�F�p���l */
        private float _checkFieldAlpha;

        private float _clickTimer;

        private void Awake()
        {
            _cardMoveTime = Manager.GameMgr.Instance._GameSetting._CardMoveTime;

            _checkFieldAlpha = GameMgr.Instance._GameSetting._MyFieldAlpha;
        }


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // �}�E�X�X�V
            UpdateMouse();

            // ���N���b�N��
            ClickLeft();

            // ���N���b�N�I����
            UpLeft();

            // �J�[�h�X�V��
            UpdateCard();
        }

        private void UpdateMouse()
        {
            if (_isGrab) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray�̒���
            RaycastHit[] hitAll = Physics.RaycastAll(ray, _MaxDistance);

            Debug.DrawRay(ray.origin, ray.direction * _MaxDistance, Color.red);

            Card.Card card = null;
            bool isExistCard = false;
            foreach (RaycastHit hit in hitAll)
            {
                // �J�[�h�N���X�擾
                card = hit.transform.GetComponent<Card.Card>();
                if (card == null) continue;

                if(card._info._userType != UserType.Player1) continue;

                Card.Deck deck = card.GetComponent<Card.Deck>();
                if (deck != null) continue;

                // �ړ���
                Card.Move move = card.GetComponent<Card.Move>();
                if (move != null) continue;

                // �J�[�h�I��
                isExistCard = true;

                // �擾�J�[�h������Ɠ���
                if (_card == card) continue;

                _card = card;
                UserManager.Instance._characterInfo[(int)_card._info._userType]._deck.UnselectCard(_card.gameObject);
                break;
            }

            if(_card && !isExistCard)
            {
                UserManager.Instance._characterInfo[(int)_card._info._userType]._deck.UnselectCard(null);
                _card = null;
            }
        }

        private void ClickLeft()
        {
            if (GameMgr.Instance._currentState != GameState.Game) return;

            if (!Input.GetMouseButtonDown(0)) return;

            if (!_card) return;

            if (_isGrab) return;

            // ������
            Card.Ready ready = _card.GetComponent<Card.Ready>();
            if (ready != null)
            {
                _card = null;
                return;
            }

            _distance = Vector3.Distance(_card.transform.position, Camera.main.transform.position);
            if (_distance > _CardMaxDistance)
            {
                Vector3 vec = (_card.transform.position - Camera.main.transform.position).normalized;
                vec *= _CardMaxDistance;
                _card.transform.position = Camera.main.transform.position + vec;
                _distance = Vector3.Distance(_card.transform.position, Camera.main.transform.position);
            }
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _distance;
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            _card.transform.position = pos;

            if (_card._info._isReady)
            {
                _card.SetCardVisible(false);
                float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierGrabSize;
                _card.SetSoldierScale(soldierSize);
                Cursor.visible = false;

                ChangeCheckTileAlpha(_checkFieldAlpha);
            }

            UserManager.Instance._characterInfo[(int)_card._info._userType]._deck.UnselectCard(_card.gameObject);
            Card.Hand hand = _card.GetComponent<Card.Hand>();
            hand?.SetNormalScale();

            _isGrab = true;

            _clickTimer = 0.0f;
        }

        private void UpLeft()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            if (_card == null) return;

            if (!_isGrab) return;

            // �m�F�t�B�[���h������
            ChangeCheckTileAlpha(0.0f);

            Cursor.visible = true;

            Card.Move move = _card.gameObject.AddComponent<Card.Move>();
            Vector3 destPos = _card._initPos;

            bool isReady = _card._info._isReady;
            Card.CardState nextState = Card.CardState.None;


            if (isReady)
            {
                //===== �J�[�h����OK

                // �����ꏊ�Ȃ�
                _prePos = null;
                float fieldSize = 1.0f;
                // �퓬�t�B�[���h�͈͓�
                if (_isRange)
                {
                    // ���̏��
                    nextState = Card.CardState.Departure;
                    // �ړI�n
                    destPos = _fieldPosition;

                    // ���m�T�C�Y�퓬�T�C�Y
                    //filedSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierFieldSize;
                    fieldSize = transform.localScale.x;
                }
                else
                {

                    // ���m�T�C�Y�����T�C�Y
                    fieldSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierReadySize;
                }
               
                _card.SetSoldierScale(fieldSize);
            }
            else
            {
                //===== �J�[�h����NG

                if(_prePos == null)
                {
                    if (_clickTimer < 0.5f)
                    {
                        Field.Prepare prepare = Manager.UserManager.Instance._characterInfo[((int)_card._info._userType)]._prepare;
                        Field.PreparePosition prePos = prepare.GetPreparePosition(false);
                        _prePos = prePos;
                        if (_prePos)
                        {
                            int index = _prePos.GetIndex();
                            prepare.SetExecFlag(index, true);
                            _prePos.OnCursor();
                        }
                    }
                }

                if (_prePos != null)
                {
                    //===== �����ꏊ����

                    // �ړI�n
                    destPos = _prePos.transform.position;
                    destPos.y += 0.001f;
                    // ���̏��
                    nextState = Card.CardState.Ready;


                    // �J�[�h�̏����ꏊ�ݒ�
                    _card._preparePosition = _prePos;
                }
                else
                {
                    //==== �����ꏊ�Ȃ�
                    nextState = Card.CardState.Hand;
                }
            }

            _card._info._next = nextState;
            move.StartMove(destPos, _cardMoveTime);
            _card.Change(move);
            _card = null;
            _isGrab = false;
            _clickTimer = 0.0f;

            // �I���t�B�[���h������
            int tileType = (int)Field.Battle.TileType.Select;
            TilemapFunction.SetClearAllTile(Field.Battle.Instance._tiles[tileType]);
        }

        private void UpdateCard()
        {
            if (_card == null) return;

            if (!_isGrab) return;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _distance;
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            _card.transform.position = pos;

            _clickTimer += Time.deltaTime;

            UpdatePrepare();
            UpdateField();
        }


        private void UpdatePrepare()
        {
            // ����OK
            if (_card._info._isReady) return;

            if (_prePos == null)
            {
                UserManager.Instance._characterInfo[(int)_card._info._userType]._prepare.UpdateNormalColor();
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray�̒���

            RaycastHit[] hitAll = Physics.RaycastAll(ray, _MaxDistance);
            _prePos = null;
            foreach (RaycastHit hit in hitAll)
            {
                Card.Card card = hit.transform.GetComponent<Card.Card>();
                if (card == _card) continue;

                _prePos = hit.transform.GetComponent<Field.PreparePosition>();
                if (_prePos == null) continue;

                Field.Prepare prepare = _prePos.transform.root.GetComponent<Field.Prepare>();
                if (prepare._Type != _card._info._userType)
                {
                    _prePos = null;
                    continue;
                }

                if (prepare.GetExecFlag(_prePos.GetIndex()))
                {
                    _prePos = null;
                    continue;
                }

                _prePos.OnCursor();
                break;
            }
        }

        private void UpdateField()
        {
            // ������
            if (!_card._info._isReady) return;

            int tileType = (int)Field.Battle.TileType.Select;

            int battleTileType = (int)Field.Battle.TileType.Battle;

            int checkTile = (int)Field.Battle.TileType.P1Check;
            if(_card._info._userType == UserType.Player2)
            {
                checkTile = (int)Field.Battle.TileType.P2Check;
            }

            Vector3 mousePosition = Input.mousePosition;
            //mousePosition.y -= 150.0f;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            // �I���t�B�[���h������
            TilemapFunction.SetClearAllTile(Field.Battle.Instance._tiles[tileType]);

            //Ray�̒���
            _isRange = false;
            RaycastHit[] hitAll = Physics.RaycastAll(ray, _MaxDistance);
            Debug.DrawRay(ray.origin, ray.direction * _MaxDistance, Color.red);

            foreach (RaycastHit hit in hitAll)
            {
                Card.Card card = hit.transform.GetComponent<Card.Card>();

                if (card == _card) continue;

                //// �J�[�h�ړI�n
                //Vector3 cardDestPos = hit.point;
                //cardDestPos.y = 1.0f;
                //// �J�����ʒu
                //Vector3 cameraPos = Camera.main.transform.position;
                //cameraPos.x = hit.point.x;

                //float distance = Vector3.Distance(cardDestPos, cameraPos);
                //Vector3 mousePos = Input.mousePosition;
                //mousePos.z = _distance;
                //Vector3 cardPos = Camera.main.ScreenToWorldPoint(mousePos);
                //_card.transform.position = cardPos;

                float tileScale = Field.Battle.Instance._tileScale;

                _isRange = true;
                // ����
                foreach (GameObject obj in _card.GetSoldiersList())
                {
                    Vector3 pos = hit.point + obj.transform.localPosition * _card._info._moveDir * tileScale;

                    // �ݒu�t���O
                    bool isSetting = TilemapFunction.HasTile(Field.Battle.Instance._tiles[battleTileType], pos);
                    // �͈̓t���O
                    bool isRange = TilemapFunction.IsRange(Field.Battle.Instance._tiles[tileType], pos);
                    // �ݒu�͈�
                    bool isMyRange = TilemapFunction.HasTile(Field.Battle.Instance._tiles[checkTile], pos);

                    // �͈͓� & ���ݒu & �ݒu�͈͓�
                    if (isRange && !isSetting && isMyRange) continue;

                    // �t���O�ύX
                    _isRange = false;
                    break;
                }

                Color tileColor = Color.white;
                if (!_isRange)
                {
                    tileColor = Color.red;
                }

                // �^�C���ݒu
                foreach (GameObject obj in _card.GetSoldiersList())
                {
                    Vector3 pos = hit.point + obj.transform.localPosition * _card._info._moveDir * tileScale;
                    // �͈͊O
                    if (!TilemapFunction.IsRange(Field.Battle.Instance._tiles[tileType], pos)) continue;
                    Field.Battle.Instance.SetTile(Field.Battle.TileType.Select, pos);
                    Field.Battle.Instance.SetColorTile(Field.Battle.TileType.Select, pos, tileColor);
                }


                // �t�B�[���h�ʒu�ݒ�
                if (_isRange)
                {
                    tileType = (int)Field.Battle.TileType.Battle;
                    _fieldPosition = TilemapFunction.GetCellsWorldPos(Field.Battle.Instance._tiles[tileType], hit.point);
                }

                break;
            }
        }

        private void ChangeCheckTileAlpha(float alpha)
        {
            if (_card == null) return;

            Field.Battle.TileType checkTile = Field.Battle.TileType.P1Check;
            if (_card._info._userType == UserType.Player2)
            {
                checkTile = Field.Battle.TileType.P2Check;
            }
            Field.Battle.Instance.SetSpriteAlpha(checkTile, alpha);
        }

        public void InitFlag()
        {
            _card = null;

            _isGrab = false;

            _distance = 0.0f;
        }
    }
}