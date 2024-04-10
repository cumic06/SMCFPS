using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFsmController : MonoBehaviour
{
    protected Monster monster;
    protected Sensor sensor;

    protected readonly LayerMask playerLayerMask = 1 << 3;
    protected bool meleeAttack;
    protected bool farAttack;

    protected int isAttackAnim = Animator.StringToHash("isAttack");

    protected virtual void Awake()
    {
        monster = GetComponent<Monster>();
        sensor = GetComponent<Sensor>();
    }

    protected virtual IEnumerator Start()
    {
        meleeAttack = monster.GetMonsterAttackType().Equals(MonsterAttackType.MeleeAttack);
        farAttack = monster.GetMonsterAttackType().Equals(MonsterAttackType.FarAttack);

        if (monster.GetMonsterCheckType() == MonsterCheckType.FindType)
        {
            monster.MovePos.Add(monster.MovePos[0]);
        }
        yield return null;

        DebugUtil.DebugLogColor("FindPlayer", Color.red);
        yield return FindPlayer();

        while (true)
        {
            yield return FollowPlayer();
            yield return AttackPlayer();
        }
    }

    protected abstract IEnumerator FindPlayer();
    protected abstract IEnumerator FollowPlayer();
    protected abstract IEnumerator AttackPlayer();

    #region Attack
    protected virtual IEnumerator MeleeAttack()
    {
        while (sensor.IsInSight(Player.Instance.gameObject))
        {
            DebugUtil.DebugLogColor("MeleeAttack", Color.red);
            Vector3 attackSize = new(2, 2, monster.GetMonsterStat().attackStat.AttackRange);
            Collider[] attackCheck = Physics.OverlapBox(transform.position, attackSize, transform.rotation, playerLayerMask);

            foreach (var playerCheck in attackCheck)
            {
                float attackPower = monster.GetMonsterStat().attackStat.AttackPower;

                float normalizedTime = monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                bool isCanAttack = monster.Anim.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack") && normalizedTime >= 0.45f;

                if (isCanAttack)
                {
                    yield return new WaitForSeconds(monster.GetMonsterStat().attackStat.AttackDelay);
                    monster.Anim.SetBool(isAttackAnim, true);
                    playerCheck.GetComponent<Player>().TakeDamage((float)attackPower);
                }
                yield return null;
            }
        }
    }

    protected virtual IEnumerator FarAttack()
    {
        DebugUtil.DebugLogColor("FarAttack", Color.red);
        while (sensor.IsInSight(Player.Instance.gameObject))
        {
            //transform.LookAt2D(Player.Instance.transform.position);
            //if (monster.GetMonsterShootType().Equals(MonsterShootType.ShotGun))
            //{
            //    float normalizedTime = monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            //    bool isCanAttack = monster.Anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot") && normalizedTime >= 0.45f;

            //    yield return new WaitForSeconds(monster.GetMonsterStat().attackStat.AttackDelay);
            //    monster.Anim.SetBool(isAttackAnim, true);
            //    SpawnBullet((int)monster.GetMonsterStat().attackStat.AttackCount, 45);
            //    Debug.Log("Shot");
            //    yield return null;
            //}
            //else
            //{
            //    SpawnBullet(1, 0);
            //    monster.Anim.SetBool(isAttackAnim, true);
            //    yield return new WaitForSeconds(monster.GetMonsterStat().attackStat.AttackDelay);
            //}
            yield return null;
        }
    }

    public void SpawnBullet(int count, int angle)
    {
        float currentAngle = (float)count / (float)angle;
        for (int i = 0; i < count; i++)
        {
            ObjectPooling.Instance.SpawnObject(monster.MonsterBullet, monster.BulletPos.position, Quaternion.Euler(new(0, i * currentAngle, 0)));
            monster.MonsterBullet.GetComponent<Bullet>().BulletDamage = monster.GetMonsterStat().attackStat.AttackPower;
        }
    }
    #endregion

    #region Check
    protected bool CheckPlayerInAttackRange()
    {
        Vector3 checkAttackRange = new(2, 2, monster.GetMonsterStat().attackStat.AttackRange);
        Collider[] checkBox = Physics.OverlapBox(transform.position, checkAttackRange, transform.rotation, playerLayerMask);
        foreach (var player in checkBox)
        {
            return player;
        }
        return false;
    }
    #endregion

    #region ViewRay
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
         //   ViewPlayerAttackRay();
        }
    }

    //protected virtual void ViewPlayerAttackRay()
    //{
    //    Vector3 checkAttackRange = new(2, 2, monster.GetMonsterStat().attackStat.AttackRange);
    //    Collider[] checkPlayerRay = Physics.OverlapBox(transform.position, checkAttackRange, transform.rotation, playerLayerMask);

    //    foreach (var checkPlayer in checkPlayerRay)
    //    {
    //        if (checkPlayer)
    //        {
    //            Gizmos.color = Color.yellow;
    //            Gizmos.DrawWireCube(transform.position, checkAttackRange);
    //            Gizmos.DrawRay(transform.position, transform.forward * monster.GetMonsterStat().attackStat.AttackRange);
    //        }
    //    }
    //}
    #endregion

    protected virtual IEnumerator PeekPlayer()
    {
        while (!sensor.IsCheckPlayer())
        {
            transform.eulerAngles = new(transform.eulerAngles.x, transform.eulerAngles.y + 1, transform.eulerAngles.z);
            yield return null;
        }
        yield break;
    }
}