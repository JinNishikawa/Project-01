using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/EnemySetting", fileName = "EnemySetting")]
    public class EnemySetting : ScriptableObject
    {
        /** “GƒŠƒXƒg */
        public CharacterStatus[] _EnemyStatusList;
    }

}