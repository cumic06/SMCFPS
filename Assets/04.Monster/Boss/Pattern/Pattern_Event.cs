using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern_Event : Pattern
{
    [SerializeField] private float patternCoolTime = 3f;
    protected TimeEndTrigger coolTimeTrigger;
    protected bool patternAble = true;

    public override PatternPriority Priority => PatternPriority.Medium;

    protected override void Start()
    {
        coolTimeTrigger = new TimeEndTrigger(patternCoolTime);
        coolTimeTrigger.onEnd += () =>
        {
            ResetPatternCondition();
            coolTimeTrigger.timer.CurrentTime = 0f;
            patternAble = true;
        };

        base.Start();
    }

    protected override bool Condition()
    {
        return patternAble;
    }
}
