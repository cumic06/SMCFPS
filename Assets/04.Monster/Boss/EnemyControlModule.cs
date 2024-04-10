using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlModule : MonoBehaviour, IDamageable
{
    public delegate void OnAnyECMDamaged();
    public delegate void OnAllECMDied();
    public static event OnAnyECMDamaged anyECMDamaged;
    public static event OnAllECMDied allECMDied;

    private static int moduleCount;

    [SerializeField] private GameObject hacker; // ÇØÄ¿ Áö¸Á»ý

    private float hp = 10f;

    private void Awake()
    {
        moduleCount++;
    }

    public void TakeDamage(float damage)
    {
        anyECMDamaged?.Invoke();
        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            Die();
        }
    }

    private void Die()
    {
        moduleCount--;

        if(moduleCount <= 0)
        {
            allECMDied?.Invoke();
        }
    }
}
