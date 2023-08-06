using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    /** �ΏۃJ���� */
    private Camera _targetCamera;

    /** �ړI�𑜓x */
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
        // ��ʃA�X�y�N�g��
        var screenAspect = Screen.width / (float)Screen.height;
        // �ړI�A�X�y�N�g��
        var targetAspect = _aspectVec.x / _aspectVec.y;

        // �{��
        var magRate = targetAspect / screenAspect;

        // Viewport�����l��Rect���쐬
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
