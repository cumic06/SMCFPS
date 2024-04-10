using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device7 : Pattern
{
    [SerializeField] private float delay = 0.7f;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Transform bulletFireTf;
    [SerializeField] private float bulletSpeed = 30f;

    public override IEnumerator ExecutePattern()
    {
        isPatternProcess = true;

        yield return null;

        // ¡∂¡ÿº±
        yield return owner.TurretRotate();
        indicator.SetActive(true);

        yield return Util.GetWait(delay);

        indicator.SetActive(false);
        Vector3 dir = (Player.Instance.transform.position - bulletFireTf.position).normalized;
        GameObject go = ObjectPooling.Instance.SpawnObject(owner.BulletPrefab, bulletFireTf.position, Quaternion.LookRotation(dir));
        TestBullet bullet = go.GetComponent<TestBullet>();
        bullet.Init(dir, bulletSpeed);

        isPatternProcess = false;
    }

    protected override void CreatePatternCondition()
    {
        base.CreatePatternCondition();

        if (isPatternProcess && indicator.activeSelf)
        {
            indicator.SetActive(false);
            isPatternProcess = false;
        }
    }

    protected override bool Condition()
    {
        return !owner.IsHalfOverHp && owner.Controller.IsInSight();
    }
}
