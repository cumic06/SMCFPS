using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device5 : Pattern
{
    [SerializeField] private ArcRocket rocketPrefab;
    [SerializeField] private Transform rocketFireTf;

    public override IEnumerator ExecutePattern()
    {
        Debug.Log(owner.Controller.IsInSight());
        yield return null;

        yield return owner.TurretRotate(owner.TurretMount);
        yield return owner.TurretRotate(owner.TurretHead);

        Vector3 euler = owner.TurretHead.localEulerAngles;
        euler.x = -10;

        yield return Util.GetWait(0.5f);

        yield return owner.TurretRotate(owner.TurretHead, Quaternion.Euler(euler), 3f);

        ArcRocket rocket = Instantiate(rocketPrefab, rocketFireTf.position, Quaternion.identity);
        rocket.Init(Player.Instance.transform.position);

        yield return Util.GetWait(1f);
    }

    protected override bool Condition()
    {
        return !owner.IsHalfOverHp && !owner.Controller.IsInSight();
    }

    public override void ResetPatternCondition()
    {
        base.ResetPatternCondition();
    }
}
