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
        /** �ړ����t���O */
        private bool _isMove;
        /** �ړI�n */
        private Vector3 _goal;
        /** �J�n�ʒu */
        private Vector3 _start;
        /** �J�n�p�x */
        private float _startAngle;
        /** �ړI�p�x */
        private float _goalAngle;
        /** �ړ����� */
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

            // �o�ߎ��ԉ��Z
            _timer += deltaTime;

            // �����v�Z
            float rate = Mathf.Clamp01(_timer / _moveTime);
            rate = Rating(rate);



            // �ړ�
            transform.position = Vector3.Lerp(_start, _goal, rate);

            // ��]
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
