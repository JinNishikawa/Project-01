using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
namespace UIGame
{
    public class TextMove : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private bool _isAppear = false;
        private bool _isFin = false;
        private float _maxAlphaTime = 0.3f;
        private float _maxMoveTime = 0.5f;
        private float _waitTime = 1.0f;

        private float _timer;

        private float _textMax = 1450.0f;
        private float _textMin = 200.0f;

        private RawImage _image;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _timer = 0.0f;

            ReadyPanel panel = GetComponentInParent<ReadyPanel>();
            _image = panel._thunder.GetComponentInChildren<RawImage>();

            Color color = _image.color;
            color.a = 0.0f;
            _image.color = color;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            float rate;

            if(_isFin)
            {
                rate = Mathf.Clamp01(_timer / _waitTime);
                if (_timer > _waitTime)
                {
                    ReadyPanel panel = GetComponentInParent<ReadyPanel>();
                    panel.NextType(ReadyPanel.Type.FinAlpha);
                }
                return;
            }

            if(_isAppear)
            {
                //rate = Mathf.Clamp01(_timer / _maxAlphaTime);
                //Color color = _text.color;
                //color.a = rate;
                //_text.color = color;

                //color = _image.color;
                //color.a = rate;
                //_image.color = color;

                if (_timer > _maxAlphaTime)
                {
                    _isAppear = false;
                    _timer = 0.0f;
                }

                return;
            }


            rate = Mathf.Clamp01(_timer / _maxMoveTime);
            float textRate = Easing.CubicIn(rate, 1.0f, 0.0f, 1.0f);

            _text.fontSize = _textMax - (textRate * (_textMax - _textMin));

            float colorRate = Easing.SineInOut(rate, 1.0f, 0.0f, 1.0f);
            Color color = _text.color;
            color.a = colorRate;
            _text.color = color;

            color = _image.color;
            color.a = colorRate;
            _image.color = color;

            if (_timer > _maxMoveTime)
            {
                _isFin = true;
                _timer = 0.0f;
            }
        }
    }
}