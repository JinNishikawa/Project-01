using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select
{
    public class PaperManager : SingletonMonoBehaviour<PaperManager>
    {
        private Data.CharacterStatus[] _enemyStatus;

        private GameObject _sourcePaper;

        private GameObject[] _wantedList = new GameObject[5];
        private Vector3[] _paperPos = new Vector3[5];

        private bool _isMove;

        private int _center = 2;
        private int _currentTarget;

        private Animator _humanAnimator;
        private float _timer;
        private float _idleTime = 1.0f;

        private bool _isDecide;
        private float _sceneWaitTime = 2.0f;

        // Start is called before the first frame update
        void Start()
        {
            _enemyStatus = SelectEnemyManager.Instance._enemySetting._EnemyStatusList;
            _sourcePaper = Resources.Load<GameObject>("Prototype/Select/Paper");
            _currentTarget = 0;

            // à íuê›íË
            for (int i=0;i<5;i++)
            {
                Vector3 pos = Vector3.zero;
                pos.x = 4.0f;
                pos.z = 15.0f * (i - _center);
                _paperPos[i] = pos;
            }

            _paperPos[_center].x = 0.0f;

            for(int i=1;i<4;i++)
            {
                _wantedList[i] = Create(2 - i);
                _wantedList[i].transform.localPosition = _paperPos[i];
            }

            _wantedList[_center].GetComponent<Wanted>().StartText();

            _isMove = false;

            _humanAnimator = GameObject.Find("Human").GetComponent<Animator>();

            _isDecide = false;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTimer();
        }

        GameObject Create(int index)
        {
            if(index < 0)
            {
                index += _enemyStatus.Length;
            }

            if(index >= _enemyStatus.Length)
            {
                index -= _enemyStatus.Length;
            }

            GameObject obj = Instantiate(_sourcePaper, transform);
            obj.transform.localPosition = Vector3.zero;
            Wanted wanted = obj.GetComponent<Wanted>();
            wanted.SetStatus(_enemyStatus[index]);
            return obj;
        }

        public void Next()
        {
            if (_isMove) return;
            if (_isDecide) return;

            // í«â¡
            _wantedList[0] = Create(_currentTarget + 2);
            _wantedList[0].transform.localPosition = _paperPos[0];

            for (int i = 4; i > 0; i--)
            {
                _wantedList[i] = _wantedList[i - 1];

                if (_wantedList[i] != null)
                {
                    if (i == 4)
                    {
                        _wantedList[i].GetComponent<Wanted>().StartMove(true, _paperPos[i]);
                    }
                    else
                    {
                        _wantedList[i].GetComponent<Wanted>().StartMove(false, _paperPos[i]);
                    }
                }
            }

            _isMove = true;
            _currentTarget++;
            if (_currentTarget >= _enemyStatus.Length)
            {
                _currentTarget = 0;
            }

            _humanAnimator.SetTrigger("Next");
        }

        public void Back()
        {
            if (_isMove) return;
            if (_isDecide) return;

            // í«â¡
            _wantedList[4] = Create(_currentTarget - 2);
            _wantedList[4].transform.localPosition = _paperPos[4];

            for (int i = 0; i < _wantedList.Length - 1; i++)
            {
                _wantedList[i] = _wantedList[i + 1];

                if (_wantedList[i] != null)
                {
                    if (i == 0)
                    {
                        _wantedList[i].GetComponent<Wanted>().StartMove(true, _paperPos[i]);
                    }
                    else
                    {
                        _wantedList[i].GetComponent<Wanted>().StartMove(false, _paperPos[i]);
                    }
                }
            }

            _isMove = true;
            _currentTarget--;
            if(_currentTarget < 0)
            {
                _currentTarget = _enemyStatus.Length - 1;
            }

            _humanAnimator.SetTrigger("Back");
        }

        public void DestroyPaper(GameObject obj)
        {
            for (int i=0;i<_wantedList.Length;i++)
            {
                if(_wantedList[i] == obj)
                {
                    _wantedList[i] = null;
                    Destroy(obj);
                    break;
                }
            }

            _wantedList[_center].GetComponent<Wanted>().StartText();
            _timer = _idleTime;
        }

        private void UpdateTimer()
        {
            if (_timer <= 0.0f) return;

            _timer -= Time.deltaTime;

            if(_timer < 0.0f)
            {
                if(_isDecide)
                {
                    GotoNext();
                }
                else
                {
                    _isMove = false;
                    _humanAnimator.SetTrigger("Idle");
                }
            }
        }

        public void DecideEnemy()
        {
            if (_isDecide) return;

            _isDecide = true;
            _timer = _sceneWaitTime;
            _humanAnimator.SetTrigger("Decide");
        }

        private void GotoNext()
        {
            bool isSuccess = FadeManager.StartFade("Game", Color.white, 1.0f);
            if (isSuccess)
            {
                GameObject sourceObj = Resources.Load<GameObject>("Game/SceneData/DataTransporter");
                GameObject transportObj = Instantiate(sourceObj, Vector3.zero, Quaternion.identity);
                DataTransporter data = transportObj.GetComponent<DataTransporter>();
                data._enemyStatus = _enemyStatus[_currentTarget];
            }
        }
    }
}