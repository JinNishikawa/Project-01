using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class State : MonoBehaviour
    {
        /** ゲームマネージャー */
        protected GameMgr _gameManager;

        /** タイマー */
        protected float _timer;


        public virtual void Awake()
        {
            _gameManager = GameMgr.Instance;
        }

        // Start is called before the first frame update
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {
            _timer -= Time.deltaTime;
        }

        public virtual void Fin()
        {

        }

        public void SetTimer(float time)
        {
            _timer = time;
        }

        public virtual void Next()
        {

        }
    }
}