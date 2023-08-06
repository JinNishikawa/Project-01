using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIGame
{
    public class LineAppear : MonoBehaviour
    {
        Image[] _image;

        float _maxTime = 2.0f;
        float _timer;
        float _clip;

        private void Awake()
        {
            _image = new Image[transform.childCount];
            for(int i=0;i<_image.Length;i++)
            {
                _image[i] = transform.GetChild(i).GetComponent<Image>();
            }
            _clip = 1.0f;

            _timer = 0.0f;
        }

        // Start is called before the first frame update
        void Start()
        {
            SetMaterialClip();
        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            _clip = 1.0f - Mathf.Clamp01(_timer / _maxTime);

            SetMaterialClip();

            if(_timer > _maxTime)
            {
                _clip = 0.0f;
                SetMaterialClip();

                for (int i = 0; i < _image.Length; i++)
                {
                    _image[i].material = null;
                }

                ReadyPanel panel = GetComponentInParent<ReadyPanel>();
                panel.NextType(ReadyPanel.Type.White);

                Destroy(this);
            }
        }

        void SetMaterialClip()
        {
            for (int i = 0; i < _image.Length; i++)
            {
                _image[i].material.SetFloat("_Clip", _clip);
            }
        }
    }
}