using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class UserAttack : State
    {
        enum AttackType
        {
            Jump,
            Move,
            Attack,
            Wait
        }


        Field.LifeMeter _meter;
        GameObject _lifeScale;
        private Vector3 _lastPos;

        private Vector3 _start;
        private Vector3 _goal;

        private AttackType _type;
        private float _height = 10.0f;

        private bool _lastLife = false;

        public override void Awake()
        {
            base.Awake();
            Manager.UserType enemyType = Manager.UserType.Player1;
            if (enemyType == _soldier._info._userType)
            {
                enemyType = Manager.UserType.Player2;
            }
            _meter = Manager.UserManager.Instance._characterInfo[(int)enemyType]._lifeMeter;
            _lifeScale = _meter.GetLifeScale();
            _lastLife = false;
            if (_lifeScale)
            {
                // ç≈å„ÇÃHP
                LifeScale lifeScale = _lifeScale.GetComponent<LifeScale>();
                if (lifeScale && lifeScale.GetLastFlag())
                {
                    _lastLife = true;
                }
            }
            _lastPos = transform.position;
            _start = transform.position;
            _goal = transform.position;
            _goal.y = _height;

            float scale = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierFieldSize;
            _soldier.SetCharacterScale(scale);
            _soldier.SetVisibleSprite(SoldierMesh.Character, true);            

            _type = AttackType.Jump;
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos, null);
            _soldier._boxCollider.SetActive(false);

            RemainSoldier();
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

            //_soldier.SetBaseSprite(false);
        }

        public override void SetTimer(float time)
        {
            base.SetTimer(time);
        }

        private void RemainSoldier()
        {
            _soldier._info._remainCount--;
            if (_soldier._info._remainCount <= 0)
            {
                if (_soldier._info._isOnlyVisual)
                {
                    SoldierVisual sv = GetComponentInParent<Card.Card>()._soldierVisual;
                    sv.SetVisibleSprite(false);
                    sv.gameObject.SetActive(false);
                    sv.Change(null);
                }
            }
        }

        private void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;

            switch (_type)
            {
                case AttackType.Jump:
                    if (_timer < 0.0f)
                    {
                        _type = AttackType.Move;
                        SetTimer(0.5f);
                        _goal = transform.position;
                        if (_lifeScale != null)
                        {
                            _goal = _lifeScale.transform.position;
                            _goal.y = transform.position.y;
                        }
                        else
                        {
                            transform.localPosition = _soldier._initLocalPosition;
                            _soldier._isExist = false;
                            _soldier.SetVisibleSprite(SoldierMesh.Character, false);
                            _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                            _soldier.Change(null);
                        }
                        _start = transform.position;
                    }
                    break;
                case AttackType.Attack:

                    if(_timer < _maxTime * 0.1f)
                    {
                        if (_lastLife)
                        {
                            Manager.GameMgr.Instance.SetSystemSpeed(0.1f);
                        }
                    }

                    if (_timer < 0.0f)
                    {
                        _type = AttackType.Wait;
                        _meter?.Damage(_lifeScale);
                        SetTimer(0.5f);

                        if (_lastLife)
                        {
                            Manager.GameMgr.Instance.SetSystemSpeed(1.0f);
                        }

                        Explosion();
                    }
                    break;
                case AttackType.Wait:
                    transform.position = _goal;
                    if (_timer < 0.0f)
                    {
                        transform.localPosition = _soldier._initLocalPosition;
                        _soldier._isExist = false;
                        _soldier.SetVisibleSprite(SoldierMesh.Character, false);
                        _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                        _soldier.Change(null);
                    }
                    return;
                case AttackType.Move:
                    if (_timer < 0.0f)
                    {
                        _type = AttackType.Attack;
                        SetTimer(_maxTime);
                        _goal = _lastPos;
                        if (_lifeScale != null)
                        {
                            _goal = _lifeScale.transform.position;
                        }
                        _start = transform.position;
                        _start.x = _goal.x;
                        _start.z = _goal.z;
                    }
                    break;
            }

            float rate = Mathf.Clamp01((_maxTime - _timer) / _maxTime);
            if (_type == AttackType.Jump)
            {
                rate = Easing.QuartOut(rate, 1.0f, 0.0f, 1.0f);
            }
            else if (_type == AttackType.Attack)
            {
                rate = Easing.ExpIn(rate, 1.0f, 0.0f, 1.0f);
            }
            else if (_type == AttackType.Move)
            {
                rate = Easing.QuartIn(rate, 1.0f, 0.0f, 1.0f);
            }

            // à⁄ìÆ
            Vector3 nextPos = Vector3.Lerp(_start, _goal, rate);
            if (_type == AttackType.Move)
            {
                nextPos.y += Mathf.Sin(rate * Mathf.PI) * 3.0f;
            }
            transform.position = nextPos;
        }

        private void Explosion()
        {
            if (!_lastLife) return;

            GameObject sphere = Resources.Load<GameObject>("Prototype/Field/Other/ExplosionSphere");
            GameObject explosion = Instantiate(sphere, _lifeScale.transform.position, Quaternion.identity);
        }
    }
}