using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device6 : Pattern_Event
{
    [SerializeField] private int patternCount = 3;
    [SerializeField] private int healCount = 10;
    [SerializeField] private float healCycle = 0.5f;
    [SerializeField] private float healAmount = -60;
    [SerializeField] private GameObject healObj; 

    private bool damaged;

    protected override void Start()
    {
        base.Start();

        patternAble = false;
        owner.onDamaged += OnBossDamaged;
    }

    public override IEnumerator ExecutePattern()
    {
        patternCount--;

        yield return null;

        healObj.SetActive(true);

        for (int i = 0; i < healCount; i++)
        {
            for (float j = 0; j < healCycle; j+=Time.deltaTime)
            {
                if (damaged)
                    break;

                yield return null;
            }

            if (damaged)
                break;
            // Heal
            Debug.Log("Heal");
            owner.TakeDamage(healAmount);
        }

        //damaged = false;
        //coolTimeTrigger.StartTimer();
        patternAble = false;
        healObj.SetActive(false);
        ResetPatternCondition();
    }

    protected override bool Condition()
    {
        if (damaged)
        {
            coolTimeTrigger.timer.CurrentTime = 0.0f;
            damaged = false;
        }

        return owner.PatternAble && patternCount > 0 && base.Condition();
    }

    protected override void StartConditionTimer()
    {
        base.StartConditionTimer();

        damaged = false;
        coolTimeTrigger.StartTimer();
    }

    private void OnBossDamaged()
    {
        damaged = true;
    }
}
