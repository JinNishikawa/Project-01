using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class DeckInfo : MonoBehaviour
{
    public Data.CardData data;
    public int count;
}


[Serializable]
public struct GlobalData
{
    public List<DeckInfo> _Deck;
    public string updateTime;
    public string name;
    public int fileNo;
}

public class SaveData : MonoBehaviour
{
    
}
