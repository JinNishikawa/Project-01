using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    /** 音タイプ */
    public enum SoundType
    {
        MASTER,
        BGM,
        SE,
        MAX
    }

    /** オーディオミキサー */
    [SerializeField]
    private AudioMixer _audioMixer;

    /** BGM出力オブジェクト */
    private GameObject _BGMSource;
    /** SE出力オブジェクト */
    private GameObject _SESource;

    private GameObject[] _soundParent;

    /** サウンド召喚用フラグ */
    private static bool _isSoundInstance = false;

    private float[] _currentVolume;
    private string[] _paramName = {"MasterVolume", "BGMVolume", "SEVolume"};

    [SerializeField]
    private float[] _maxVolume = new float[(int)SoundType.MAX];

    private void Awake()
    {
        if (!_isSoundInstance)
        {
            DontDestroyOnLoad(this);
            _isSoundInstance = true;

            _soundParent = new GameObject[2];
            _soundParent[0] = transform.Find("BGM").gameObject;
            _soundParent[1] = transform.Find("SE").gameObject;

            _BGMSource = Resources.Load<GameObject>("System/Sound/AudioBGM");
            _SESource = Resources.Load<GameObject>("System/Sound/AudioSE");

            _currentVolume = new float[(int)SoundType.MAX];
            for (int i=0;i<_currentVolume.Length;i++)
            {
                _currentVolume[i] = -100.0f;
            }
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(SoundType.MASTER, _maxVolume[(int)SoundType.MASTER]);
        SetVolume(SoundType.BGM, _maxVolume[(int)SoundType.BGM]);
        SetVolume(SoundType.SE, _maxVolume[(int)SoundType.SE]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(SoundType type, AudioClip clip)
    {
        GameObject audio = null;    
        if (type == SoundType.BGM)
        {
            audio = Instantiate(_BGMSource, _soundParent[0].transform);
        }
        else
        {
            audio = Instantiate(_SESource, _soundParent[1].transform);
        }

        if (audio == null) return;

        AudioBase audioBase = audio.GetComponent<AudioBase>();
        audioBase.Play(clip);
    }

    public void SetVolume(SoundType type, float volume)
    {
        if (Mathf.Approximately(_currentVolume[(int)type], volume))
            return;

        if (volume < 0.0f || 100.0f < volume) return;
        volume -= 80.0f;

        _audioMixer.SetFloat(_paramName[(int)type], volume);
        _currentVolume[(int)type] = volume;
    }

    public float GetMaxVolume(SoundType type)
    {
        float volume = _maxVolume[(int)type];
        return volume;
    }

    public void DeleteSound()
    {
        for(int i=0;i<_soundParent[0].transform.childCount;i++)
        {
            Destroy(_soundParent[0].transform.GetChild(i).gameObject);
        }

        for(int i=0;i<_soundParent[1].transform.childCount;i++)
        {
            Destroy(_soundParent[1].transform.GetChild(i).gameObject);
        }
    }
}
