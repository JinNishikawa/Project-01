using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public class AppearBlock : MonoBehaviour
    {
        public enum NextType
        {
            None,
            Normal,
            DrawLine
        }

        public enum EasingType
        {
            SineInOut,
            QuadIn,
            QuadOut,
            QuartIn,
            QuartOut
        }


        private bool _isAppear = false;

        private float _timer;
        private float _maxTime;

        private Vector3 _startScale;
        private Vector3 _goalScale;

        private NextType _next;
        private EasingType _easingType;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            TimeUpdate();
        }

        void TimeUpdate()
        {
            if (!_isAppear) return;

            _timer -= Time.deltaTime;

            float rate = Mathf.Clamp01((_maxTime - _timer) / _maxTime);
            rate = Rate(rate);

            transform.localScale  = Vector3.Lerp(_startScale, _goalScale, rate);

            if (_timer < 0.0f)
            {
                if(_next == NextType.Normal)
                {
                    AppearBlock appear = gameObject.AddComponent<AppearBlock>();
                    appear.StartAppear(0.3f, 1.0f,EasingType.QuadIn ,NextType.DrawLine);
                }

                if(_next == NextType.DrawLine)
                {
                    DrawLine line = gameObject.AddComponent<DrawLine>();
                }

                Destroy(this);
            }
        }

        private float Rate(float rate)
        {
            switch (_easingType)
            {
                case EasingType.SineInOut:
                    return Easing.SineInOut(rate, 1.0f, 0.0f, 1.0f);
                case EasingType.QuadIn:
                    return Easing.QuadIn(rate, 1.0f, 0.0f, 1.0f);
                case EasingType.QuadOut:
                    return Easing.QuadOut(rate, 1.0f, 0.0f, 1.0f);
                case EasingType.QuartIn:
                    return Easing.QuartIn(rate, 1.0f, 0.0f, 1.0f);
                case EasingType.QuartOut:
                    return Easing.QuartOut(rate, 1.0f, 0.0f, 1.0f);
            }

            return 0.0f;
        }

        public void StartAppear(float time, float goalScaleY, EasingType easing, NextType next = NextType.None)
        {
            gameObject.SetActive(true);

            _isAppear = true;
            _maxTime = time;
            _timer = _maxTime;

            _startScale = transform.localScale;
            _goalScale = transform.localScale;
            _goalScale.y = goalScaleY;

            _next = next;
            _easingType = easing;
        }
    }
}