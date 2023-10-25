using System;

using UnityEngine;

[CreateAssetMenu(fileName = "FormationData", menuName = "Project-01/FormationData", order = 0)]
public class FormationData : ScriptableObject
{
    [field: SerializeField]
    public uint Id { get; private set; }
    [field: SerializeField]
    public uint[,] Formation { get; private set; }
}

