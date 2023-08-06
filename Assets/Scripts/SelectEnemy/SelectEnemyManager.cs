using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemyManager : SingletonMonoBehaviour<SelectEnemyManager>
{
    [SerializeField]
    private string _nextSceneName;

    [SerializeField]
    private AudioClip _BGMClip;

    
    public Data.EnemySetting _enemySetting;

    SoundManager _soundMgr = null;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject canvas = GameObject.Find("Canvas");
        //GameObject sourceObject = Resources.Load<GameObject>("Prototype/UI/EnemySelect/SelectPanel");
        //Instantiate(sourceObject, canvas.transform);

        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_soundMgr == null)
        {
            _soundMgr = SoundManager.Instance;
            if(_soundMgr)
            {
                _soundMgr.PlayAudio(SoundManager.SoundType.BGM, _BGMClip);
            }
        }
    }

    public void NextScene()
    {

    }
}
