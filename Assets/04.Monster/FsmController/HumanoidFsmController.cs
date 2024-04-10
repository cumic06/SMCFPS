using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidFsmController : MonsterFsmController
{
    private bool isDamaged;

    protected override IEnumerator Start()
    {
        monster.onDamaged += OnDamaged;
        return base.Start();
    }

    protected override IEnumerator FindPlayer()
    {
        if (monster.GetMonsterCheckType().Equals(MonsterCheckType.FindType))
        {
            StartCoroutine(SearchPlayer());
        }
        else
        {
            StartCoroutine(PeekPlayer());
        }

        yield return new WaitUntil(() => sensor.IsCheckPlayer() || isDamaged);
        DebugUtil.DebugLogColor("EndFind", Color.blue);
    }

    public void OnDamaged()
    {
        isDamaged = true;
    }

    protected override IEnumerator FollowPlayer()
    {
        DebugUtil.DebugLogColor("FollowPlayer", Color.red);

        monster.SetNavMeshIsMove(false, true, true);
        monster.Anim.SetBool(isAttackAnim, false);
        monster.SetNavMeshMoveSpeed(monster.GetMonsterStat().moveSpeedStat.MoveSpeed);

        while (!CheckPlayerInAttackRange())
        {
            monster.NavMeshAgent.SetDestination(Player.Instance.transform.position);
            monster.NavMeshAgent.stoppingDistance = 3;
            yield return null;
        }

        DebugUtil.DebugLogColor("EndFollow", Color.blue);
        yield return new WaitUntil(() => CheckPlayerInAttackRange());
    }

    protected override IEnumerator AttackPlayer()
    {
        DebugUtil.DebugLogColor("AttackPlayer", Color.red);

        monster.NavMeshAgent.stoppingDistance = monster.GetMonsterStat().attackStat.AttackRange;
        monster.Anim.SetBool(isAttackAnim, true);
        monster.SetNavMeshIsMove(true, false, false);
        monster.SetNavMeshMoveSpeed(0);
        monster.NavMeshAgent.velocity = Vector3.zero;
        sensor.SensorOnOff(false);

        if (meleeAttack)
        {
            yield return StartCoroutine(MeleeAttack());
        }
        else if (farAttack)
        {
            yield return StartCoroutine(FarAttack());
        }
        else
        {

        }
        DebugUtil.DebugLogColor("EndAttack", Color.blue);
        yield return new WaitUntil(() => !CheckPlayerInAttackRange());
    }

    protected IEnumerator SearchPlayer()
    {
        int movePosCount = monster.MovePos.Count;
        WaitForFixedUpdate fixedWait = new();
        yield return null;

        while (!sensor.IsCheckPlayer())
        {
            for (int i = 0; i < movePosCount - 1 && !sensor.IsCheckPlayer(); i++)
            {
                // 시간 * 속도 = 거리
                // 시간 = 거리 / 속도
                //float mul = Vector3.Distance(monster.MovePos[i].position, monster.MovePos[i + 1].position) / monster.GetMonsterStat().moveSpeedStat.MoveSpeed;
                //mul = 1f / mul;

                Vector3 nextPos = monster.MovePos[i + 1].position;

                monster.NavMeshAgent.SetDestination(nextPos);
                monster.NavMeshAgent.stoppingDistance = 0;

                yield return null;
                yield return new WaitUntil(() => !sensor.IsCheckPlayer() && monster.NavMeshAgent.remainingDistance < 0.25f);
            }
            yield return null;
        }
        yield break;
    }
}