using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class State : MonoBehaviour
    {
        protected Card _card;

        protected Info _info;

        protected float _timer;

        protected Field.CharacterInfo _characterInfo;

        public virtual void Awake()
        {
            _card = GetComponent<Card>();
            _info = GetComponent<Info>();
            if (Manager.UserManager.Instance)
            {
                _characterInfo = Manager.UserManager.Instance._characterInfo[(int)_info._userType];
            }
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

        public void SetTimer(float time)
        {
            _timer = time;
        }
    }
}
