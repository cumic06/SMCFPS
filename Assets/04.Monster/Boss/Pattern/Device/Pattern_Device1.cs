using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device1 : Pattern
{
    [SerializeField] private GameObject bounceBulletPrefab;
    [SerializeField] private Transform[] bulletInitTfs;
    [SerializeField] private int fireCount = 15;
    [SerializeField] private float damage = 20;

    [SerializeField] private float patternDelay = 0.05f;
    private bool patternAble;
    private Coroutine patternDelayCor;

    protected override void Start()
    {
        base.Start();

        owner.Controller.anyPatternEnded += AnyPatternEnded;
    }

    protected override bool Condition()
    {
        if (patternDelayCor == null)
        {
            patternDelayCor = StartCoroutine(C_PatternDelay());
            return false;
        }
        return patternAble;
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        yield return owner.TurretRotate();

        for (int i = 0; i < fireCount; i++)
        {
            foreach (Transform tf in bulletInitTfs)
            {
                GameObject go = ObjectPooling.Instance.SpawnObject(bounceBulletPrefab, tf.position, Quaternion.identity);
                BounceBullet bullet = go.GetComponent<BounceBullet>();
                bullet.SetDamage(damage);
                bullet.Init((Player.Instance.transform.position - tf.position).normalized);
            }
            
            yield return Util.GetWait(1f);
        }
    }

    private void AnyPatternEnded(Pattern pattern)
    {
        if (patternDelayCor != null)
            StopCoroutine(patternDelayCor);

        patternDelayCor = null;
        patternAble = false;
    }

    private IEnumerator C_PatternDelay()
    {
        yield return Util.GetWait(patternDelay);
        patternAble = owner.IsHalfOverHp && owner.Controller.IsInSight();
        patternDelayCor = null;
    }
}