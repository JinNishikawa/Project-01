using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class DebugManager : SingletonMonoBehaviour<DebugManager>
    {
        private bool _isDebug;

        [SerializeField]
        private System.HideCommand _command;

        [SerializeField]
        private GameObject _debugPanel;

        // Start is called before the first frame update
        void Start()
        {
            _command.Start();

            SetDebugFlag(false);
        }

        // Update is called once per frame
        void Update()
        {
            CommandUpdate();
        }

        public void SetDebugFlag(bool isFlag)
        {
            _isDebug = isFlag;

            if(_debugPanel)
            {
                _debugPanel.SetActive(_isDebug);
            }

            for (int i=0;i<2;i++) {
                float a = 0.0f;
                if(_isDebug)
                {
                    a = 255.0f;
                }
                Field.Battle.Instance.SetSpriteAlpha(Field.Battle.TileType.Battle, a);
            }
        }

        public void SwitchFlag()
        {
            SetDebugFlag(!_isDebug);
        }

        public bool GetFlag()
        {
            return _isDebug;
        }

        private void CommandUpdate()
        {
            if (_command == null) return;

            _command.Update();


            if (_command.GetSuccess())
            {
                SwitchFlag();
                _command.Start();
            }

        }
    }
}