using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class End : State
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Fin()
        {
            base.Fin();
        }

        public override void Start()
        {
            base.Start();

            // ƒ}ƒEƒX‰Šú‰»
            MouseManager.Instance.InitFlag();
        }

        public override void Update()
        {
            base.Update();

            if (_timer < 0.0f)
            {
                _gameManager.Change(GameState.Result);
            }
        }
    }
}