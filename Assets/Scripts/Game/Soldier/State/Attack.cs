using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Soldier
{
    public class Attack : State
    {
        private Rigidbody _rb;

        //private Vector3 _moveVec;
        private float _angleSpeed = 720.0f;
        private bool _isFly;
        private Vector3 _hitPosition;

        public override void Awake()
        {
            base.Awake();
            _isFly = false;
            _hitPosition = transform.position;
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            if(_soldier is SoldierVisual)
            {

            }
            else if (_soldier is Soldier)
            {
                //=== ëOà íuÉ^ÉCÉãçÌèú
                Field.Battle.Instance.SetTile(Field.Battle.TileType.Battle, _soldier._storePos, null);

                transform.localPosition = _soldier._initLocalPosition;
                _rb = GetComponent<Rigidbody>();
                _soldier._boxCollider.SetActive(false);
                _soldier.SetVisibleSprite(SoldierMesh.Field, false);
            }

            _soldier._animation.SetAnimationType(SoldierAnimation.AnimType.Attack);
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
            if (_soldier is SoldierVisual)
            {
                _soldier.SetCharacterAlpha(1.0f);
            }
        }

        public void AddForce()
        {
            _rb.useGravity = true;

            float speed = Manager.GameMgr.Instance._GameSetting._SoldierForceSpeed;
            Vector3 upVec = Vector3.up * Random.Range(0.5f, 1.0f);
            Vector3 backVec = (Vector3.right * -1 * _soldier._info._moveDir) * Random.Range(0.5f, 1.5f);
            Vector3 rightVec = Vector3.forward * Random.Range(-0.8f, 0.8f);
            Vector3 dir = upVec + backVec + rightVec;

            _rb.AddForce(_rb.mass * dir * speed / Time.deltaTime, ForceMode.Force);

            _isFly = true;
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

            if (_soldier is SoldierVisual)
            {
                if (_isFly)
                {
                    _soldier.SetCharacterAlpha(Mathf.Sin(_timer * Mathf.PI * 2.0f * 10.0f));
                }
                else
                {
                    if (_timer < _maxTime * 0.6f)
                    {
                        _soldier._animation.SetAnimationType(SoldierAnimation.AnimType.Wait);
                        _isFly = true;
                    }
                }
                _soldier._animation.Update();
            }
            else if (_soldier is Soldier)
            {
                if (_isFly)
                {
                    // ÇPÉtÉåÅ[ÉÄÇ≈âÒì]Ç∑ÇÈäpìxÇäpë¨ìxÇ∆åoâﬂéûä‘Ç©ÇÁåvéZ
                    float angle = _angleSpeed * deltaTime;
                    Vector3 rot = new Vector3(_angleSpeed, 0.0f, 0.0f) * Time.deltaTime;
                    _soldier._characterAnchor.transform.Rotate(rot, Space.Self);
                }
                else
                {
                    _soldier._animation.Update();
                    transform.position = _hitPosition;
                    if (_timer < _maxTime * 0.6f)
                    {
                        AddForce();
                    }
                }
            }


            if (_timer < 0.0f)
            {
                if (_soldier is SoldierVisual)
                {

                }
                else if (_soldier is Soldier)
                {
                    _soldier._characterAnchor.transform.localRotation = Quaternion.identity;
                    _soldier.GetCharacterObject().transform.localEulerAngles = Vector3.zero;
                    _soldier.SetCharacterRotation(Quaternion.identity);

                    // rigitbodyèâä˙âª
                    _rb.useGravity = false;
                    _rb.velocity = Vector3.zero;

                    // ï`âÊÉtÉâÉOOFF
                    _soldier._isExist = false;
                    _soldier.SetVisibleSprite(SoldierMesh.Character, false);
                    _soldier.SetVisibleSprite(SoldierMesh.Field, false);
                    _soldier.Change(null);

                    RemainSoldier();
                }
            }
        }
    }

}