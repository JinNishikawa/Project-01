using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class TitleManager : SingletonMonoBehaviour<TitleManager>
    {
        [SerializeField]
        private AudioClip _BGMClip;   

        SoundManager _soundMgr = null;

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
    }
}