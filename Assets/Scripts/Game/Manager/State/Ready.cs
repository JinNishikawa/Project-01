using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Ready : State
    {
        private GameObject _readyPanel;

        public override void Awake()
        {
            base.Awake();

            GameObject canvas = GameObject.Find("Canvas");
            GameObject sourceObject = Resources.Load<GameObject>("Prototype/UI/ReadyPanel/ReadyPanel");
            _readyPanel = Instantiate(sourceObject, canvas.transform);
        }

        public override void Fin()
        {
            base.Fin();

            Destroy(_readyPanel);
        }

        public override void Start()
        {
            base.Start();
            _timer = 20.0f;
        }

        public override void Update()
        {
            base.Update();

            if (_timer < 0.0f)
            {
                _gameManager.Change(GameState.Start);
            }
        }
    }
}