using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class DeckEditorManager : SingletonMonoBehaviour<DeckEditorManager>
    {
        [SerializeField]
        private string _nextSceneName;

        [SerializeField]
        private AudioClip _BGMClip;


        SoundManager _soundMgr = null;

        /** ÉQÅ[ÉÄê›íË */
        public Data.GameSetting _GameSetting;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_soundMgr == null)
            {
                _soundMgr = SoundManager.Instance;
                if (_soundMgr)
                {
                    _soundMgr.PlayAudio(SoundManager.SoundType.BGM, _BGMClip);
                }
            }
        }

        public void NextScene()
        {
            FadeManager.StartFade(_nextSceneName, Color.black, 1.0f);
        }

    }
}