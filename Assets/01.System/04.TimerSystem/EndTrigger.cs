using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EndTrigger
{
    public Action onEnd;
    public TimerAgent timer;

    public virtual void StartTimer()
    {
        timer.CurrentTime = 0f;
        TimerSystem.Instance.AddTimer(timer);
    }
}

public class TimeEndTrigger : EndTrigger
{
    public TimeEndTrigger(float time)
    {
        timer = new TimerAgent(time, updateAction: null, endAction: d => onEnd?.Invoke());
    }
}

public class ConditionEndTrigger : EndTrigger
{
    private bool firstCondition = false;

    public ConditionEndTrigger(Func<bool> condition, Action conditionEvent)
    {
        timer = new TimerAgent(float.MaxValue,
            updateAction: d =>
            {
                if (firstCondition)
                    return;

                if (condition())
                {
                    firstCondition = true;
                    conditionEvent?.Invoke();
                }
            },
            endAction: d =>
            {
                onEnd?.Invoke();
            });
    }

    public override void StartTimer()
    {
        firstCondition = false;
        base.StartTimer();
    }
}