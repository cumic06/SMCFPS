using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pattern : MonoBehaviour
{
    protected Boss owner;
    protected ConditionEndTrigger patternCondition;
    protected bool isPatternProcess;

    public virtual PatternPriority Priority => PatternPriority.Low;

    protected virtual void Awake()
    {
        owner = GetComponentInParent<Boss>();
    }

    protected virtual void Start()
    {
        ResetPatternCondition();
    }

    public virtual void ResetPatternCondition()
    {
        CreatePatternCondition();
    }

    protected virtual void CreatePatternCondition()
    {
        patternCondition = new ConditionEndTrigger(Condition, ConditionEvent);
        patternCondition.onEnd += OnConditionEnd;
        StartConditionTimer();
    }

    protected virtual void StartConditionTimer()
    {
        patternCondition.StartTimer();
    }

    protected virtual void OnConditionEnd()
    {
        owner.TryPatternStart(this);
        //if(owner.TryPatternStart(this))
        //{
        //    //patternAble = false;
        //}
    }
    
    private void ConditionEvent()
    {
        patternCondition.timer.TimerTime = 0f;
    }

    protected abstract bool Condition();

    public abstract IEnumerator ExecutePattern();
}

public enum PatternPriority
{
    None,
    Low,
    Medium,
    High
}