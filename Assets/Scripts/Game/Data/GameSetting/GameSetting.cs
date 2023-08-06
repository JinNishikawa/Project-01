using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Data
{
    [CreateAssetMenu(menuName = "Data/GameSetting", fileName = "GameSetting")]
    public class GameSetting : ScriptableObject
    {
        //==== プレイヤー色
        /** Playerの色 */
        [Header("プレイヤー色")]
        public Color[] _PlayerColor = new Color[(int)Manager.UserType.MaxPlayer];

        //==== BattleField
        /** 設置タイル */
        [Header("戦闘フィールド上に設置するタイルベース")]
        public TileBase _TileBase;

        /** 枠タイル */
        [Header("枠用タイルベース")]
        public TileBase _TileBaseLine;

        //==== PrepareField
        /** 通常時の色 */
        [Header("準備場所の通常時の色")]
        public Color _NormalColor;

        /** 準備中の色 */
        [Header("準備場所の使用時の色")]
        public Color _ExecColor;

        /** 選択中の色 */
        [Header("準備場所の選択時の色")]
        public Color _SelectColor;

        //==== DeckField
        /** 最大手札 */
        [Header("最大手札数")]
        public int _MaxHand;

        //==== カード移動時間
        /** デッキから手札 */
        [Header("デッキから手札への移動時間")]
        public float _MoveToHandTime;

        /** カード選択解除時 */
        [Header("カード選択解除時の移動時間")]
        public float _CardMoveTime;

        /** デッキ戻り時 */
        [Header("攻撃後デッキまでの移動時間")]
        public float _MoveToDeckTime;

        /** フィールド移動 */
        [Header("戦闘フィールド上の移動時間")]
        public float _FieldMoveTime;

        //==== マウス選択
        /** 選択時の最大スケール */
        [Header("マウス選択時の最大スケール")]
        public float _MaxScaleRate;

        /** 拡縮時間 */
        [Header("マウス選択時の拡縮時間")]
        public float _ActionTime;

        /** 移動距離 */
        [Header("マウス選択時の移動距離")]
        public float _MoveDistanceRate;

        /** 初期マス */
        [Header("初期マス数")]
        public int _InitGridCount;

        /** 自陣確認用タイルマップα値 */
        [Header("自陣用α値")]
        public float _MyFieldAlpha;

        //==== プレイヤー速度
        /** 兵士待ち時間 */
        [Header("兵士待ち時間")]
        public float[] _SoldierWaitTime = new float[(int)Card.SpeedStep.MaxStep];

        /** 兵士衝突時 */
        [Header("兵士吹き飛び速度")]
        public float _SoldierForceSpeed;

        //=== カード情報
        /** カードスリーブ表 */
        [Header("カードスリーブ表")]
        public Texture[] _CardSurfaceFront = new Texture[(int)Manager.UserType.MaxPlayer];

        [Header("カード枠")]
        public Texture[] _CardLine = new Texture[(int)Manager.UserType.MaxPlayer];

        /** カードスリーブ裏 */
        [Header("カードスリーブ裏")]
        public Texture[] _CardSurfaceBack = new Texture[(int)Manager.UserType.MaxPlayer];

        [Header("スピード用")]
        public Texture[] _SpeedStar = new Texture[(int)Card.SpeedStep.MaxStep];

        //==== エフェクト設定
        [Header("エフェクト設定")]
        public EffectData _EffectData;

        //==== 敵設定
        [Header("敵設定")]
        public EnemySetting _EnemySetting;

        //=== システム用設定
        [Header("システム設定")]
        public SystemSetting _SystemSetting;

    }
}