using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Wait : State
    {
        /** �������� */
        private bool _isAlive = false;

        /** �J�[�h�I���� */
        private bool _isFin = false;

        /** ���w�F */
        private Color myColor;

        /** �G�w�F */
        private Color enemyColor;

        public override void Awake()
        {
            base.Awake();

            // �҂����Ԑݒ�
            _timer = _info._soldierWaitTime;

            // ��������
            _isAlive = false;
            _isFin = false;

            Vector3 pos = transform.position;
            pos.y = 0.0f;
            transform.position = pos;

            myColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)_info._userType];
            Manager.UserType enemyType = Manager.UserType.Player1;
            if(enemyType == _info._userType)
            {
                enemyType = Manager.UserType.Player2;
            }
            enemyColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._BattleFieldColors[(int)enemyType];
        }

        public override void Start()
        {
            base.Start();

            _isAlive = CheckFinCard();
            if(_isFin)
            {
                _timer = 0.0f;
            }

            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                if (!soldier._isAlive) continue;

                Soldier.Wait wait = soldier.gameObject.AddComponent<Soldier.Wait>();
                soldier.Change(wait);
            }

            if(_info._isOnlyVisual)
            {
                Soldier.Wait wait = _card._soldierVisual.gameObject.AddComponent<Soldier.Wait>();
                _card._soldierVisual.Change(wait);
            }

            //Manager.UserManager.Instance._characterInfo[(int)_info._userType].CheckFrontPosition();
            Manager.UserManager.Instance._characterInfo[(int)_info._userType].CheckFrontPositions();
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
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
               
                if(soldier.GetComponent<Soldier.Attack>())
                {
                    continue;
                }
                if(soldier.GetComponent<Soldier.UserAttack>())
                {
                    continue;
                }
                if (!soldier._isExist)
                {
                    soldier.Change(null);
                    continue;
                }
                if (_card.GetExistCount() <= 0)
                {
                    soldier.Change(null);
                    continue;
                }

                Soldier.Move move = soldier.gameObject.AddComponent<Soldier.Move>();
                soldier.Change(move);
            }

            if (_info._isOnlyVisual)
            {
                Soldier.Move move = _card._soldierVisual.gameObject.AddComponent<Soldier.Move>();
                _card._soldierVisual.Change(move);
            }

            base.Fin();
        }

        private void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer < 0.0f)
            {
                float time = 0.0f;
                if (!_isFin && CheckNextCell()) return;

                // �ړ��X�e�[�g�ǉ�
                Move move = gameObject.AddComponent<Move>();
                Vector3 nextPos;
                if (_isFin)
                {
                    Manager.UserManager.Instance._characterInfo[(int)_info._userType].RemoveFieldList(gameObject);
                    // �S��
                    if (!_isAlive)
                    {
                        _info._next = CardState.None;
                        _card.Change(null);
                        Destroy(gameObject);
                        return;
                    }

                    // ���m������
                    nextPos = _characterInfo._deck.transform.position;

                    // �X�P�[��
                    float soldierSize = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierCardSize;
                    _card.SetSoldierScale(soldierSize);
                    time = Manager.GameMgr.Instance._GameSetting._MoveToDeckTime;
                    _info._next = CardState.Deck;
                    _card.transform.GetChild(0).gameObject.SetActive(true);

                }
                else
                {
                    int tileType = (int)Field.Battle.TileType.Battle;

                    // �i�s����
                    int dir = _card._info._moveDir;
                    // ���̃Z���ʒu
                    Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], transform.position);
                    // �ړ�
                    nextCellPos.x += dir;
                    // ���[���h���W�֕ϊ�
                    nextPos = TilemapFunction.GetWorldPos(Field.Battle.Instance._tiles[tileType], nextCellPos);
                    time = Manager.GameMgr.Instance._GameSetting._FieldMoveTime;

                    _info._next = CardState.Wait;
                }

                move.StartMove(nextPos, time);
                _card.Change(move);
            }
        }

        private bool CheckFinCard()
        {
            // ���m��
            if (_card.GetExistCount() > 0) return false;

            _isFin = true;
            _timer = 0.0f;
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                if (!soldier._isAlive) continue;
                return true;
            }

            return false;
        }

        private bool CheckNextCell()
        {
            // �^�C���^�C�v
            int tileType = (int)Field.Battle.TileType.Battle;
            // �i�s����
            int dir = _card._info._moveDir;

            foreach (GameObject obj in _card.GetSoldiersList())
            {
                // ���̈ʒu
                Vector3 pos = obj.transform.localPosition * dir + transform.position;

                // ���̃Z���ʒu
                Vector3Int nextCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], pos);
                // �ړ�
                nextCellPos.x += dir;

                Soldier.Soldier soldier = obj.GetComponent<Soldier.Soldier>();
                soldier._iconObj.SetActive(false);

                // ���̈ʒu�̃^�C���Ȃ�
                if (!Field.Battle.Instance._tiles[tileType].HasTile(nextCellPos)) continue;

                // �����J�[�h
                if (CheckSameCard(obj, nextCellPos)) continue;

                // �G�̃J�[�h
                if (CheckEnemyCard(nextCellPos)) continue;


                bool isDebug = Manager.DebugManager.Instance.GetFlag();
                if (isDebug)
                {
                    soldier._iconObj.SetActive(true);
                }

                // ���̃J�[�h������
                return true;
            }

            // ���̃J�[�h�Ȃ�
            return false;
        }

        private bool CheckSameCard(GameObject checkObj,Vector3Int nextCellPos)
        {
            // �^�C���^�C�v
            int tileType = (int)Field.Battle.TileType.Battle;
            // �i�s����
            int dir = _card._info._moveDir;

            // �����J�[�h���ǂ���
            foreach (GameObject obj in _card.GetSoldiersList())
            {
                if (checkObj == obj) continue;
                Vector3 otherPos = obj.transform.localPosition * dir + transform.position;
                Vector3Int otherCellPos = TilemapFunction.GetCellsPos(Field.Battle.Instance._tiles[tileType], otherPos);
                if (otherCellPos != nextCellPos) continue;

                // �����J�[�h
                return true;
            }

            return false;
        }

        private bool CheckEnemyCard(Vector3Int nextCellPos)
        {
            // �^�C���^�C�v
            int tileType = (int)Field.Battle.TileType.Battle;
            // �i�s����
            int dir = _card._info._moveDir;

            Color nextColor = Field.Battle.Instance._tiles[tileType].GetColor(nextCellPos);

            if (nextColor == enemyColor)
            {
                return true;
            }

            return false;
        }
    }
}