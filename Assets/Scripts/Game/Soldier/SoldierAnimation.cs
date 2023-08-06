using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    [CreateAssetMenu(menuName = "Soldier/Animation", fileName = "SoldierAnimation")]
    public class SoldierAnimation : ScriptableObject
    {
        public enum SpriteDir
        {
            Front,
            Left,
            Right,
            Back,
            MaxDir
        }

        public enum AnimType
        {
            Walk,
            Wait,
            Attack,
            Max
        }



        [SerializeField]
        private Sprite[] _walk = new Sprite[(int)SpriteDir.MaxDir * 4];

        [SerializeField]
        private Sprite[] _normalWalk = new Sprite[(int)SpriteDir.MaxDir * 4];

        [SerializeField]
        private Sprite[] _wait = new Sprite[(int)SpriteDir.MaxDir * 6];

        [SerializeField]
        private Sprite[] _normalWait = new Sprite[(int)SpriteDir.MaxDir * 6];

        [SerializeField]
        private Sprite[] _attack = new Sprite[(int)SpriteDir.MaxDir * 7];

        [SerializeField]
        private Sprite[] _normalAttack = new Sprite[(int)SpriteDir.MaxDir * 7];

        [SerializeField]
        private float[] _AnimTime = new float[(int)AnimType.Max];

        private SpriteRenderer[] _spriteRenderer = new SpriteRenderer[(int)SpriteDir.MaxDir];

        private Color _emissionColor;

        private AnimType _currentType;
        private float _timer;
        private int _frame;

        private int[] _maxFrame;
        private bool _isLoop;

        // Start is called before the first frame update
        public void Start()
        {
            _emissionColor = Color.white;
            _emissionColor.a = 0.0f;
            if (Manager.GameMgr.Instance)
            {
                _emissionColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._SoldierEmissionColor;
            }
            _timer = 0.0f;

            for(int i=0;i<(int)SpriteDir.MaxDir;i++)
            {
                _spriteRenderer[i] = null;
            }

            _currentType = AnimType.Walk;

            _maxFrame = new int[(int)AnimType.Max];
            _maxFrame[(int)AnimType.Walk] = _walk.Length / (int)SpriteDir.MaxDir;
            _maxFrame[(int)AnimType.Wait] = _wait.Length / (int)SpriteDir.MaxDir;
            _maxFrame[(int)AnimType.Attack] = _attack.Length / (int)SpriteDir.MaxDir;

        }

        // Update is called once per frame
        public void Update()
        {
            float time = Time.deltaTime * Manager.GameMgr.Instance._gameSpeed;
            UpdateTimer(time);
        }

        private void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer < 0.0f)
            {
                _timer = _AnimTime[(int)_currentType];
                _frame++;
                if (!_isLoop)
                {
                    if (_frame >= _maxFrame[(int)_currentType])
                    {
                        _frame = _maxFrame[(int)_currentType] - 1;
                    }
                }
                else
                {
                    if (_frame % _maxFrame[(int)_currentType] == 0)
                    {
                        _frame -= _maxFrame[(int)_currentType];
                    }
                }
                for (int i = 0; i < Soldier._dir.Length; i++)
                {
                    SetSprite(Soldier._dir[i], _frame);
                }
            }
        }

        public void SetSprite(SpriteDir dir, int frame)
        {
            if (_spriteRenderer[(int)dir] == null) return;

            Sprite currentSprite = null;
            Sprite currentNormal = null;
            switch (_currentType)
            {
                case AnimType.Walk:
                    {
                        int index = (int)dir * _maxFrame[(int)AnimType.Walk] + frame;
                        currentSprite = _walk[index];
                        currentNormal = _normalWalk[index];
                        _isLoop = true;
                    }
                    break;
                case AnimType.Wait:
                    {
                        int index = (int)dir * _maxFrame[(int)AnimType.Wait] + frame;
                        currentSprite = _wait[index];
                        currentNormal = _normalWait[index];
                        _isLoop = true;
                    }
                    break;
                case AnimType.Attack:
                    {
                        int index = (int)dir * _maxFrame[(int)AnimType.Attack] + frame;
                        currentSprite = _attack[index];
                        currentNormal = _normalAttack[index];
                        _isLoop = false;
                    }
                    break;
                case AnimType.Max:
                    break;
            }

            if (currentSprite == null) return;

            _spriteRenderer[(int)dir].sprite = currentSprite;
            Material mat = _spriteRenderer[(int)dir].material;
            Sprite bumpSprite = currentNormal;
            mat.SetTexture("_BumpTex", bumpSprite.texture);         
            mat.SetColor("_Color", _emissionColor);
        }

        public void SetAnimationType(AnimType type, bool isRandom = false)
        {
            _currentType = type;
            _frame = 0;
            if(isRandom)
            {
                _frame = Random.Range(0, _maxFrame[(int)_currentType]);
            }
            SetSprite(_frame);
        }

        public void SetSprite(int frame)
        {
            for (int i = 0; i < Soldier._dir.Length; i++)
            {
                SetSprite(Soldier._dir[i], frame);
            }
        }

        public void SetRenderer(SpriteDir dir, SpriteRenderer renderer)
        {
            _spriteRenderer[(int)dir] = renderer;
        }
    }
}