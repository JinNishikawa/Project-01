using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
namespace System
{
    [CreateAssetMenu(menuName = "System/Command", fileName = "Command")]
    public class HideCommand : ScriptableObject
    {
        /** 終了かどうか */
        private bool _isSuccess;

        /** 設定コマンド */
        [Header("コマンド")]
        public KeyCode[] _Command;

        /** 最大時間 */
        [Header("タイムリミット")]
        public float _LimitTime;

        /** 現在のコマンド番号 */
        private int _currentCnt;

        /** タイマー */
        private float _timer;

        //=== デバッグ用
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private TextMeshProUGUI _text;

        // Start is called before the first frame update
        public void Start()
        {
            _isSuccess = false;
            _currentCnt = 0;
            _timer = 0.0f;

            InitDebug();
        }

        // Update is called once per frame
        public void Update()
        {
            UpdateTimer();

            KeyChecker();

            UpdateDebug();
        }

        private void UpdateTimer()
        {
            if (_isSuccess) return;

            _timer -= Time.deltaTime;

            if(_timer < 0.0f)
            {
                _currentCnt = 0;
                _timer = 0.0f;
            }
        }

        private KeyCode GetKey()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        return code;
                    }
                }
            }

            return KeyCode.None;
        }

        private void KeyChecker()
        {
            if (_isSuccess) return;

            // キー入力
            KeyCode code = GetKey();

            // 未入力
            if (code == KeyCode.None) return;

            // 誤入力
            if (code != _Command[_currentCnt]) return;

             _currentCnt++;
            _timer = _LimitTime;

            // 終了
            if (_currentCnt == _Command.Length)
            {
                _isSuccess = true;
                return;
            }
        }

        public bool GetSuccess()
        {
            return _isSuccess;
        }

        private void InitDebug()
        {
            if (_slider)
            {
                _slider.maxValue = _Command.Length;
                _slider.value = _currentCnt;
            }

            if (_text)
            {
                _text.text = _currentCnt.ToString();
            }
        }

        private void UpdateDebug()
        {
            if (_slider)
            {
                _slider.value = _currentCnt;
            }

            if (_text)
            {
                _text.text = _currentCnt.ToString();
            }
        }
    }
}