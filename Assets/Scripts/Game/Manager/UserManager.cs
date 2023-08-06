using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public enum UserType
    {
        Player1,
        Player2,
        MaxPlayer
    }

    public class UserManager : SingletonMonoBehaviour<UserManager>
    {
        /** �L�����N�^�[���ۑ� */
        [HideInInspector]
        public Field.CharacterInfo[] _characterInfo = new Field.CharacterInfo[(int)UserType.MaxPlayer];

        /** �L�����N�^�[�X�e�[�^�X */
        public Data.CharacterStatus[] _CharacterStatus = new Data.CharacterStatus[(int)UserType.MaxPlayer];

        public void SetNextState()
        {
            Manager.GameMgr.Instance.Change(Manager.GameState.Ready);
        }

        public void SetCharacterStatus(UserType type, Data.CharacterStatus status)
        {
            _CharacterStatus[(int)type] = Instantiate(status);

            // �̗͕ύX
            _characterInfo[(int)type]._lifeMeter.InitFlag();

            // �e�N�X�`���ύX
            _characterInfo[(int)type].SetTexture(_CharacterStatus[(int)type]._CharacterTexture);

        }

        public void SetEnemyAI()
        {
            for (int i = 0; i < (int)UserType.MaxPlayer; i++)
            {
                _characterInfo[i].InitAI();
            }
        }

        public void InitStatus()
        {
            // �^�C���}�b�v������
            Field.Battle.Instance.InitFlag();

            for (int i=0;i<(int)UserType.MaxPlayer;i++)
            {
                // �f�b�L������
                _characterInfo[i]._deck.InitFlag();

                // �L�����N�^�[��񏉊���
                _characterInfo[i].InitFlag();

                // ���C�t
                _characterInfo[i]._lifeMeter.InitFlag();

                // ����
                _characterInfo[i]._prepare.InitFlag();
            }

            
        }
    }

}