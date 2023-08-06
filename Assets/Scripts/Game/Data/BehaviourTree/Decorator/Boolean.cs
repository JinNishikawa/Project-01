using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boolean : DecoratorNode
{
    public bool isFlag;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(isFlag)
        {    
            return child.Update();
        }
        else
        {
            return State.Failure;
        }

    }
}
