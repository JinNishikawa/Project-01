using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase", menuName = "Project-01/CharacterData", order = 0)]
public class CharacterDataBase : ScriptableObject
{
    [SerializeField]
    private List<CharacterData> _datas;

    public IEnumerable<CharacterData> Datas { get { return _datas; } }

    [Serializable]
    public class CharacterData
    {
        [field: SerializeField]
        public uint Id { get; private set; }
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public Sprite Image { get; private set; }
        [field: SerializeField]
        public uint Hp { get; private set; }
        [field: SerializeField]
        public uint Atk { get; private set; }
    }
}
