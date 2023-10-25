using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartyData", menuName = "Project-01/PartyData", order = 0)]
public class PartyData : ScriptableObject
{
    [field:SerializeField]
    public uint Id { get; private set; }
    [field:SerializeField]
    public float MoveInterval { get; private set; }
    [field:SerializeField]
    public uint FormationId { get; private set; }
    [field:SerializeField]
    public IEnumerable<uint> FormationArgs { get; private set; }
}
