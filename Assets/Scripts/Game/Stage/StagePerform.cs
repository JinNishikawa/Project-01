using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public class StagePerform : MonoBehaviour
    {
        enum PerformType
        {
            None,
            Stage,
            Other,
            Effect,
            Max
        }

        private PerformType _currentType;

        /** ステージ上のオブジェクト */
        private List<GameObject> _stageObjectList;
        private int _groundParentCnt;
        private int _maxBlockWidth;
        private int[] _currentWave;

        /** その他演出オブジェクト */
        private List<GameObject>[] _otherObjectsList;

        private List<ParticleSystem> _effectList;

        private bool _isVisible;
        private int _performIndex;

        private float _timer = 0.0f;
        private float _waitTime = 0.1f;

        Manager.State _state;

        private void Awake()
        {
            _stageObjectList = new List<GameObject>();
            _stageObjectList.Clear();
            _effectList = new List<ParticleSystem>();
            _effectList.Clear();
            _currentType = PerformType.None;
        }

        // Start is called before the first frame update
        void Start()
        {
            // ステージオブジェクト取得
            Transform stage = transform.Find("Stage");
            _groundParentCnt = stage.childCount;
            _currentWave = new int[_groundParentCnt];
            for (int i=0;i<_groundParentCnt;i++)
            {
                Transform parent = stage.GetChild(i);
                _maxBlockWidth = parent.childCount;
                for (int j =0;j< _maxBlockWidth; j++) {
                    _stageObjectList.Add(parent.GetChild(j).gameObject);
                }

                _currentWave[i] = -1;
            }

            _currentWave[0] = 0;

            // その他オブジェクト取得
            Transform other = transform.Find("Other");
            _otherObjectsList = new List<GameObject>[other.GetChild(0).childCount];
            for(int i=0;i<other.GetChild(0).childCount;i++)
            {
                _otherObjectsList[i] = new List<GameObject>();
                _otherObjectsList[i].Clear();
            }
            // L:R
            for(int posCnt=0;posCnt<other.childCount;posCnt++)
            {
                Transform parent = other.GetChild(posCnt);
                // order
                for(int performCnt = 0;performCnt < parent.childCount; performCnt++)
                {
                    Transform perform = parent.GetChild(performCnt);
                    for(int objCnt = 0;objCnt<perform.childCount;objCnt++)
                    {
                        GameObject obj = perform.GetChild(objCnt).gameObject;
                        _otherObjectsList[performCnt].Add(obj);

                        ParticleSystem[] particle = obj.GetComponentsInChildren<ParticleSystem>();
                        foreach(ParticleSystem p in particle)
                        {
                            _effectList.Add(p);
                        }
                    }
                }
            }

            AllInit();
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePerform();

        }

        public void AllInit()
        {
            foreach(GameObject obj in _stageObjectList)
            {
                SetScale scale = obj.AddComponent<SetScale>();
                scale.SetScaleY(0.0f);
                obj.SetActive(false);
            }

            foreach (ParticleSystem particle in _effectList)
            {
                particle.Stop();
            }

            for (int i=0;i<_otherObjectsList.Length;i++)
            {
                foreach(GameObject obj in _otherObjectsList[i])
                {
                    SetScale scale = obj.AddComponent<SetScale>();
                    scale.SetScaleY(0.0f);
                    obj.SetActive(false);
                }
            }
        }

        private void UpdatePerform()
        {
            switch (_currentType)
            {
                case PerformType.None:
                    break;
                case PerformType.Stage:
                    Wave();
                    break;
                case PerformType.Other:
                    UpdateOther();
                    break;
                case PerformType.Effect:
                    UpdateEffect();
                    break;
                case PerformType.Max:
                    _state.Next();
                    Destroy(this);
                    break;
            }
        }

        private void Wave()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0.0f) return;

            bool isFin = true;
            for (int i = 0; i < _groundParentCnt; i++)
            {
                if (_currentWave[i] < _maxBlockWidth)
                {
                    isFin = false;
                    break;
                }
            }

            if(isFin)
            {
                _isVisible = false;
                _currentType = PerformType.Other;
                _performIndex = 0;
                return;
            }

            // 出現
            for (int i=0;i<_groundParentCnt;i++)
            {
                if (_currentWave[i] == -1 || _currentWave[i] >= _maxBlockWidth) continue;


                // 出現開始
                AppearBlock appear = _stageObjectList[i * _maxBlockWidth + _currentWave[i]].AddComponent<AppearBlock>();
                appear.StartAppear(0.3f, 2.0f,AppearBlock.EasingType.QuadIn ,AppearBlock.NextType.Normal);
            }

            // 変更(同列
            for (int i = 0; i < _groundParentCnt; i++)
            {
                if (_currentWave[i] == -1) continue;

                // 次のブロック
                _currentWave[i]++;
                if (_currentWave[i] >= _maxBlockWidth)
                {
                    _currentWave[i] = _maxBlockWidth;
                }
            }

            // 一つ上
            for (int i = 0; i < _groundParentCnt; i++)
            {
                if (_currentWave[i] != -1) continue;

                _currentWave[i]++;
                if (_currentWave[i] >= _maxBlockWidth)
                {
                    _currentWave[i] = _maxBlockWidth;
                }
                break;
            }


            _timer = _waitTime;
        }

        void UpdateOther()
        {
            if (_performIndex >= _otherObjectsList.Length)
            {
                _currentType = PerformType.Effect;
                return;
            }

            if (!_isVisible)
            {
                foreach (GameObject obj in _otherObjectsList[_performIndex])
                {
                    AppearBlock appear = obj.AddComponent<AppearBlock>();
                    appear.StartAppear(0.3f, 1.0f, AppearBlock.EasingType.QuartIn);
                }
                _timer = 0.5f;
                _isVisible = true;
                foreach(ParticleSystem particle in _effectList)
                {
                    particle.Stop();
                }
                return;
            }

            _timer -= Time.deltaTime;
            if (_timer < 0.0f)
            {
                _performIndex++;
                _isVisible = false;
            }
        }

        void UpdateEffect()
        {
            foreach (ParticleSystem particle in _effectList)
            {
                particle.Play();
            }
            _currentType = PerformType.Max;
        }

 

        public void StartPerform(Manager.State state)
        {
            _currentType = PerformType.Stage;
            _state = state;
        }
    }
}