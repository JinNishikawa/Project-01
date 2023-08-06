using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScale : MonoBehaviour
{
    private bool _isExist;

    private bool _isAttack;
    private float _timer;

    private Rigidbody _rb;

    private bool _isLast;

    [SerializeField]
    private float _AttackTime;

    private void Awake()
    {
        _isExist = true;
        _isAttack = false;
        _isLast = false;
        _rb = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isAttack)
        {
            _timer -= Time.deltaTime;
            if(_timer < 0.0f)
            {
                GameObject explisionObject = Manager.GameMgr.Instance._GameSetting._EffectData?._HPExplosion;
                Manager.EffectManager.Instance.StartEffect(explisionObject, transform.position, 0.25f);
                Destroy(gameObject);
            }
        }
    }

    public bool GetExist()
    {
        return _isExist;
    }

    public void SetFlag()
    {
        _isExist = false;
    }

    public void SetLastFlag()
    {
        _isLast = true;
    }

    public bool GetLastFlag()
    {
        return _isLast;
    }

    public void OnAttack()
    {
        _isAttack = true;
        //_timer = _AttackTime;

        //float speed = 20.0f;
        //Vector3 dir = (Camera.main.transform.position - transform.position).normalized;

        //_rb.AddForce(_rb.mass * dir * speed / Time.deltaTime, ForceMode.Force);
    }
}

