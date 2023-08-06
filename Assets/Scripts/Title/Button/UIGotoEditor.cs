using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class UIGotoEditor : UIButtonBase
    {
        [SerializeField]
        private string _nextSceneName;

        [SerializeField]
        private Color _fadeColor = Color.black;

        // Start is called before the first frame update
        void Start()
        {

        }

        public override void OnClick()
        {
            FadeManager.StartFade(_nextSceneName, _fadeColor, 1.0f);
        }

    }
}