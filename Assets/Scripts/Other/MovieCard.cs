using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movie
{
    public class MovieCard : MonoBehaviour
    {
        private Data.DeckData[] _deckDataList;

        private List<GameObject> _cardList;

        private bool _isAlpah = false;

        private float _FallTime = 2.0f;

        private int _current;

        // Start is called before the first frame update
        void Start()
        {
            _deckDataList = Resources.LoadAll<Data.DeckData>("Test/Movie/Deck");
            _cardList = new List<GameObject>();
            CreateDeck(1, Manager.UserType.Player1);
            CreateDeck(0, Manager.UserType.Player2);

            _current = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                FallCard(_current);
                _current++;
                if(_current >= _cardList.Count)
                {
                    _current = 0;
                }
            }
        }

        private void UpdateAlpha()
        {
            if (!_isAlpah) return;

            int max = _cardList.Count;
            float time = Time.deltaTime;
            for (int i = 0; i < max; i++)
            {
                GameObject cardObj = _cardList[i];
                Card.Card card = cardObj.GetComponent<Card.Card>();
                if (card == null) continue;

                card._prepareTimeText.gameObject.SetActive(false);
                card.SetCardAlpha(0.5f);
            }

            _isAlpah = false;
        }

        private void FallCard(int index)
        {
            if (index < 0 || _cardList.Count <= index) return;

            GameObject cardObj = _cardList[index];
            Card.Card card = cardObj.GetComponent<Card.Card>();
            if (card == null) return;

            card._prepareTimeText.gameObject.SetActive(false);
            card.SetVisibleBase(false);
            card._speedStar.gameObject.SetActive(false);

            float x = Random.Range(-9.0f, 9.0f);
            Vector3 pos = cardObj.transform.position;
            pos.x = x;
            pos.y = 9.0f;
            cardObj.transform.position = pos;

            Vector3 goalPos = pos;
            goalPos.y = -7.0f;

            Card.Move move = cardObj.AddComponent<Card.Move>();
            // 移動設定
            move.StartMove(goalPos, _FallTime, Card.MoveType.QuadIn);

            // 次のステート設定
            card._info._next = Card.CardState.None;
            // ステート変更
            card.Change(move);
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

        private void CreateCard(Data.CardData cardData, int num, Manager.UserType type)
        {
            GameObject cardObj = null;
            for (int i = 0; i < num; i++)
            {
                cardObj = cardData.CreateGameCard(type);
                if (cardObj == null) return;
                cardObj.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f), Space.World);
                Card.Card card = cardObj.GetComponent<Card.Card>();
                Card.Info info = card.GetComponent<Card.Info>();

                info._userType = type;
                info._moveDir = 1;
                info._next = Card.CardState.None;

                // サイズ
                Vector3 scale = cardObj.transform.localScale;
                scale *= 2.0f;
                cardObj.transform.localScale = scale;

                // 位置
                Vector3 pos = cardObj.transform.position;
                pos.y = 9.0f;
                cardObj.transform.position = pos;

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

        private void CreateDeck(int index, Manager.UserType type)
        {
            if (index < 0 || _deckDataList.Length <= index) return;

            Data.DeckData deckData = _deckDataList[index];
            for (int i = 0; i < deckData._deck.Count; i++)
            {
                CreateCard(deckData._deck[i], deckData._count[i], type);
            }
        }

        private void CardReset()
        {
            InitCard();
            OrderCard();
        }

        private void InitCard()
        {
            int max = _cardList.Count;
            for (int i = 0; i < max; i++)
            {
                GameObject cardObj = _cardList[i];
                Card.Card card = cardObj.GetComponent<Card.Card>();
                if (card == null) continue;

                card._prepareTimeText.gameObject.SetActive(false);
                card.SetVisibleBase(false);
            }
        }

        private void OrderCard()
        {
            int max = _cardList.Count;
            for (int i = 0; i < max; i++)
            {
                GameObject cardObj = _cardList[i];
                Vector3 scale = cardObj.transform.localScale;
                scale *= 2.0f;
                cardObj.transform.localScale = scale;

                Vector3 pos = cardObj.transform.position;
                pos.y = 9.0f;
                cardObj.transform.position = pos;
            }
        }
    }
}