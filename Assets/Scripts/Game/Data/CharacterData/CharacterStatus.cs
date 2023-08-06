using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/CharacterStatus", fileName = "CharacterStatus")]
    public class CharacterStatus : ScriptableObject
    {
        /** �L�����N�^�[�� */
        [Header("���O")]
        public string _Name;

        /** �L�����N�^�[�t�F�C�X */
        [Header("�L�����N�^�[�t�F�C�X�e�N�X�`��")]
        public Sprite _CharacterFaceTexture;

        /** �L�����N�^�[�e�N�X�`�� */
        [Header("�L�����N�^�[�e�N�X�`��")]
        public Texture _CharacterTexture;

        /** �L�����N�^�[�ڍ� */
        [Header("�ڍ׏��")]
        public string _Information;

        /** ���� */
        [Header("HP")]
        public int _Life;

        [Header("�L�����N�^�[AI")]
        public BehaviourTree _behaviourTree = null;

        [Header("�f�b�L")]
        public DeckData _DeckData;
    }
}