using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Data
{
    [CreateAssetMenu(menuName = "Data/GameSetting", fileName = "GameSetting")]
    public class GameSetting : ScriptableObject
    {
        //==== �v���C���[�F
        /** Player�̐F */
        [Header("�v���C���[�F")]
        public Color[] _PlayerColor = new Color[(int)Manager.UserType.MaxPlayer];

        //==== BattleField
        /** �ݒu�^�C�� */
        [Header("�퓬�t�B�[���h��ɐݒu����^�C���x�[�X")]
        public TileBase _TileBase;

        /** �g�^�C�� */
        [Header("�g�p�^�C���x�[�X")]
        public TileBase _TileBaseLine;

        //==== PrepareField
        /** �ʏ펞�̐F */
        [Header("�����ꏊ�̒ʏ펞�̐F")]
        public Color _NormalColor;

        /** �������̐F */
        [Header("�����ꏊ�̎g�p���̐F")]
        public Color _ExecColor;

        /** �I�𒆂̐F */
        [Header("�����ꏊ�̑I�����̐F")]
        public Color _SelectColor;

        //==== DeckField
        /** �ő��D */
        [Header("�ő��D��")]
        public int _MaxHand;

        //==== �J�[�h�ړ�����
        /** �f�b�L�����D */
        [Header("�f�b�L�����D�ւ̈ړ�����")]
        public float _MoveToHandTime;

        /** �J�[�h�I�������� */
        [Header("�J�[�h�I���������̈ړ�����")]
        public float _CardMoveTime;

        /** �f�b�L�߂莞 */
        [Header("�U����f�b�L�܂ł̈ړ�����")]
        public float _MoveToDeckTime;

        /** �t�B�[���h�ړ� */
        [Header("�퓬�t�B�[���h��̈ړ�����")]
        public float _FieldMoveTime;

        //==== �}�E�X�I��
        /** �I�����̍ő�X�P�[�� */
        [Header("�}�E�X�I�����̍ő�X�P�[��")]
        public float _MaxScaleRate;

        /** �g�k���� */
        [Header("�}�E�X�I�����̊g�k����")]
        public float _ActionTime;

        /** �ړ����� */
        [Header("�}�E�X�I�����̈ړ�����")]
        public float _MoveDistanceRate;

        /** �����}�X */
        [Header("�����}�X��")]
        public int _InitGridCount;

        /** ���w�m�F�p�^�C���}�b�v���l */
        [Header("���w�p���l")]
        public float _MyFieldAlpha;

        //==== �v���C���[���x
        /** ���m�҂����� */
        [Header("���m�҂�����")]
        public float[] _SoldierWaitTime = new float[(int)Card.SpeedStep.MaxStep];

        /** ���m�Փˎ� */
        [Header("���m������ё��x")]
        public float _SoldierForceSpeed;

        //=== �J�[�h���
        /** �J�[�h�X���[�u�\ */
        [Header("�J�[�h�X���[�u�\")]
        public Texture[] _CardSurfaceFront = new Texture[(int)Manager.UserType.MaxPlayer];

        [Header("�J�[�h�g")]
        public Texture[] _CardLine = new Texture[(int)Manager.UserType.MaxPlayer];

        /** �J�[�h�X���[�u�� */
        [Header("�J�[�h�X���[�u��")]
        public Texture[] _CardSurfaceBack = new Texture[(int)Manager.UserType.MaxPlayer];

        [Header("�X�s�[�h�p")]
        public Texture[] _SpeedStar = new Texture[(int)Card.SpeedStep.MaxStep];

        //==== �G�t�F�N�g�ݒ�
        [Header("�G�t�F�N�g�ݒ�")]
        public EffectData _EffectData;

        //==== �G�ݒ�
        [Header("�G�ݒ�")]
        public EnemySetting _EnemySetting;

        //=== �V�X�e���p�ݒ�
        [Header("�V�X�e���ݒ�")]
        public SystemSetting _SystemSetting;

    }
}