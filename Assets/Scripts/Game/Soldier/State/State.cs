using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class State : MonoBehaviour
    {
        protected Soldier _soldier;

        protected float _timer;

        protected float _maxTime;

        public virtual void Awake()
        {
            _soldier = GetComponent<Soldier>();
        }

        // Start is called before the first frame update
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        public virtual void Fin()
        {

        }

        public virtual void SetTimer(float time)
        {
            _timer = time;
            _maxTime = _timer;
        }
    }
}