using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    [CreateAssetMenu(menuName = "Data/SystemSetting", fileName = "SystemSetting")]
    public class SystemSetting : ScriptableObject
    {
        public enum GameStyle
        {
            Hori,
            Ver
        }

        [Header("戦闘フィールド判別用色")]
        public Color[] _BattleFieldColors;

        [Header("ゲームスタイル")]
        public GameStyle _Style;

        [Header("兵士カードサイズ")]
        public float _SoldierCardSize;

        [Header("兵士フィールドサイズ")]
        public float _SoldierFieldSize;

        [Header("兵士準備サイズ")]
        public float _SoldierReadySize;

        [Header("兵士掴むサイズ")]
        public float _SoldierGrabSize;

        [Header("マテリアル")]
        public Material[] _Material;

        [Header("兵士エミッション色"), ColorUsage(false, true)]
        public Color _SoldierEmissionColor;

        [Header("アウトラインサイズ")]
        public float _OutlineSize;

        [Header("アウトライン色"), ColorUsage(true, true)]
        public Color _OutlineColor;

        [Header("ディゾルブ用マテリアル")]
        public Material _DissolveMaterial;

        [Header("ゲームスピード設定")]
        public float _GameSpeed;
    }

}