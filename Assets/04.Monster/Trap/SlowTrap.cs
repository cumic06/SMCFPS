using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : Monster
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(-GetMonsterStat().attackStat.AttackPower);
            //���ο��Լ� ����
            //player.Slow();
        }
    }
}
