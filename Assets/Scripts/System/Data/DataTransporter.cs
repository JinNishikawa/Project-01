using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataTransporter : MonoBehaviour
{
    [HideInInspector]
    public Data.CharacterStatus _enemyStatus;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        SettingData();
    }

    public void SettingData()
    {
        Manager.UserManager manager = Manager.UserManager.Instance;
        if (manager == null) return;
   
        Manager.UserManager.Instance.SetCharacterStatus(Manager.UserType.Player2, _enemyStatus);

        // âèú
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        Destroy(gameObject);
    }
}
