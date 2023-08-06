using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutLine_eff : MonoBehaviour
{
    public Material mat;
    public float tickness = 1.02f;
    [ColorUsage(true, true)] public Color OutlineColor;
    private SpriteRenderer ren;


    // Start is called before the first frame update
    void Start()
    {

        //ゲームオブジェクトならアウトライン表示
        GameObject OutlineObj = Instantiate(this.gameObject, transform.position, transform.rotation, transform);


        OutlineObj.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer ren = OutlineObj.GetComponent<SpriteRenderer>();
        ren.material = mat;
        ren.material.SetFloat("_tickness", tickness);
        ren.material.SetColor("_OutLineColor", OutlineColor);
        ren.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ren.enabled = false;
        OutlineObj.GetComponent<Collider>().enabled = false;
        OutlineObj.GetComponent<SpriteOutLine_eff>().enabled = false;
        this.ren = ren;

    }

    /// <summary>
    /// オブジェクトにマウスカーソルを合わせてアウトラインエフェクトを表示
    /// </summary>
    private void OnMouseExit()
    {
        Debug.Log("アウトライン表示");
        ren.enabled = false;
    }

    private void OnMouseEnter()
    {
        ren.enabled = true;
    }


    /// <summary>
    ///常にアウトライン表示
    /// </summary>
    //void Update()
    //{
    //    ren.enabled = true;
    //}
}
