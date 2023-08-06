using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCard : ActionNode
{
    Card.Card _card = null;
    Field.PreparePosition _prePos = null;
    /** �J�[�h�ړ����� */
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
            //===== �J�[�h����OK

            // �����ꏊ�Ȃ�
            _prePos = null;

            // ���̏��
            nextState = Card.CardState.Departure;
            // �ړI�n
            destPos = blackboard._fieldPosition;

            // �J�[�h�\��
            _card.SetCardVisible(false);

            //// ���m�T�C�Y�퓬�T�C�Y
            //_card.SetSoldierScale(Vector3.one);
        }
        else
        {
            //===== �J�[�h����NG
            if (_card._info._userType == Manager.UserType.Player2)
            {
                _card.SetVisibleSoldier(false);
            }

            if (_prePos != null)
            {
                //===== �����ꏊ����

                // �ړI�n
                destPos = _prePos.transform.position;
                destPos.y += 0.001f;
                // ���̏��
                nextState = Card.CardState.Ready;

                // �J�[�h�̏����ꏊ�ݒ�
                _card._preparePosition = _prePos;
            }
            else
            {
                //==== �����ꏊ�Ȃ�
                nextState = Card.CardState.Hand;
            }
        }

        _card._info._next = nextState;
        move.StartMove(destPos, _cardMoveTime);
        _card.Change(move);

        // �I���t�B�[���h������
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
