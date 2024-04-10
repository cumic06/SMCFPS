using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Security3 : Pattern
{
    [SerializeField] private GameObject a;

    protected override bool Condition()
    {
        return !owner.Controller.IsInSight();
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        a.SetActive(true);
        // 플레이어 이동속도 50% 감소
        yield return Util.GetWait(4f);

        a.SetActive(false);
    }
}
