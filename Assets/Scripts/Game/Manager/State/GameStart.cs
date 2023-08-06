using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameStart : State
    {
        enum PerformType
        {
            None,
            Card,
            Prepare,
            Stage,
            Max
        }

        private PerformType _current;
        private PerformType _next;
        private float _nextCardTime;


        public override void Awake()
        {
            base.Awake();
            _timer = 3.0f;
            _current = PerformType.None;
            _next = PerformType.Card;
        }

        public override void Fin()
        {
            base.Fin();
        }

        public override void Start()
        {
            base.Start();
            NextPerform();
        }

        public override void Update()
        {
            base.Update();

            if(_current == PerformType.Card)
            {
                if (_timer < _nextCardTime) {
                    for (int i = 0; i < (int)UserType.MaxPlayer; i++)
                    {
                        UserManager.Instance._characterInfo[i]._deck.SetHand();
                    }
                    _nextCardTime -= (1.0f / 5.0f);
                }
            }

            if(_timer < 0.0f)
            {
                NextPerform();
            }
        }

        public override void Next()
        {
            _gameManager.Change(GameState.Game);
        }

        private void NextPerform()
        {
            _current = _next;
            switch (_current)
            {
                case PerformType.Card:
                    // カード配布
                    for (int i = 0; i < (int)UserType.MaxPlayer; i++)
                    {
                        UserManager.Instance._characterInfo[i]._deck.PrepareDeck();
                        UserManager.Instance._characterInfo[i]._lifeMeter.InitFlag();
                    }
                    _next = PerformType.Prepare;
                    _timer = 3.0f;
                    _nextCardTime = _timer - (1.0f / 5.0f);
                    break;
                case PerformType.Prepare:
                    for (int i = 0; i < (int)UserType.MaxPlayer; i++)
                    {
                        UserManager.Instance._characterInfo[i]._prepare.SetCircleEffect(true);
                    }
                    _next = PerformType.Stage;
                    _timer = 1.0f;
                    break;
                case PerformType.Stage:
                    Stage.StagePerform perform = GameObject.FindWithTag("StageObject").GetComponent<Stage.StagePerform>();
                    perform.StartPerform(this);
                    _next = PerformType.None;
                    _timer = 1.0f;
                    break;
                case PerformType.Max:
                    break;
            }

        }
    }
}