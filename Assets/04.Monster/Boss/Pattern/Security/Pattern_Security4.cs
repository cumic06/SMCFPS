using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Security4 : Pattern_Event
{
    private bool onECMDamaged = false;

    protected override void Start()
    {
        base.Start();

        EnemyControlModule.anyECMDamaged += OnAnyECMDamaged;
    }

    public override IEnumerator ExecutePattern()
    {
        patternAble = false;
        yield return null;

        // 주무기 해킹
        Debug.Log("주무기 해킹");

        yield return Util.GetWait(2f);

        coolTimeTrigger.StartTimer();
        onECMDamaged = false;
    }

    protected override bool Condition()
    {
        return base.Condition() && onECMDamaged;
    }

    private void OnAnyECMDamaged()
    {
        Debug.Log("FF");
        onECMDamaged = true;
    }
}
