using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingTexture : MonoBehaviour
{
    private Material _mat;

    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private Texture _texture;

    [SerializeField]
    Vector2 _textureOffset;

    [SerializeField]
    Vector2 _textureScale;

    private void Awake()
    {
        if (_texture == null && _sprite != null)
        {
            _texture = _sprite.texture;
            _textureScale = _sprite.rect.size / _sprite.texture.width;
            _textureOffset = _sprite.rect.position / _sprite.texture.width;
        }

        _mat = GetComponent<MeshRenderer>().material;
        _mat.SetTexture("_BaseMap", _texture);
        _mat.SetTextureOffset("_BaseMap", _textureOffset);
        _mat.SetTextureScale("_BaseMap", _textureScale);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScale(int dir)
    {
        _textureScale.x *= dir;
        _mat.SetTextureScale("_BaseMap", _textureScale);
    }

    public void SetScale(Vector2 scale)
    {
        _textureScale = scale;
        _mat.SetTextureScale("_BaseMap", _textureScale);
    }

    public void SetTexture(Texture tex)
    {
        if (tex == null) return;
        Material currentMat = GetComponent<MeshRenderer>().material;
        if(currentMat != _mat)
        {
            _mat = currentMat;
        }

        _texture = tex;
        _mat.SetTexture("_BaseMap", tex);
    }

    public void SetTexture(Sprite sprite)
    {
        if (sprite == null) return;
        // テクスチャ設定
        _texture = _sprite.texture;
        // サイズ設定
        _textureScale = sprite.rect.size / sprite.texture.width;
        // オフセット設定
        _textureOffset = sprite.rect.position / sprite.texture.width;
        
        // マテリアル適用
        _mat.SetTexture("_BaseMap", _texture);
        _mat.SetTextureOffset("_BaseMap", _textureOffset);
        _mat.SetTextureScale("_BaseMap", _textureScale);
    }

    public void UpdateMaterial()
    {
        _mat = GetComponent<MeshRenderer>().material;
    }
}
