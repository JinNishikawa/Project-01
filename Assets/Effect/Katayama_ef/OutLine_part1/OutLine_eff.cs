using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLine_eff : MonoBehaviour
{
    //public float tickness = 1.02f;
    //[ColorUsage(true, true)]
    //public Color OutlineColor;
    private MeshRenderer _ren;
    //GameObject OutlineObj;

    private void Awake()
    {
        _ren = GetComponentInChildren<MeshRenderer>();
        float tickness = Manager.GameMgr.Instance._GameSetting._SystemSetting._OutlineSize + 1.0f;
        _ren.material.SetFloat("_tickness", tickness);
        Color outlineColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._OutlineColor;
        _ren.material.SetColor("_OutLineColor", outlineColor);
        _ren.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _ren.enabled = false;

        //////ゲームオブジェクトならアウトライン表示
        ////GameObject sourceObject = Resources.Load<GameObject>("Prototype/Base/Surface");
        ////OutlineObj = Instantiate(sourceObject, transform.position, transform.rotation, transform);


        ////OutlineObj.transform.localScale = new Vector3(1, 1, 1);
        //_ren = GetComponent<MeshRenderer>();
        ////_ren.material = Manager.GameMgr.Instance._GameSetting._SystemSetting._OutlineMaterial;
        //_ren.material.SetFloat("_tickness", tickness);
        ////OutlineColor = Manager.GameMgr.Instance._GameSetting._SystemSetting._OutlineColor;
        //_ren.material.SetColor("_OutLineColor", OutlineColor);
        //_ren.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //_ren.enabled = false;
        ////OutlineObj.GetComponent<Collider>().enabled = false;
        ////OutlineObj.GetComponent<OutLine_eff>().enabled = false;
        ////this.ren = ren;

    }

    public void OnVisible(bool isVisible)
    {
        _ren.enabled = isVisible;
    }

    /// <summary>
    /// オブジェクトにマウスカーソルを合わせてアウトラインを表示
    /// </summary>
    private void OnMouseExit()
    {
        //Debug.Log("アウトライン表示");
        //_ren.enabled = false;
    }

    private void OnMouseEnter()
    {
        //_ren.enabled = true;
    }

    public void Fin()
    {
        
    }


    /// <summary>
    ///常にアウトライン表示
    /// </summary>
    //void Update()
    //{
    //    ren.enabled = true;
    //}
}
