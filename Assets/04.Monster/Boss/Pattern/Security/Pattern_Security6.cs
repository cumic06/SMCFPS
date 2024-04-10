using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Security6 : Pattern_Special
{
    [SerializeField] private Transform teleportTf;

    protected override void Start()
    {
        base.Start();

        EnemyControlModule.allECMDied += OnAllECMDied;
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        Debug.Log("∆–≈œ 6");
        owner.MovePosition(teleportTf.position);
    }

    private void OnAllECMDied()
    {
        patternAble = true;
    }
}
