using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    /** 対象カメラ */
    private Camera _targetCamera;

    /** 目的解像度 */
    [SerializeField]
    private Vector2 _aspectVec;

    // Start is called before the first frame update
    void Start()
    {
        _targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // 画面アスペクト比
        var screenAspect = Screen.width / (float)Screen.height;
        // 目的アスペクト比
        var targetAspect = _aspectVec.x / _aspectVec.y;

        // 倍率
        var magRate = targetAspect / screenAspect;

        // Viewport初期値でRectを作成
        var viewportRect = new Rect(0, 0, 1, 1);

        if(magRate < 1)
        {
            viewportRect.width = magRate;
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            viewportRect.height = 1 / magRate;
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }

        _targetCamera.rect = viewportRect;
    }
}
