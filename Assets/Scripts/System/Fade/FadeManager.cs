using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public enum FADE_STAT
    {
        FADE_NONE = 0,
        FADE_IN,
        FADE_OUT
    };

    /** キャンバス召喚用フラグ */
    public static bool _isFadeInstance = false;

    /** フェード状態 */
    private static FADE_STAT _fadeState;

    /** 透過率 */
    private float _alpha = 0.0f;

    /** 次のシーン名 */
    private static string _nextSceneName;

    /** 速さ */
    private static float _fadeSpeed = 0.2f;

    /** 色 */
    private static Color _color = Color.black;

    /** パネル */
    private Image _image = null;

    private SoundManager _soundManager = null;

    private bool _isExecute = false;

    private void Awake()
    {
        if (!_isFadeInstance)
        {
            DontDestroyOnLoad(this);
            _isFadeInstance = true;
            _image = GetComponentInChildren<Image>();
            _isExecute = true;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = SoundManager.Instance;

        if (_isExecute)
        {
            _alpha = 1.0f;
            Execute(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FinGame();

        if (_soundManager == null)
        {
            _soundManager = SoundManager.Instance;
        }

        switch (_fadeState)
        {          
            case FADE_STAT.FADE_IN:     // フェードイン

                // α値更新
                _alpha -= Time.deltaTime / _fadeSpeed;
                if(_alpha <= 0.0f)
                {
                    SetVolume(1.0f);
                    _alpha = 0.0f;
                    _fadeState = FADE_STAT.FADE_NONE;
                    return;
                }

                _color.a = _alpha;
                _image.color = _color;
                SetVolume(1.0f - _alpha);
                break;
            
            
            case FADE_STAT.FADE_OUT:    //フェードアウト
                _alpha += Time.deltaTime / _fadeSpeed;
                if(_alpha >= 1.0f)
                {
                    _alpha = 1.0f;
                    SetVolume(0.0f);
                    _fadeState = FADE_STAT.FADE_IN;

                    _color.a = _alpha;
                    _image.color = _color;

                    _soundManager.DeleteSound();

                    // シーン遷移
                    SceneController.Change(_nextSceneName);
                    return;
                }

                _color.a = _alpha;
                _image.color = _color;
                SetVolume(1.0f - _alpha);
                break;
        }
    }

    private void FinGame()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }

    }

    public static bool StartFade(string name, Color color, float speed)
    {
        if (_fadeState != FADE_STAT.FADE_NONE) return  false;

        _fadeState = FADE_STAT.FADE_OUT;
        _nextSceneName = name;
        _fadeSpeed = speed;
        _color = color;

        return true;
        
    }

    public static void Execute(float speed)
    {
        if (_fadeState != FADE_STAT.FADE_NONE) return;

        _fadeState = FADE_STAT.FADE_IN;
        _fadeSpeed = speed;
    }

    private void SetVolume(float rate)
    {
        if (_soundManager == null) return;
        float maxVolume = _soundManager.GetMaxVolume(SoundManager.SoundType.MASTER);
        float volume = rate * maxVolume;
        _soundManager.SetVolume(SoundManager.SoundType.MASTER, volume);
    }

    static public FADE_STAT GetCurrentState()
    {
        return _fadeState;
    }
}
