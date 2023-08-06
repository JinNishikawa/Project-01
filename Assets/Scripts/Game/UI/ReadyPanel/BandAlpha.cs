using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
namespace UIGame
{
    public class BandAlpha : MonoBehaviour
    {
        private Image[] _images;
        private TextMeshProUGUI[] _texts;
        private RawImage _rowImage;

        private float _maxTime = 0.5f;
        private float _timer;

        // Start is called before the first frame update
        void Start()
        {
            _images = GetComponentsInChildren<Image>();

            _texts = GetComponentsInChildren<TextMeshProUGUI>();

            _rowImage = GetComponentInChildren<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            float alpha = 0.0f;

            Color color;
            alpha = 1.0f - Mathf.Clamp01(_timer / _maxTime);
            for (int i = 0; i < _images.Length; i++)
            {
                color = _images[i].color;
                color.a = alpha;
                _images[i].color = color;
            }

            for(int i=0;i<_texts.Length;i++)
            {
                color = _texts[i].color;
                color.a = alpha;
                _texts[i].color = color;
            }

            color = _rowImage.color;
            color.a = alpha;
            _rowImage.color = color;

            if (_timer > _maxTime)
            {
                Manager.GameMgr.Instance.Change(Manager.GameState.Start);


                Destroy(this);
            }
        }
    }
}