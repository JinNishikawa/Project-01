using Cysharp.Threading.Tasks;
using DG.Tweening;
using Omino.Infra.Master;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class Character : MonoBehaviour, IFactoryObject<CharacterData>
{
    [SerializeField]
    private SpriteRenderer _front;
    [SerializeField]
    private SpriteRenderer _back;

    private CharacterData _character;

    // TODO:　設定用のクラスまたはマスターテーブルに移行させる
    private string AtlasPath { get { return string.Format("Sprite/Character/C_{0:d3}", _character?.Id ?? 1); } }
    private string FrontSpriteName { get { return string.Format("C_{0:d3}_Front", _character?.Id ?? 1); } }
    private string BackSpriteName { get { return string.Format("C_{0:d3}_Idle_0", _character?.Id ?? 1); } }

    public async UniTask Setup(CharacterData data)
    {
        if (data == null)
            return;

        _character = data;
        _front!.sprite = await LoadCharacterSprite(FrontSpriteName);
        _back!.sprite = await LoadCharacterSprite(BackSpriteName);
    }

    private async UniTask<Sprite> LoadCharacterSprite(string spriteName)
    {
        var atlas = await Resources.LoadAsync<SpriteAtlas>(AtlasPath) as SpriteAtlas;
        return atlas?.GetSprite(spriteName);
    }
}

