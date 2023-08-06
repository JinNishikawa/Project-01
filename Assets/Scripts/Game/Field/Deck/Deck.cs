using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
namespace Field
{
    public class Deck : MonoBehaviour
    {
        /** ユーザータイプ */
        [SerializeField]
        private Manager.UserType _Type;

        /** 最大手札数 */
        private int _maxHand;

        [SerializeField]
        private int _MaxDeck;

        [SerializeField]
        private float _textRotTime;

        /** 手札までの移動時間 */
        private float _moveToHandTime;

        /** カードリスト(デッキ) */
        private List<GameObject> _deck;

        /** 生成カード位置 */
        private Vector3 _createPosition;

        /** 生成カードリスト */
        private List<GameObject> _createCardList;

        /** 手札 */
        [HideInInspector]
        public GameObject[] _hand;

        /** 手札位置 */
        private GameObject[] _handPosition;

        /** テキスト用オブジェクト */
        private GameObject _textObject;

        /** デッキ数表示用テキスト */
        private TextMeshPro[] _text;

        private Data.DeckData _deckData;

        private void Awake()
        {
            // 最大手札数
            _maxHand = Manager.GameMgr.Instance._GameSetting._MaxHand;
            // 手札への移動時間
            _moveToHandTime = Manager.GameMgr.Instance._GameSetting._MoveToHandTime;

            // カードリスト初期化
            _deck = new List<GameObject>();

            // 生成用カードリスト
            _createCardList = new List<GameObject>();

            // 手札位置初期化
            InitHand();

            _textObject = transform.Find("TextFront").gameObject;
            _text = GetComponentsInChildren<TextMeshPro>();
            for (int i=0;i<_text.Length;i++)
            {
                _text[i].text = "0";
            }
            _textObject.SetActive(false);
        }

        private void OnEnable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            // キャラクター情報設定
            Manager.UserManager.Instance._characterInfo[(int)_Type]._deck = this;

            _deckData = Manager.UserManager.Instance._CharacterStatus[(int)_Type]._DeckData;
        }

        // Update is called once per frame
        void Update()
        {
            for(int i=0;i<_deck.Count;i++)
            {
                if (_deck[i] == null) continue;
                Vector3 pos = transform.position;
                pos.y += i * 0.01f;
                _deck[i].transform.position = pos;
            }

            RotateCountText();
        }

        public void InitFlag()
        {
            // 手札初期化
            InitHandCard();
        }

        private void InitHandCard()
        {
            for(int i=0;i<_maxHand;i++)
            {
                // 手札初期化
                _hand[i] = null;
                break;
            }
        }

        public void PrepareDeck()
        {
            // カード生成
            CreateAllCard();

            // カードシャッフル
            CardShuffle();

            // デッキへセット
            SetDeck();
        }


        private void CreateAllCard()
        {
            // 生成カード用リスト初期化
            InitCreateCard();
            _deck.Clear();

            for (int i=0;i<_deckData._deck.Count;i++)
            {
                CreateCard(_deckData._deck[i], _deckData._count[i]);
            }
        }

        public void InitCreateCard()
        {
            foreach(GameObject obj in _createCardList)
            {
                Destroy(obj);
            }
            _createCardList.Clear();
            _createPosition = transform.Find("StartPosition").transform.position;

        }

        private void CreateCard(GameObject obj, int num)
        {
            GameObject cardObj = null;
            for (int i=0;i<num;i++)
            {
                cardObj = Instantiate(obj, _createPosition, Quaternion.Euler(-90, 0, 0));
                Card.Card card = cardObj.GetComponent<Card.Card>();
                Card.Info info = card.GetComponent<Card.Info>();
                info._userType = _Type;
                info._moveDir = 1;
                if (_Type == Manager.UserType.Player2)
                {
                    info._moveDir = -1;
                }
                info._next = Card.CardState.None;
                Vector3 pos = cardObj.transform.position;
                pos.y += i * 0.01f;
                cardObj.transform.position = pos;
                _createCardList.Add(cardObj);
            }
        }

        private void CreateCard(Data.CardData cardData, int num)
        {
            GameObject cardObj = null;
            for (int i = 0; i < num; i++)
            {
                cardObj = cardData.CreateGameCard(_Type);
                if (cardObj == null) return;
                Vector3 pos = _createPosition;
                pos.y += i * 0.01f;
                cardObj.transform.position = pos;
                cardObj.transform.rotation = Quaternion.Euler(-90, 0, 0);
                Card.Card card = cardObj.GetComponent<Card.Card>();
                Card.Info info = card.GetComponent<Card.Info>();

                info._userType = _Type;
                info._moveDir = 1;
                if (_Type == Manager.UserType.Player2)
                {
                    info._moveDir = -1;
                }
                info._next = Card.CardState.None;
                _createCardList.Add(cardObj);
            }
        }

        public void AddCardList(GameObject cardObj)
        {
            //cardObj.SetActive(false);
            _deck.Add(cardObj);

            for(int i=0;i<_text.Length;i++)
            {
                _text[i].text = _deck.Count.ToString();
            }

            if (Manager.GameMgr.Instance._currentState == Manager.GameState.Game)
            {
                SetHand();
            }
        }

