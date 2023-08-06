using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
namespace Select
{
    public class Wanted : MonoBehaviour
    {
        private GameObject textObj;
        private TextMeshPro _text;

        private Vector3 _destPos;
        private Vector3 _startPos;

        private bool _isMove;
        private bool _isDestroy;

        private float _timer;
        private float _maxTime = 1.0f;

        private SpriteRenderer _image;

        private void Awake()
        {
            _isMove = false;
            _isDestroy = false;
            textObj = transform.Find("Text").gameObject;
            _text = textObj.GetComponent<TextMeshPro>();
            textObj.SetActive(false);
            _image = transform.Find("Image").GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void StartText()
        {
            textObj.SetActive(true);
        }

        public void StartMove(bool isDestroy, Vector3 dest)
        {
            if (_isMove) return;

            textObj.SetActive(false);
            _isMove = true;
            _startPos = transform.localPosition;
            _destPos = dest;
            _timer = 0.0f;
            _isDestroy = isDestroy;
        }

        private void Move()
        {
            if (!_isMove) return;

            _timer += Time.deltaTime;
            float rate = Mathf.Clamp01(_timer / _maxTime);
            transform.localPosition = Vector3.Lerp(_startPos, _destPos, rate);

            if(rate >= 1.0f)
            {
                if(_isDestroy)
                {
                    PaperManager.Instance.DestroyPaper(gameObject);
                }

                _isMove = false;
            }
        }

        public void SetStatus(Data.CharacterStatus status)
        {
            _text.text = status._Name;
            _image.sprite = status._CharacterFaceTexture;
        }
    }
}