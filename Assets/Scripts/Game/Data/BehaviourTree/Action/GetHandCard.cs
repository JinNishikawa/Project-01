using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHandCard : ActionNode
{
    /** ƒfƒbƒL */
    private Field.Deck _deck;

    [SerializeField]
    private bool _isRandom;

    protected override void OnStart()
    {
        _deck = Manager.UserManager.Instance._characterInfo[(int)Manager.UserType.Player2]._deck;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        int num = _deck.GetHandCount();

        if (num <= 0)
        {
            return State.Failure;
        }

        Card.Card card = _deck.GetHandCard(_isRandom);

        if (card == null)
        {
            return State.Failure;
        }

        blackboard._card = card;

        return State.Success;
    }
}
