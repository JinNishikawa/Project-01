using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Card", fileName = "CardData")]
    public class CardData : ScriptableObject
    {
        /** レアリティ */
        [Header("レアリティ")]
        public Card.Rarity _Rarity;

        /** 名前 */
        [Header("名前")]
        public string _Name;

        /** 陣形 */
        [Header("陣形")]
        public GameObject _Formation;

        /** 待ち時間 */
        [Header("準備時間")]
        public float _ReadyTime;

        /** スピード */
        [Header("進行ステップ")]
        public Card.SpeedStep _Step;

        /** コスト */
        [Header("カードコスト")]
        public int _Cost;

        /** キャラクター */
        [Header("キャラクター")]
        public SoldierCharacter[] _Character = new SoldierCharacter[2];

        [Header("兵士サイズ")]
        public float[] _SoldierScale = new float[2];

        [Header("兵士ビジュアル")]
        public bool[] _isOnly = new bool[2];

        /** 生成用カードベース */
        private GameObject _cardBase;

        public GameObject CreateGameCard(Manager.UserType type)
        {
            GameObject cardObj = null;

            _cardBase = Resources.Load<GameObject>("Prototype/Base/CardBase");
            cardObj = Instantiate(_cardBase);
            GameObject formation = Instantiate(_Formation, cardObj.transform);
            formation.name = "Soldiers";

            Card.Info info = cardObj.GetComponent<Card.Info>();
            info._speedStep = _Step;
            info._readyTime = _ReadyTime;
            info._rarity = _Rarity;
            info._cost = _Cost;
            info._illust = _Character[(int)type]._CardIllust;
            info._animation = _Character[(int)type]._Animation;
            info._soldierScale = _SoldierScale[(int)type];
            info._isOnlyVisual = _isOnly[(int)type];

            if(info._isOnlyVisual)
            {
                GameObject source = Resources.Load<GameObject>("Prototype/Soldier/SoldierVisual");
                GameObject visual = Instantiate(source, cardObj.transform);
                visual.name = "SoldierVisual";
            }

            return cardObj;
        }
    }
}