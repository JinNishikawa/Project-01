using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIGame
{
    public class LineAppearAlpha : MonoBehaviour
    {

        private Image[] _image;

        private bool _isAppear;
        private float _maxTime = 0.2f;
        private float _timer;

        private bool _isStart;

        private ReadyPanel.Type next;

        private void Awake()
        {
            _image = new Image[transform.childCount];
            for (int i = 0; i < _image.Length; i++)
            {
                _image[i] = transform.GetChild(i).GetComponent<Image>();
            }
            _isStart = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!_isStart) return;

            _timer += Time.deltaTime;
            float alpha = 0.0f;

            if (_isAppear)
            {
                alpha = Mathf.Clamp01(_timer / _maxTime);

                for (int i=0;i<_image.Length;i++)
                {
                    Color color = _image[i].color;
                    color.a = alpha;
                    _image[i].color = color;
                }

                if (_timer > _maxTime)
                {
                    if (next != ReadyPanel.Type.MaxType)
                    {
                        ReadyPanel panel = GetComponentInParent<ReadyPanel>();
                        panel.NextType(next);
                    }

                    Destroy(this);
                }
                return;
            }

            alpha = 1.0f - Mathf.Clamp01(_timer / _maxTime);
            for (int i = 0; i < _image.Length; i++)
            {
                Color color = _image[i].color;
                color.a = alpha;
                _image[i].color = color;
            }

            if (_timer > _maxTime)
            {
                if(next != ReadyPanel.Type.MaxType)
                {
                    ReadyPanel panel = GetComponentInParent<ReadyPanel>();
                    panel.NextType(next);
                }

                Destroy(this);
            }
        }

        public void StartAlpha(bool isAppear, ReadyPanel.Type type = ReadyPanel.Type.MaxType, float maxTime = 1.0f)
        {
            _isStart = true;
            _isAppear = isAppear;

            _maxTime = maxTime;
            _timer = 0.0f;

            next = type;
        }
    }
}