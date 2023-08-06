using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class UIButtonBase : MonoBehaviour
    {
        protected bool _isMove;
        protected bool _isSelect;
        private Vector3 _startScale;
        private Vector3 _endScale;
        private float _timer;

        private float _maxRate = 1.2f;
        private float _maxTime = 0.1f;

        private void Awake()
        {
            _isMove = false;
            _isSelect = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Move();
            Select();
        }

        private void Move()
        {
            if (!_isMove) return;

            // Œo‰ßŽžŠÔ‰ÁŽZ
            _timer -= Time.deltaTime;

            // Š„‡ŒvŽZ
            float rate = Mathf.Clamp01((_maxTime - _timer) / _maxTime);

            transform.localScale = Vector3.Lerp(_startScale, _endScale, rate);

            if(rate >= 1.0f)
            {
                _isMove = false;
                if(_isSelect)
                {
                    _timer = 0.0f;
                }
            }
        }

        private void Select()
        {
            if (!_isSelect) return;
            if (_isMove) return;

            _timer += Time.deltaTime;
            float rate = Mathf.Sin(_timer * Mathf.PI * 3.0f) * 0.2f + 1.0f;
            transform.localScale = Vector3.one * rate;          
        }

        public virtual void MoveStart(bool isSelect)
        {
            _isMove = true;
            _startScale = transform.localScale;
            _isSelect = isSelect;

            if(isSelect)
            {
                _endScale = Vector3.one * _maxRate;
            }
            else
            {
                _endScale = Vector3.one;
            }

            _timer = _maxTime;
        }

        

        public void OnPointerEnter()
        {
            _isSelect = true;
        }

        public void OnPointerEnd()
        {
            MoveStart(false);
        }

        public virtual void OnClick()
        {

        }
    }
}