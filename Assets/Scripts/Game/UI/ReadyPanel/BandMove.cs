using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGame
{
    public class BandMove : MonoBehaviour
    {
        private Vector3 _startPos;
        private Vector3 _goalPos;
        private bool _isMove = false;
        private float _timer;

        private float _maxTime = 0.8f;

        private RectTransform _rt;

        // Start is called before the first frame update
        void Start()
        {
            _rt = GetComponent<RectTransform>();

            _goalPos = _rt.anchoredPosition;
            _startPos = _goalPos;
            _startPos.x *= 3;
            _rt.anchoredPosition = _startPos;

        }

        // Update is called once per frame
        void Update()
        {
            OnMove();
        }

        public void OnMove()
        {
            if (!_isMove) return;

            // Œo‰ßŽžŠÔ‰ÁŽZ
            _timer += Time.deltaTime;

            // Š„‡ŒvŽZ
            float rate = Mathf.Clamp01(_timer / _maxTime);

            rate = Easing.QuartInOut(rate, 1.0f, 0.0f, 1.0f);

            // ˆÚ“®
            _rt.anchoredPosition = Vector3.Lerp(_startPos, _goalPos, rate);

            if(rate >= 1.0f)
            {
                _isMove = false;

                ReadyPanel panel = GetComponentInParent<ReadyPanel>();
                panel.NextType(ReadyPanel.Type.Thunder);

                Destroy(this);
            }
        }

        public void MoveStart()
        {
            if (_isMove) return;

            _isMove = true;
            _timer = 0.0f;
        }
    }
}