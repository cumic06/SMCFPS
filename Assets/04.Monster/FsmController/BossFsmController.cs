using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFsmController : HumanoidFsmController
{
    public delegate void OnAnyPatternEnded(Pattern pattern);
    public event OnAnyPatternEnded anyPatternEnded;

    private Boss boss;
    private Pattern currentPattern;
    private WaitUntil patternWait;

    private Coroutine attackCor;
    private Coroutine patternCor;

    protected override void Awake()
    {
        boss = GetComponent<Boss>();
        sensor = GetComponentInChildren<Sensor>();
        patternWait = new WaitUntil(() => currentPattern != null);
    }

    protected override IEnumerator Start()
    {
        yield return null;

        attackCor = StartCoroutine(AttackPlayer());
    }

    protected override IEnumerator AttackPlayer()
    {
        while(true)
        {
            yield return patternWait;

            Debug.Log("Find Pattern");

            patternCor = StartCoroutine(currentPattern.ExecutePattern());
            yield return patternCor;

            PatternEnd();
        }
    }

    private void PatternEnd()
    {
        Debug.Log("Pattern End");
        anyPatternEnded?.Invoke(currentPattern);
        currentPattern = null;
        boss.StartTimer();
    }

    public bool TryPattern(Pattern pattern)
    {
        //if (!boss.PatternAble)
        //    return false;

        if (currentPattern != null)
        {
            if (currentPattern.Priority < pattern.Priority)
            {
                RestartCor();
                PatternEnd();
            }
            //else if(currentPattern.Priority == pattern.Priority)
            //{

            //}
            else
            {
                //boss.StartTimer();
                return false;
            }
        }

        currentPattern = pattern;

        return true;
    }

    private void RestartCor()
    {
        if (attackCor != null)
            StopCoroutine(attackCor);
        if (patternCor != null)
            StopCoroutine(patternCor);

        attackCor = StartCoroutine(AttackPlayer());
        Debug.Log("Restart Cor");
    }

    public bool IsInSight() => sensor.IsCheckPlayer();
}