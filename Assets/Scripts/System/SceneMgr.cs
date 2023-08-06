using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : SingletonMonoBehaviour<SceneMgr>
{
    private void Awake()
    {
        CreateSound();

        CreateFade();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateSound()
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag("Sound");
        if (soundObject != null) return;

        GameObject soundSource = Resources.Load<GameObject>("System/Sound/SoundMixer");
        Instantiate(soundSource, Vector3.zero, Quaternion.identity);
    }

    private void CreateFade()
    {
        GameObject fadeObject = GameObject.FindGameObjectWithTag("Fade");
        if (fadeObject != null) return;

        GameObject fadeSource = Resources.Load<GameObject>("System/Fade/FadeCanvas");
        Instantiate(fadeSource, Vector3.zero, Quaternion.identity);
    }

}
