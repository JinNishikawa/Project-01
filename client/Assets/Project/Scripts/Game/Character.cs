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
    private string FrontSpriteResourcesPath { get { return string.Format("Sprite/Character/C_{0:d3}_Front", _character?.Id ?? 1); } }
    private string BackSpriteResourcesPath { get { return string.Format("Sprite/Character/C_{0:d3}_Idle", _character?.Id ?? 1); } }
    private string FrontSpriteName { get { return string.Format("C_{0:d3}_Front", _character?.Id ?? 1); } }
    private string BackSpriteName { get { return string.Format("C_{0:d3}_Idle_0", _character?.Id ?? 1); } }

    public async UniTask Setup(CharacterData data)
    {
        if (data == null)
            return;

        _character = data;
        _front.sprite = await LoadCharacterSprite(FrontSpriteResourcesPath, FrontSpriteName);
        _back.sprite = await LoadCharacterSprite(BackSpriteResourcesPath, BackSpriteName);
    }

    private async UniTask<Sprite> LoadCharacterSprite(string path, string spriteName)
    {
        var atlas = await Resources.LoadAsync<SpriteAtlas>(path) as SpriteAtlas;
        return atlas?.GetSprite(spriteName);
    }
}

