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

        //�Q�[���I�u�W�F�N�g�Ȃ�A�E�g���C���\��
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
    /// �I�u�W�F�N�g�Ƀ}�E�X�J�[�\�������킹�ăA�E�g���C���G�t�F�N�g��\��
    /// </summary>
    private void OnMouseExit()
    {
        Debug.Log("�A�E�g���C���\��");
        ren.enabled = false;
    }

    private void OnMouseEnter()
    {
        ren.enabled = true;
    }


    /// <summary>
    ///��ɃA�E�g���C���\��
    /// </summary>
    //void Update()
    //{
    //    ren.enabled = true;
    //}
}
