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
            //슬로우함수 실행
            //player.Slow();
        }
    }
}
