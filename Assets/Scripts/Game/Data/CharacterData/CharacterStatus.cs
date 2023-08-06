using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/CharacterStatus", fileName = "CharacterStatus")]
    public class CharacterStatus : ScriptableObject
    {
        /** キャラクター名 */
        [Header("名前")]
        public string _Name;

        /** キャラクターフェイス */
        [Header("キャラクターフェイステクスチャ")]
        public Sprite _CharacterFaceTexture;

        /** キャラクターテクスチャ */
        [Header("キャラクターテクスチャ")]
        public Texture _CharacterTexture;

        /** キャラクター詳細 */
        [Header("詳細情報")]
        public string _Information;

        /** 生存 */
        [Header("HP")]
        public int _Life;

        [Header("キャラクターAI")]
        public BehaviourTree _behaviourTree = null;

        [Header("デッキ")]
        public DeckData _DeckData;
    }
}