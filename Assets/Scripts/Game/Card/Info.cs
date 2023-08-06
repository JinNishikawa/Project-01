using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Info : MonoBehaviour
    {
        //===== 外部入力なし
        /** 準備時間 */
        [HideInInspector]
        public float _readyTime;

        /** スピードステート */
        [HideInInspector]
        public SpeedStep _speedStep;

        /** アニメーション */
        [HideInInspector]
        public Soldier.SoldierAnimation _animation;

        [HideInInspector]
        public float _soldierScale;

        /** カードコスト */
        [HideInInspector]
        public int _cost;

        /** カードレアリティ */
        [HideInInspector]
        public Rarity _rarity;

        /** カードイラスト */
        [HideInInspector]
        public Sprite _illust;

        /** 兵士待機時間 */
        [HideInInspector]
        public float _soldierWaitTime;


        [HideInInspector]
        public bool _isOnlyVisual;

        // 移動方向
        [HideInInspector]
        public int _moveDir;

        // プレイヤータイプ
        [HideInInspector]
        public Manager.UserType _userType;

        /** 準備できてるか */
        [HideInInspector]
        public bool _isReady;

        /** 次のステート */
        [HideInInspector]
        public CardState _next = CardState.None;

        /** 残り兵士数 */
        [HideInInspector]
        public int _remainCount;
    }
}
