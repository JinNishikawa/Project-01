using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIGame
{
    public class WhiteOut : MonoBehaviour
    {
        private Image _image;

        private bool _isAppear;
        private float _maxTime = 0.25f;
        private float _timer;

        private void Awake()
        {
            _image = GetComponent<Image>();
            Color color = _image.color;
            color.a = 0.0f;
            _image.color = color;

            _isAppear = true;
            _timer = 0.0f;


        }

        // Start is called before the first frame update
        void Start()
        {
            ReadyPanel panel = GetComponentInParent<ReadyPanel>();
            LineAppearAlpha appear = panel._line.AddComponent<LineAppearAlpha>();
            appear.StartAlpha(false, ReadyPanel.Type.MaxType, _maxTime);
        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            Color color = _image.color;
            float alpha = 0.0f;

            if (_isAppear)
            {
                alpha = Mathf.Clamp01(_timer / _maxTime);
                color.a = alpha;
                _image.color = color;

                if (_timer > _maxTime)
                {
                    _timer = 0.0f;
                    _isAppear = false;
                }
                return;
            }

            alpha = 1.0f - Mathf.Clamp01(_timer / _maxTime);
            color.a = alpha;
            _image.color = color;

            if (_timer > _maxTime)
            {
                ReadyPanel panel = GetComponentInParent<ReadyPanel>();
                panel.NextType(ReadyPanel.Type.LineAlpha);

                gameObject.SetActive(false);

                Destroy(this);
            }

        }
    }
}