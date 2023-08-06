using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public enum MoveType
    {
        SineIn,
        SineOut,
        SineInOut,
        QuadIn,
        QuadOut,
        QuadInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuardIn,
        QuardOut,
        QuardInOut,
        QuintIn,
        QuintOut,
        QuintInOut,

    }


    public class Move : State
    {
        /** 移動中フラグ */
        private bool _isMove;
        /** 目的地 */
        private Vector3 _goal;
        /** 開始位置 */
        private Vector3 _start;
        /** 開始角度 */
        private float _startAngle;
        /** 目的角度 */
        private float _goalAngle;
        /** 移動時間 */
        private float _moveTime;

        private MoveType _type;

        public Move()
        {
            _isMove = false;
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();

            float gameSpeed = 1.0f;
            if(Manager.GameMgr.Instance)
            {
                gameSpeed = Manager.GameMgr.Instance._gameSpeed;
            }

            float time = Time.deltaTime * gameSpeed;
            UpdateMove(time);
        }

        public override void Fin()
        {
            base.Fin();
        }

        public void UpdateMove(float deltaTime)
        {
            if (!_isMove) return;

            // 経過時間加算
            _timer += deltaTime;

            // 割合計算
            float rate = Mathf.Clamp01(_timer / _moveTime);
            rate = Rating(rate);



            // 移動
            transform.position = Vector3.Lerp(_start, _goal, rate);

            // 回転
            //float angle = Mathf.LerpAngle(_startAngle, _goalAngle, rate);
            //transform.eulerAngles = new Vector3(angle, 0, 0);

            if (rate >= 1.0f)
            {
                _card.SetNextState();
            }
        }

        private float Rating(float rate)
        {
            switch (_type)
            {
                case MoveType.SineIn:
                    break;
                case MoveType.SineOut:
                    return Easing.SineInOut(rate, 1.0f, 0.0f, 1.0f);
                case MoveType.SineInOut:
                    break;
                case MoveType.QuadIn:
                    return Easing.QuadIn(rate, 1.0f, 0.0f, 1.0f);
                case MoveType.QuadOut:
                    break;
                case MoveType.QuadInOut:
                    break;
                case MoveType.CubicIn:
                    break;
                case MoveType.CubicOut:
                    break;
                case MoveType.CubicInOut:
                    break;
                case MoveType.QuardIn:
                    break;
                case MoveType.QuardOut:
                    break;
                case MoveType.QuardInOut:
                    break;
                case MoveType.QuintIn:
                    break;
                case MoveType.QuintOut:
                    break;
                case MoveType.QuintInOut:
                    break;
            }

            return Easing.SineInOut(rate, 1.0f, 0.0f, 1.0f);
        }


        public void StartMove(Vector3 dest, float time)
        {
            _timer = 0.0f;
            _moveTime = time;
            _start = transform.position;
            _goal = dest;

            _type = MoveType.SineInOut;
            //_startAngle = transform.eulerAngles.x;
            //Debug.Log(_startAngle);
            //_goalAngle = 90.0f;

            _isMove = true;
        }

        public void StartMove(Vector3 dest, float time, MoveType type)
        {
            _timer = 0.0f;
            _moveTime = time;
            _start = transform.position;
            _goal = dest;

            //_startAngle = transform.eulerAngles.x;
            //Debug.Log(_startAngle);
            //_goalAngle = 90.0f;

            _type = type;

            _isMove = true;
        }

    }
}
