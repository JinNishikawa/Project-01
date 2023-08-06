using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPrepareCard : ActionNode
{
    /** �����ʒu */
    private Field.Prepare prepare;

    /** �ʒu�����_�� */
    [SerializeField]
    private bool _isRandom;

    protected override void OnStart()
    {
        prepare = Manager.UserManager.Instance._characterInfo[(int)Manager.UserType.Player2]._prepare;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        int num = prepare.GetReadyCardCount();
        
        if(num <= 0)
        {
            return State.Failure;
        }

        Card.Card card = prepare.GetReadyCard(_isRandom);

        if(card == null)
        {
            return State.Failure;
        }

        blackboard._card = card;



        return State.Success;
    }

}
