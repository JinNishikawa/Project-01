using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPreparePosition : ActionNode
{
    /** �����ʒu */
    private Field.Prepare prepare;

    /** �����_���t���O */
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
        Field.PreparePosition preparePosition = prepare.GetPreparePosition(_isRandom);

        if(preparePosition == null)
        {
            return State.Failure;
        }

        blackboard._prePos = preparePosition;
        return State.Success;
    }
}
