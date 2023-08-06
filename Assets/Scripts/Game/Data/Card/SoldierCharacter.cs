using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Soldier/Character", fileName = "SoldierCharacter")]
    public class SoldierCharacter : ScriptableObject
    {
        /** �C���X�g */
        [Header("�J�[�h�C���X�g")]
        public Sprite _CardIllust;

        /** �A�j���[�V���� */
        [Header("�A�j���[�V����")]
        public Soldier.SoldierAnimation _Animation;
    }

}