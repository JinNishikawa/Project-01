using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Result : State
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Fin()
        {
            base.Fin();

            UserManager.Instance.InitStatus();

            DeleteAllCard();
        }

        public override void Start()
        {
            base.Start();

            Cursor.visible = true;
        }

        public override void Update()
        {
            base.Update();

            if (_timer < 0.0f)
            {
                FadeManager.StartFade("SelectEnemy", Color.white, 1.0f);
                _gameManager.Change(GameState.None);
            }
        }

        public void DeleteAllCard()
        {
            GameObject[] cardObj = GameObject.FindGameObjectsWithTag("Card");
            foreach(GameObject obj in cardObj)
            {
                Destroy(obj);
            }
        }
    }
}