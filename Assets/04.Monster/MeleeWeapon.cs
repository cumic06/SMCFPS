using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public Monster monster;
    private BoxCollider boxCol;

    public BoxCollider BoxCol => boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Player player);
        int attackPower = monster.GetMonsterStat().attackStat.AttackPower;
        if (player != null)
            player.TakeDamage(attackPower);
    }
}
