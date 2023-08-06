using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Soldier/Character", fileName = "SoldierCharacter")]
    public class SoldierCharacter : ScriptableObject
    {
        /** イラスト */
        [Header("カードイラスト")]
        public Sprite _CardIllust;

        /** アニメーション */
        [Header("アニメーション")]
        public Soldier.SoldierAnimation _Animation;
    }

}