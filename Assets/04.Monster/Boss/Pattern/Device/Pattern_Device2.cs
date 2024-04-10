using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device2 : Pattern
{
    [SerializeField] private Transform bulletFireTf;
    [SerializeField] private Transform turretBase;
    [SerializeField] private float damage;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float patternDelay = 0.1f;
    private bool patternAble;
    private Coroutine patternDelayCor;

    protected override void Start()
    {
        base.Start();

        owner.Controller.anyPatternEnded += AnyPatternEnded;
    }

    protected override bool Condition()
    {
        if(patternDelayCor == null)
        {
            patternDelayCor = StartCoroutine(C_PatternDelay());
            return false;
        }
        else
        {
            return patternAble;
        }
    }

    private IEnumerator C_PatternDelay()
    {
        yield return Util.GetWait(patternDelay);
        patternAble = owner.IsHalfOverHp;
        patternDelayCor = null;
    }

    private void AnyPatternEnded(Pattern pattern)
    {
        if (patternDelayCor != null)
            StopCoroutine(patternDelayCor);

        patternDelayCor = null;
        patternAble = false;
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        Vector3 angle = Vector3.zero;

        for (float i = 0; i < 360; i += 10)
        {
            angle.y = i;
            Quaternion qu = Quaternion.Euler(angle);
            yield return owner.TurretRotate(turretBase, qu, speed: rotateSpeed);

            GameObject bulletObj = ObjectPooling.Instance.SpawnObject(owner.BulletPrefab, bulletFireTf.position, qu);
            TestBullet bullet = bulletObj.GetComponent<TestBullet>();
            bullet.SetDamage(damage);
            bullet.Init(bullet.transform.forward);
        }
    }

    protected override void OnConditionEnd()
    {
        base.OnConditionEnd();
    }
}