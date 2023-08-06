using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDebugLog : ActionNode
{
    public float duration = 1;
    float startTime;
    public string message;

    protected override void OnStart()
    {
        startTime = Time.time;
        Debug.Log($"OnStart{message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"OnStop{message}");
    }

    protected override State OnUpdate()
    {
        Debug.Log($"OnUpdate{message}");
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }

        return State.Runnnig;
    }
}
