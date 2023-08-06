using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
namespace System
{
    public class GameSpeedUI : MonoBehaviour
    {
        [SerializeField]
        private float _maxValue;
        [SerializeField]
        private float _minValue;

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private TextMeshProUGUI _text;

        // Start is called before the first frame update
        void Start()
        {
            _slider.value = Manager.GameMgr.Instance._gameSpeed;
            _slider.maxValue = _maxValue;
            _text.text = _slider.value.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeSpeed()
        {
            Manager.GameMgr.Instance._gameSpeed = _slider.value;
            _text.text = _slider.value.ToString();
        }

        public void ResetButton()
        {
            _slider.value = 1.0f;
            ChangeSpeed();
        }
    }
}