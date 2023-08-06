using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [HideInInspector]
    public Card.Card _card = null;

    [HideInInspector]
    public Field.PreparePosition _prePos = null;

    [HideInInspector]
    public Vector3 _fieldPosition = Vector3.zero;
}
