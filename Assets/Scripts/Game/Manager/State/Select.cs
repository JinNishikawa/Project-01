using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Select : State
    {
        //private GameObject _selectPanel;

        public override void Awake()
        {
            base.Awake();

            //GameObject canvas = GameObject.Find("Canvas");
            //GameObject sourceObject = Resources.Load<GameObject>("Prototype/UI/EnemySelect/SelectPanel");
            //_selectPanel = Instantiate(sourceObject, canvas.transform);

            Cursor.visible = true;

            _timer = 2.0f;
        }

        public override void Fin()
        {
            base.Fin();
        }

        public override void Start()
        {
            base.Start();

            GameObject data = GameObject.FindGameObjectWithTag("DataTransport");
            if(data == null)
            {
                Data.CharacterStatus enemyStatus = UserManager.Instance._CharacterStatus[(int)UserType.Player2];
                UserManager.Instance.SetCharacterStatus(UserType.Player2, enemyStatus);
            }
        }

        public override void Update()
        {
            base.Update();

            if(FadeManager.GetCurrentState() == FadeManager.FADE_STAT.FADE_NONE)
            {               
                UserManager.Instance.SetNextState();
            }

        }

    }
}