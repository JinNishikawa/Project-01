using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Card", fileName = "CardData")]
    public class CardData : ScriptableObject
    {
        /** ���A���e�B */
        [Header("���A���e�B")]
        public Card.Rarity _Rarity;

        /** ���O */
        [Header("���O")]
        public string _Name;

        /** �w�` */
        [Header("�w�`")]
        public GameObject _Formation;

        /** �҂����� */
        [Header("��������")]
        public float _ReadyTime;

        /** �X�s�[�h */
        [Header("�i�s�X�e�b�v")]
        public Card.SpeedStep _Step;

        /** �R�X�g */
        [Header("�J�[�h�R�X�g")]
        public int _Cost;

        /** �L�����N�^�[ */
        [Header("�L�����N�^�[")]
        public SoldierCharacter[] _Character = new SoldierCharacter[2];

        [Header("���m�T�C�Y")]
        public float[] _SoldierScale = new float[2];

        [Header("���m�r�W���A��")]
        public bool[] _isOnly = new bool[2];

        /** �����p�J�[�h�x�[�X */
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