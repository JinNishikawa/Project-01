using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGame
{
    public class SelectPanel : MonoBehaviour
    {
        private Data.CharacterStatus[] _enemyStatus;

        private int _currentEnemyIndex;

        private GameObject _characterInfoSource;

        private GameObject _Contents;

        private EnemySelect _enemySelect;

        private void Awake()
        {
            _enemyStatus = SelectEnemyManager.Instance._enemySetting._EnemyStatusList;
            _currentEnemyIndex = 0;

            _characterInfoSource = Resources.Load<GameObject>("Prototype/UI/EnemySelect/CharacterInfo");
            _Contents = transform.Find("Contents").gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {
            CreatePanel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CreatePanel()
        {
            GameObject panel = Instantiate(_characterInfoSource, _Contents.transform);
            _enemySelect = panel.GetComponent<EnemySelect>();
            _enemySelect.SetCharacter(_enemyStatus[_currentEnemyIndex]);
        }

        public void NextButton()
        {
            _currentEnemyIndex++;
            if(_enemyStatus.Length <= _currentEnemyIndex)
            {
                _currentEnemyIndex = 0;
            }

            _enemySelect.SetCharacter(_enemyStatus[_currentEnemyIndex]);
        }

        public void BackButton()
        {
            _currentEnemyIndex--;
            if (_currentEnemyIndex < 0)
            {
                _currentEnemyIndex = _enemyStatus.Length - 1;
            }

            _enemySelect.SetCharacter(_enemyStatus[_currentEnemyIndex]);
        }

        public void DecideEnemy()
        {
            bool isSuccess = FadeManager.StartFade("Game", Color.white, 1.0f);
            if (isSuccess)
            {
                GameObject sourceObj = Resources.Load<GameObject>("Game/SceneData/DataTransporter");
                GameObject transportObj = Instantiate(sourceObj, Vector3.zero, Quaternion.identity);
                DataTransporter data = transportObj.GetComponent<DataTransporter>();
                data._enemyStatus = _enemyStatus[_currentEnemyIndex];
            }
            //Manager.UserManager.Instance.SetCharacterStatus(Manager.UserType.Player2, _enemyStatus[_currentEnemyIndex]);

            //Manager.GameMgr.Instance.Change(Manager.GameState.Ready);
        }
    }
}