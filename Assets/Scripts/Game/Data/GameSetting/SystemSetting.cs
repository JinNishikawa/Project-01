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

        [Header("�퓬�t�B�[���h���ʗp�F")]
        public Color[] _BattleFieldColors;

        [Header("�Q�[���X�^�C��")]
        public GameStyle _Style;

        [Header("���m�J�[�h�T�C�Y")]
        public float _SoldierCardSize;

        [Header("���m�t�B�[���h�T�C�Y")]
        public float _SoldierFieldSize;

        [Header("���m�����T�C�Y")]
        public float _SoldierReadySize;

        [Header("���m�͂ރT�C�Y")]
        public float _SoldierGrabSize;

        [Header("�}�e���A��")]
        public Material[] _Material;

        [Header("���m�G�~�b�V�����F"), ColorUsage(false, true)]
        public Color _SoldierEmissionColor;

        [Header("�A�E�g���C���T�C�Y")]
        public float _OutlineSize;

        [Header("�A�E�g���C���F"), ColorUsage(true, true)]
        public Color _OutlineColor;

        [Header("�f�B�]���u�p�}�e���A��")]
        public Material _DissolveMaterial;

        [Header("�Q�[���X�s�[�h�ݒ�")]
        public float _GameSpeed;
    }

}