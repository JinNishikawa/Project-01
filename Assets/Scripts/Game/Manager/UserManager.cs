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
        /** キャラクター情報保存 */
        [HideInInspector]
        public Field.CharacterInfo[] _characterInfo = new Field.CharacterInfo[(int)UserType.MaxPlayer];

        /** キャラクターステータス */
        public Data.CharacterStatus[] _CharacterStatus = new Data.CharacterStatus[(int)UserType.MaxPlayer];

        public void SetNextState()
        {
            Manager.GameMgr.Instance.Change(Manager.GameState.Ready);
        }

        public void SetCharacterStatus(UserType type, Data.CharacterStatus status)
        {
            _CharacterStatus[(int)type] = Instantiate(status);

            // 体力変更
            _characterInfo[(int)type]._lifeMeter.InitFlag();

            // テクスチャ変更
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
            // タイルマップ初期化
            Field.Battle.Instance.InitFlag();

            for (int i=0;i<(int)UserType.MaxPlayer;i++)
            {
                // デッキ初期化
                _characterInfo[i]._deck.InitFlag();

                // キャラクター情報初期化
                _characterInfo[i].InitFlag();

                // ライフ
                _characterInfo[i]._lifeMeter.InitFlag();

                // 準備
                _characterInfo[i]._prepare.InitFlag();
            }

            
        }
    }

}