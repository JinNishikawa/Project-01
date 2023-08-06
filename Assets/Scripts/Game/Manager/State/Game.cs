using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Game : State
    {
        Field.LifeMeter[] _lifeMeters = new Field.LifeMeter[(int)UserType.MaxPlayer];

        public override void Awake()
        {
            base.Awake();

            for (int i = 0; i < (int)UserType.MaxPlayer; i++)
            {
                _lifeMeters[i] = UserManager.Instance._characterInfo[i]._lifeMeter;
            }

            // “GAIÝ’è
            UserManager.Instance.SetEnemyAI();

            Cursor.visible = true;
        }

        public override void Fin()
        {
            base.Fin();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();


            if(GetFinFlag())
            {
                _gameManager.Change(GameState.End);
            }

        }

        private bool GetFinFlag()
        {
            foreach(Field.LifeMeter meter in _lifeMeters)
            {
                int life = meter.GetLife();
                if(life <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}