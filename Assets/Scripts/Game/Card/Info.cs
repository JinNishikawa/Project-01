using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public class Info : MonoBehaviour
    {
        //===== �O�����͂Ȃ�
        /** �������� */
        [HideInInspector]
        public float _readyTime;

        /** �X�s�[�h�X�e�[�g */
        [HideInInspector]
        public SpeedStep _speedStep;

        /** �A�j���[�V���� */
        [HideInInspector]
        public Soldier.SoldierAnimation _animation;

        [HideInInspector]
        public float _soldierScale;

        /** �J�[�h�R�X�g */
        [HideInInspector]
        public int _cost;

        /** �J�[�h���A���e�B */
        [HideInInspector]
        public Rarity _rarity;

        /** �J�[�h�C���X�g */
        [HideInInspector]
        public Sprite _illust;

        /** ���m�ҋ@���� */
        [HideInInspector]
        public float _soldierWaitTime;


        [HideInInspector]
        public bool _isOnlyVisual;

        // �ړ�����
        [HideInInspector]
        public int _moveDir;

        // �v���C���[�^�C�v
        [HideInInspector]
        public Manager.UserType _userType;

        /** �����ł��Ă邩 */
        [HideInInspector]
        public bool _isReady;

        /** ���̃X�e�[�g */
        [HideInInspector]
        public CardState _next = CardState.None;

        /** �c�蕺�m�� */
        [HideInInspector]
        public int _remainCount;
    }
}
