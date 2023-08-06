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
        /** �I�����ǂ��� */
        private bool _isSuccess;

        /** �ݒ�R�}���h */
        [Header("�R�}���h")]
        public KeyCode[] _Command;

        /** �ő厞�� */
        [Header("�^�C�����~�b�g")]
        public float _LimitTime;

        /** ���݂̃R�}���h�ԍ� */
        private int _currentCnt;

        /** �^�C�}�[ */
        private float _timer;

        //=== �f�o�b�O�p
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

            // �L�[����
            KeyCode code = GetKey();

            // ������
            if (code == KeyCode.None) return;

            // �����
            if (code != _Command[_currentCnt]) return;

             _currentCnt++;
            _timer = _LimitTime;

            // �I��
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