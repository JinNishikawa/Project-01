using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public enum GameState
    {
        None,
        Select,
        Ready,
        Start,
        Game,
        End,
        Result,

        MaxState
    }

    public class GameMgr : SingletonMonoBehaviour<GameMgr>
    {
        /** ゲームステート */
        [HideInInspector]
        public GameState _currentState;

        /** ステートクラス */
        private State _state;

        /** ゲーム設定 */
        public Data.GameSetting _GameSetting;

        [SerializeField]
        private AudioClip _clip;

        [HideInInspector]
        public float _gameSpeed;

        [HideInInspector]
        public float _systemSpeed;

        private void Awake()
        {
            _gameSpeed = _GameSetting._SystemSetting._GameSpeed;
            _systemSpeed = Time.timeScale;
        }

        // Start is called before the first frame update
        void Start()
        {
            Change(GameState.Select);

            SoundManager.Instance?.PlayAudio(SoundManager.SoundType.BGM, _clip);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Change(GameState nextState)
        {
            _state?.Fin();
            Destroy(_state);

            _currentState = nextState;

            State state = null;
            switch (_currentState)
            {
                case GameState.Select:
                    Select select = gameObject.AddComponent<Select>();
                    state = select;
                    break;
                case GameState.Ready:
                    Ready ready = gameObject.AddComponent<Ready>();
                    state = ready;
                    break;
                case GameState.Start:
                    GameStart start = gameObject.AddComponent<GameStart>();
                    state = start;
                    break;
                case GameState.Game:
                    Game game = gameObject.AddComponent<Game>();
                    state = game;
                    break;
                case GameState.End:
                    End end = gameObject.AddComponent<End>();
                    state = end;
                    break;
                case GameState.Result:
                    Result result = gameObject.AddComponent<Result>();
                    state = result;
                    break; 
            }

            _state = state;
            _state?.SetTimer(1.0f);
        }

        public void SetSystemSpeed(float rate)
        {
            if (_systemSpeed == rate) return;

            _systemSpeed = rate;
            Time.timeScale = _systemSpeed;
        }

    }
}