using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCard : ActionNode
{
    Card.Card _card = null;
    Field.PreparePosition _prePos = null;
    /** カード移動時間 */
    private float _cardMoveTime;

    protected override void OnStart()
    {
        _card = blackboard._card;

        if (_card == null) return;

        Card.Move move = _card.gameObject.AddComponent<Card.Move>();
        Vector3 destPos = _card._initPos;

        bool isReady = _card._info._isReady;
        Card.CardState nextState = Card.CardState.None;

        _prePos = blackboard._prePos;

        _cardMoveTime = Manager.GameMgr.Instance._GameSetting._CardMoveTime;

        if (isReady)
        {
            //===== カード準備OK

            // 準備場所なし
            _prePos = null;

            // 次の状態
            nextState = Card.CardState.Departure;
            // 目的地
            destPos = blackboard._fieldPosition;

            // カード表示
            _card.SetCardVisible(false);

            //// 兵士サイズ戦闘サイズ
            //_card.SetSoldierScale(Vector3.one);
        }
        else
        {
            //===== カード準備NG
            if (_card._info._userType == Manager.UserType.Player2)
            {
                _card.SetVisibleSoldier(false);
            }

            if (_prePos != null)
            {
                //===== 準備場所あり

                // 目的地
                destPos = _prePos.transform.position;
                destPos.y += 0.001f;
                // 次の状態
                nextState = Card.CardState.Ready;

                // カードの準備場所設定
                _card._preparePosition = _prePos;
            }
            else
            {
                //==== 準備場所なし
                nextState = Card.CardState.Hand;
            }
        }

        _card._info._next = nextState;
        move.StartMove(destPos, _cardMoveTime);
        _card.Change(move);

        // 選択フィールド初期化
        int tileType = (int)Field.Battle.TileType.Select;
        TilemapFunction.SetClearAllTile(Field.Battle.Instance._tiles[tileType]);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(_card == null)
        {
            return State.Failure;
        }

        Card.Move move = _card.GetComponent<Card.Move>();
        if(move == null)
        {
            _card = null;
            _prePos = null;
            return State.Success;
        }

        return State.Runnnig;
    }
}
