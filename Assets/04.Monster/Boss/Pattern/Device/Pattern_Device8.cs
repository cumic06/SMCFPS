using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device8 : Pattern
{
    [SerializeField] private float limitDistance = 7f;
    [SerializeField] private int attackCount = 3;
    [SerializeField] private float damage = 100f;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private GameObject electricObj;
    [SerializeField] private float addRange = 2f;

    protected override void Start()
    {
        base.Start();

        owner.Controller.anyPatternEnded += AnyPatternEnded;
    }

    private void AnyPatternEnded(Pattern pattern)
    {
        if (electricObj.activeSelf)
            electricObj.SetActive(false);   
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        electricObj.SetActive(true);
        yield return Util.GetWait(1.5f);

        float range = limitDistance + addRange;
        for (int i = 0; i < attackCount; i++)
        {
            if(owner.GetDistanceFromPlayer() < range)
            {
                Player.Instance.TakeDamage(damage);
            }

            yield return Util.GetWait(delay);
        }
        electricObj.SetActive(false);
    }

    protected override bool Condition()
    {
        return !owner.IsHalfOverHp && owner.GetDistanceFromPlayer() < limitDistance;
    }
}
