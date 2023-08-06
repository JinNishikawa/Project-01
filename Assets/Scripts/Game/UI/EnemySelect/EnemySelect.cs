using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UIGame
{
    public class EnemySelect : MonoBehaviour
    {
        /** �L�����N�^�[�C���[�W�e�N�X�`�� */
        private Image _characterImage;

        /** ���O�p�e�L�X�g */
        private TextMeshProUGUI _nameText;

        /** �ڍחp�e�L�X�g */
        private TextMeshProUGUI _infoText;

        private void Awake()
        {
            _characterImage = transform.Find("Image").GetComponent<Image>();

            _nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();

            _infoText = transform.Find("InfoText").GetComponent<TextMeshProUGUI>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCharacter(Data.CharacterStatus status)
        {
            _characterImage.sprite = status._CharacterFaceTexture;

            _nameText.text = status._Name;

            _infoText.text = status._Information;
        }

    }
}