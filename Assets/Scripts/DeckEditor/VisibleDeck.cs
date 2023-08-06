using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleDeck : MonoBehaviour
{
    private Data.DeckData[] _deckDataList;

    private List<GameObject> _cardList;

    // Start is called before the first frame update
    void Start()
    {
        _deckDataList = Resources.LoadAll<Data.DeckData>("Prototype/Data/DeckData");
        _cardList = new List<GameObject>();
        CreateDeck(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateCard(Data.CardData cardData, int num)
    {
        GameObject cardObj = null;
        for (int i = 0; i < num; i++)
        {
            cardObj = cardData.CreateGameCard(Manager.UserType.Player1);
            if (cardObj == null) return;
            cardObj.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f), Space.World);
            Card.Card card = cardObj.GetComponent<Card.Card>();
            Card.Info info = card.GetComponent<Card.Info>();

            info._userType = Manager.UserType.Player1;
            info._moveDir = 1;
            info._next = Card.CardState.None;

            _cardList.Add(cardObj);
        }
    }

    private void DeleteCardList()
    {
        foreach (GameObject obj in _cardList)
        {
            Destroy(obj);
        }
        _cardList.Clear();
    }

    private void CreateDeck(int index)
    {

        if (index < 0 || _deckDataList.Length <= index) return;
        DeleteCardList();

        Data.DeckData deckData = _deckDataList[index];
        for (int i = 0; i < deckData._deck.Count; i++)
        {
            CreateCard(deckData._deck[i], deckData._count[i]);
        }

        OrderCard();
    }

    private void OrderCard()
    {
        int max = _cardList.Count;
        int half = max / 2;
        for(int i=0;i<max;i++)
        {
            GameObject cardObj = _cardList[i];
            Vector3 scale = cardObj.transform.localScale;
            scale *= 2.0f;
            cardObj.transform.localScale = scale;

            Vector3 pos = cardObj.transform.position;
            pos.x = (scale.x * 1.2f) * (i % half) - (scale.x * 1.2f * half * 0.5f);
            pos.x -= ~(half % 2) * scale.x * 1.2f * 0.5f;
            pos.y = (scale.y * 1.5f) * (i / half) * -1 + (scale.y * 1.5f);
            cardObj.transform.position = pos;
        }
    }
}
