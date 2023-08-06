using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioBase : MonoBehaviour
{
    private AudioSource _audioSource;

    private bool _isPlay;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _isPlay = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlay && !_audioSource.loop && !_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        _isPlay = true;
    }
}
