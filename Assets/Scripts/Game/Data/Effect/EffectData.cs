using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {

    [CreateAssetMenu(menuName = "Data/EffectData", fileName = "EffectData")]
    public class EffectData : ScriptableObject
    {

        [Header("HP���j")]
        public GameObject _HPExplosion;

        [Header("���m�o��")]
        public GameObject _SoldierAppear;

        [Header("���@�w")]
        public GameObject _MagicCircle;

        [Header("����")]
        public GameObject _Summon;

        [Header("����")]
        public GameObject _WalkSmoke;

        [Header("�A�E�g���C��")]
        public GameObject _OutLine;

    }
}