        private void InitHand()
        {
            // 手札用配列生成
            _hand = new GameObject[_maxHand];
            InitHandCard();

            // 手札位置生成
            GameObject baseObj = new GameObject("HandBase");
            Vector3 pos = Vector3.zero;
            

            Data.SystemSetting.GameStyle style = Manager.GameMgr.Instance._GameSetting._SystemSetting._Style;
            if(style == Data.SystemSetting.GameStyle.Hori)
            {
                pos.z = -3.5f;
                if (_Type == Manager.UserType.Player2)
                {
                    pos.z *= -1;
                }
            }
            else
            {
                pos.x = -11.0f;
                pos.y = 0.5f;
                if (_Type == Manager.UserType.Player2)
                {
                    pos.x *= -1;
                }

            }
           
            baseObj.transform.position = pos;

            _handPosition = new GameObject[_maxHand];
            for(int i=0;i<_maxHand;i++)
            {
                GameObject obj = new GameObject("Hand" + (i + 1));
                obj.transform.parent = baseObj.transform;
                if (style == Data.SystemSetting.GameStyle.Hori)
                {
                    pos.x = 1.4f * (i - (_maxHand / 2));
                }
                else
                {
                    //pos.z = 1.4f * (i - (_maxHand / 2));
                    pos.z = 1.3f * (i - (_maxHand / 2));
                }
                obj.transform.position = pos;
                _handPosition[i] = obj;
            }

            if (style == Data.SystemSetting.GameStyle.Hori)
            {

            }
            else
            {
                Vector3 basePos = baseObj.transform.position;
                basePos.z = 3.5f;
                if (_Type == Manager.UserType.Player2)
                {
                    basePos.z *= -1;
                }
                baseObj.transform.position = basePos;
            }
        }

        public void SetHand()
        {
            if (_deck.Count <= 0) return;

            for (int i=0;i<_maxHand;i++)
            {
                if (_hand[i] != null) continue;
                // カードオブジェクト取得
                GameObject cardObj = _deck[_deck.Count - 1];

                cardObj.SetActive(true);

                // カード取得
                Card.Card card = cardObj.GetComponent<Card.Card>();
                // カード移動ステート追加
                Card.Move move = cardObj.AddComponent<Card.Move>();
                // 移動設定
                move.StartMove(_handPosition[i].transform.position, _moveToHandTime);

                // 次のステート設定
                card._info._next = Card.CardState.Hand;
                // ステート変更
                card.Change(move);

                // 手札設定
                _hand[i] = cardObj;

                // デッキから消去
                _deck.Remove(cardObj);

                break;
            }

            for (int i = 0; i < _text.Length; i++)
            {
                _text[i].text = _deck.Count.ToString();
            }
        }

        private void CardShuffle()
        {
            for (int i = _createCardList.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                var temp = _createCardList[i];
                _createCardList[i] = _createCardList[j];
                _createCardList[j] = temp;
            }
        }

        private void SetDeck()
        {
            for (int i=0;i<_createCardList.Count;i++)
            {
                Card.Card card = _createCardList[i].GetComponent<Card.Card>();
                card._info._next = Card.CardState.Deck;
                Card.Move move = card.gameObject.AddComponent<Card.Move>();
                Vector3 pos = transform.position;
                pos.y += 0.01f * i;
                move.StartMove(pos, _moveToHandTime);
                card.Change(move);
            }

            _createCardList.Clear();
        }

        public void RemoveHand(GameObject cardObj)
        {

            for(int i=0;i<_maxHand;i++)
            {
                if (_hand[i] != cardObj) continue;

                _hand[i] = null;

                // 手持ちへ移動
                SetHand();
                break;
            }
        }

        public void UnselectCard(GameObject selectCard)
        {
            for (int i = 0; i < _maxHand; i++)
            {
                if (_hand[i] == null) continue;

                Card.Card card = _hand[i].GetComponent<Card.Card>();
                Card.Hand hand = card.GetComponent<Card.Hand>();
                if (selectCard == _hand[i])
                {
                    hand?.StartSelect(true);
                    continue;
                }

                hand?.StartSelect(false);
            }
        }

        public int GetHandCount()
        {
            int cnt = 0;
            for(int i=0;i<_hand.Length;i++)
            {
                if (_hand[i] == null) continue;
                cnt++;
            }

            return cnt;
        }

        public Card.Card GetHandCard(bool isRandom)
        {
            int[] orderIndex = GetOrder(isRandom);

            for (int i=0;i< orderIndex.Length; i++)
            {
                int index = orderIndex[i];
                if (_hand[index] == null) continue;
                return _hand[index].GetComponent<Card.Card>();
            }

            return null;
        }

        private int[] GetOrder(bool isRandom)
        {
            int[] orderIndex = new int[_hand.Length];
            for(int i=0;i<_hand.Length;i++)
            {
                orderIndex[i] = i;
            }

            if(isRandom)
            {
                for (int i = orderIndex.Length - 1; i > 0; i--)
                {
                    var j = Random.Range(0, i + 1);
                    var temp = orderIndex[i];
                    orderIndex[i] = orderIndex[j];
                    orderIndex[j] = temp;
                }
            }

            return orderIndex;
        }

        private void RotateCountText()
        {
            if (_textObject == null) return;
            if (_textRotTime <= 0.0f) return;
            float rotY = 360.0f / _textRotTime;
            Vector3 rot = new Vector3(0.0f, rotY, 0.0f) * Time.deltaTime;
            _textObject.transform.Rotate(rot, Space.Self);
        }
    }
}