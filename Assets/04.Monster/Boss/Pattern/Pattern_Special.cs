using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern_Special : Pattern
{
    [SerializeField] protected int limitCount = 1;
    protected bool patternAble = false;

    public override PatternPriority Priority => PatternPriority.High;

    public override void ResetPatternCondition()
    {
        if(limitCount >= 1)
            base.ResetPatternCondition();
    }

    protected override void OnConditionEnd()
    {
        base.OnConditionEnd();

        limitCount--;
    }

    protected override bool Condition()
    {
        return patternAble;
    }
}
