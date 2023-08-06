using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Prepare : MonoBehaviour
    {
        /** ユーザータイプ */
        public Manager.UserType _Type;

        /** 通常時の色 */
        private Color _normalColor;

        /** 使用時の色 */
        private Color _execColor;

        /** 選択時の色 */
        private Color _selectColor;

        /** 準備位置保存用 */
        private PreparePosition[] _preparePositions;

        /** 使用フラグ */
        private bool[] _execPosition;

        /** 準備中カード */
        [HideInInspector]
        public Card.Card[] _prepareCard;

        private void Awake()
        {
            _normalColor = Manager.GameMgr.Instance._GameSetting._NormalColor;
            _execColor = Manager.GameMgr.Instance._GameSetting._ExecColor;
            _selectColor = Manager.GameMgr.Instance._GameSetting._SelectColor;
        }

        private void OnEnable()
        {
           
        }

        // Start is called before the first frame update
        void Start()
        {
            // キャラクター情報設定
            Manager.UserManager.Instance._characterInfo[(int)_Type]._prepare = this;

            GameObject position = transform.GetChild(1).gameObject;
            int count = position.transform.childCount;
            _preparePositions = new PreparePosition[count];
            _prepareCard = new Card.Card[count];
            _execPosition = new bool[count];
            for(int i=0;i<count;i++)
            {
                _preparePositions[i] = position.transform.GetChild(i).GetComponent<PreparePosition>();
                _preparePositions[i].SetIndex(i);
                _execPosition[i] = false;
                _prepareCard[i] = null;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitFlag()
        {
            GameObject position = transform.GetChild(1).gameObject;
            int count = position.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                _execPosition[i] = false;
                _prepareCard[i] = null;
            }

            UpdateNormalColor();

            SetCircleEffect(false);
        }

        public void UpdateNormalColor()
        {
            for (int i = 0; i < _preparePositions.Length; i++)
            {
                if (_execPosition[i]) continue;

                _preparePositions[i].SetNormalColor();
            }
        }

        public Color GetNormalColor()
        {
            return _normalColor;
        }

        public Color GetExecColor()
        {
            return _execColor;
        }

        public Color GetSelectColor()
        {
            return _selectColor;
        }

        public void SetExecFlag(int index, bool flag)
        {
            _execPosition[index] = flag;
        }

        public bool GetExecFlag(int index)
        {
            return _execPosition[index];
        }

        public int GetReadyCardCount()
        {
            int cnt = 0;
            for(int i=0;i<_prepareCard.Length;i++)
            {
                if (_prepareCard[i] == null) continue;
                if (!_prepareCard[i]._info._isReady) continue;
                cnt++;
            }

            return cnt;
        }

        public Card.Card GetReadyCard(bool isRandom)
        {
            int[] orderIndex = GetOrder(isRandom);

            for (int i=0;i< orderIndex.Length;i++)
            {
                int index = orderIndex[i];
                if (_prepareCard[index] == null) continue;
                if (!_prepareCard[index]._info._isReady) continue;

                return _prepareCard[index];
            }

            return null;
        }

        public PreparePosition GetPreparePosition(bool isRandom)
        {
            int[] orderIndex = GetOrder(isRandom);

            for (int i=0;i< orderIndex.Length;i++)
            {
                int index = orderIndex[i];
                if (_execPosition[index]) continue;

                return _preparePositions[index];
            }
            return null;
        }

        private int[] GetOrder(bool isRandom)
        {
            int[] orderIndex = new int[_execPosition.Length];
            for (int i = 0; i < _execPosition.Length; i++)
            {
                orderIndex[i] = i;
            }

            if (isRandom)
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

        public void SetCircleEffect(bool isStart)
        {
            foreach(PreparePosition pre in _preparePositions)
            {
                pre.SetCircleEffect(isStart);
            }
        }
    }

}