using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class PreparePosition : MonoBehaviour
    {
        /** PrepareField */
        private Prepare _prepare;

        /** 番号 */
        private int _index;

        /** スプライトレンダラー */
        private Material _mat;

        /** 色 */
        private Color _normalColor;
        private Color _execColor;
        private Color _selectColor;

        private GameObject _summonEffect;
        private ParticleSystem[] _particle;

        private ParticleSystem[] _circleEffect;

        // Start is called before the first frame update
        void Start()
        {
            _mat = GetComponent<MeshRenderer>().material;
            _prepare = transform.root.GetComponent<Prepare>();
            _normalColor = _prepare.GetNormalColor();
            _execColor = _prepare.GetExecColor();
            _selectColor = _prepare.GetSelectColor();

            _circleEffect = GetComponentsInChildren<ParticleSystem>();
            SetCircleEffect(false);

            GameObject explisionObject = Manager.GameMgr.Instance._GameSetting._EffectData?._Summon;
            _summonEffect = Manager.EffectManager.Instance.StartEffect(explisionObject, transform.position);
            _particle = _summonEffect.GetComponentsInChildren<ParticleSystem>();
            SetSummonEffect(false);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void StartPrepareCard(Card.Card card)
        {
            SetExecColor();

            // 準備位置を使用中へ
            _prepare.SetExecFlag(_index, true);
            _prepare._prepareCard[_index] = card;

            SetSummonEffect(true);
        }

        public void EndPrepareCard()
        {
            SetSummonEffect(false);

            SetNormalColor();

            _prepare._prepareCard[_index] = null;
            _prepare.SetExecFlag(_index, false);
        }

        public void OnCursor()
        {
            if (_prepare.GetExecFlag(_index)) return;

            SetSelectColor();
        }

        public void SetNormalColor()
        {
            _mat.color = _normalColor;
        }

        public void SetExecColor()
        {
            _mat.color = _execColor;
        }

        public void SetSelectColor()
        {
            _mat.color = _selectColor;
        }

        public void SetIndex(int index)
        {
            _index = index;
        }

        public int GetIndex()
        {
            return _index;
        }

        public void SetSummonEffect(bool isActive)
        {
            //_summonEffect.SetActive(isActive);

            if(isActive)
            {
                foreach(ParticleSystem particle in _particle)
                {
                    particle.Play();
                }
            }
            else
            {
                foreach (ParticleSystem particle in _particle)
                {
                    particle.Stop();
                }
            }
        }

        public void SetCircleEffect(bool isActive)
        {
            //_summonEffect.SetActive(isActive);

            if (isActive)
            {
                foreach (ParticleSystem particle in _circleEffect)
                {
                    particle.Play();
                }
            }
            else
            {
                foreach (ParticleSystem particle in _circleEffect)
                {
                    particle.Stop();
                }
            }
        }

        public void SetSummonEffectHeight(float y)
        {
            Vector3 scale = _summonEffect.transform.localScale;
            scale.y = y;
            _summonEffect.transform.localScale = scale;
        }

    }
}
