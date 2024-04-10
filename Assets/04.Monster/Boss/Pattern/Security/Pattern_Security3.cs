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
        // �÷��̾� �̵��ӵ� 50% ����
        yield return Util.GetWait(4f);

        a.SetActive(false);
    }
}
