using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class DeckManager : SingletonMonoBehaviour<DeckManager>
    {
        const string _CardPath = "Prototype/Card/";

        const string _CardDataPath = "Prototype/Data/CardData/";

        /** ÉJÅ[Éhê∂ê¨å≥ */
        private GameObject[] _cardSource;

        private Data.CardData[] _cardDatas;

        private void Awake()
        {
            _cardSource = Resources.LoadAll<GameObject>(_CardPath);

            _cardDatas = Resources.LoadAll<Data.CardData>(_CardDataPath);
        }

        public GameObject GetCardSource(int index)
        {
            if (index < 0 || _cardSource.Length <= index) return null;

            return _cardSource[index];
        }

        public int GetMaxCardSource()
        {
            return _cardSource.Length;
        }

        public Data.CardData GetCardData(int index)
        {
            if (index < 0 || _cardDatas.Length <= index) return null;

            return _cardDatas[index];
        }

        public int GetMaxCardData()
        {
            return _cardDatas.Length;
        }
    }
}