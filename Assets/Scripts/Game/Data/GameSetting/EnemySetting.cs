using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/EnemySetting", fileName = "EnemySetting")]
    public class EnemySetting : ScriptableObject
    {
        /** �G���X�g */
        public CharacterStatus[] _EnemyStatusList;
    }

}