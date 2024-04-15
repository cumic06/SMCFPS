using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MonsterBT : MonoBehaviour
{
    #region º¯¼ö
    protected Monster monster;
    protected Sensor sensor;

    protected readonly LayerMask playerLayerMask = 1 << 3;
    protected bool meleeAttack;
    protected bool farAttack;

    protected int isAttackAnim = Animator.StringToHash("isAttack");
    private bool isCanAttackAnim => monster.Anim.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack") && monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.40f;
    private bool isCanFarAttackAnim => monster.Anim.GetCurrentAnimatorStateInfo(0).IsName("FarAttack") && monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.40f;
    private bool isCanSniperAttackAnim => monster.Anim.GetCurrentAnimatorStateInfo(0).IsName("Fire Rifle") && monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.40f;

    private bool findType;
    private bool peakType;
    [SerializeField] private bool isSniper = false;
    [SerializeField] private string shootSFXName = "Zapper_3p_03";

    private bool isDamaged = false;
    #endregion

    protected void Awake()
    {
        monster = GetComponent<Monster>();
        sensor = GetComponent<Sensor>();
    }

    private IEnumerator Start()
    {
        monster.onDamaged += OnDamaged;

        findType = monster.GetMonsterCheckType() == MonsterCheckType.FindType;
        peakType = monster.GetMonsterCheckType() == MonsterCheckType.PeakType;

        yield return null;
        yield return Idle();

        while (true)
        {
            if (monster.NavMeshAgent != null)
                monster.NavMeshAgent.SetDestination(Player.Instance.transform.position);

            Debug.Log($"<color=blue> Attack </color>");
            transform.rotation = Quaternion.LookRotation((Player.Instance.transform.position - transform.position).normalized);
            yield return StartCoroutine(Attack());
            Debug.Log($"<color=blue> EndAttack </color>");
            yield return null;
        }
    }


    public void OnDamaged()
    {
        isDamaged = true;
    }

    #region Idle
    public IEnumerator Idle()
    {
        if (findType)
        {
            monster.SetNavMeshMoveSpeed(monster.GetMonsterStat().moveSpeedStat.MoveSpeed);
            monster.SetNavMeshStoppingDistance(0);
            yield return null;
            StartCoroutine(FindMove());
            yield return null;
        }
        else if (peakType)
        {
            StartCoroutine(Peak());
            yield return null;
        }

        yield return new WaitUntil(() => sensor.IsCheckPlayer() || isDamaged);
    }

    private IEnumerator FindMove()
    {
        bool isFind = sensor.IsCheckPlayer();
#if UNITY_EDITOR
        Debug.Log("<color=blue> Find </color>");
#endif
        monster.MovePos.Add(monster.MovePos[0]);
        yield return null;
        while (!isFind || !isDamaged)
        {
            int monsterMoveCount = monster.MovePos.Count - 1;

            for (int i = 0; i < monsterMoveCount && !isFind; i++)
            {
                Vector3 nextPos = monster.MovePos[i + 1].position;

                //if (!monster.NavMeshAgent.pathPending)
                //{
                monster.NavMeshAgent.SetDestination(nextPos);
                //}

                yield return null;
                yield return new WaitUntil(() => isDamaged || (!isFind && monster.NavMeshAgent.remainingDistance < 0.25f));
            }
            yield return null;
        }
        yield break;
    }

    private IEnumerator Peak()
    {
        float currentAngle = 0.0f;
        int rotationDir = 1;
        while (!sensor.IsCheckPlayer())
        {
            float angle = monster.GetMonsterStat().check_Dis_Ang_Dir_Stat.CheckPeakAngle;

            if (Mathf.Abs(currentAngle) >= angle)
            {
                rotationDir *= -1;
                yield return null;
            }

            float rotationAmount = 50 * rotationDir * Time.deltaTime;
            transform.Rotate(new Vector3(0, rotationAmount, 0));
            yield return null;

            currentAngle += rotationAmount;
            yield return null;
        }
        yield break;
    }
    #endregion

    #region Follow
    public IEnumerator Follow()
    {
        monster.Anim.SetBool(isAttackAnim, false);
        sensor.SensorOnOff(true);
        monster.SetNavMeshStoppingDistance(monster.GetMonsterStat().attackStat.AttackRange);
        monster.SetNavMeshIsMove(false, true, true);
        yield return null;

        while (!sensor.IsCheckPlayer() || !CheckPlayerAttackRange())
        {
            if (!monster.NavMeshAgent.pathPending)
            {
                monster.NavMeshAgent.SetDestination(Player.Instance.transform.position);
            }
            yield return null;
        }
        yield break;
    }
    #endregion

    #region Attack
    public IEnumerator Attack()
    {
        if (findType)
        {
            monster.SetNavMeshStoppingDistance(monster.GetMonsterStat().attackStat.AttackRange);
            yield return null;
        }

        monster.Anim.SetBool(isAttackAnim, true);
        sensor.SensorOnOff(false);
        yield return null;

        switch (monster.GetMonsterAttackType())
        {
            case MonsterAttackType.MeleeAttack:
                yield return StartCoroutine(MeleeAttack());
                break;
            case MonsterAttackType.FarAttack:
                yield return StartCoroutine(FarAttack());
                break;
            case MonsterAttackType.All:
                yield return StartCoroutine(AllAttack());
                break;
        }

        yield return new WaitUntil(() => !sensor.IsCheckPlayer());
    }

    private IEnumerator MeleeAttack()
    {
        while (sensor.IsCheckPlayer())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((Player.Instance.transform.position - transform.position).normalized), 0.05f);
            monster.Anim.SetBool(isAttackAnim, true);
            for (float t = 0; t < 1; t += Time.fixedDeltaTime)
            {
                //transform.LookAt2D(Player.Instance.transform.position);
                yield return null;
            }

            if (isCanAttackAnim)
            {
                GetComponentInChildren<MeleeWeapon>().BoxCol.enabled = true;
                float attackDelay = monster.GetMonsterStat().attackStat.AttackDelay;
                yield return new WaitForSeconds(attackDelay);
                GetComponentInChildren<MeleeWeapon>().BoxCol.enabled = false;
            }
            yield return null;
        }
    }

    private IEnumerator FarAttack()
    {
        while (sensor.IsCheckPlayer() || CheckPlayerAttackRange() || isDamaged)
        {
            Debug.Log($"<color=red> IsCanAttack {sensor.IsCheckPlayer() || CheckPlayerAttackRange()} </color>");
            //Debug.Log($"<color=red>{monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1}</color>");
            //Debug.Log($"<color=red>{1}</color>");
            if (findType)
            {
                monster.SetNavMeshIsMove(true, false, true);
            }
            yield return null;

            if (isSniper)
            {
                if (isCanSniperAttackAnim)
                {
                    LookPlayer();
                    SpawnBullet();
                    float attackDelay = monster.GetMonsterStat().attackStat.AttackDelay;
                    Debug.Log("<color=red> IsCanFarAttackAnim </color>" + isCanFarAttackAnim);
                    yield return new WaitForSeconds(attackDelay);
                    LookPlayer();
                }
            }
            else if (isCanFarAttackAnim)
            {
                LookPlayer();
                SpawnBullet();
                float attackDelay = monster.GetMonsterStat().attackStat.AttackDelay;
                Debug.Log("<color=red> IsCanFarAttackAnim </color>" + isCanFarAttackAnim);
                yield return new WaitForSeconds(attackDelay);
                LookPlayer();
            }
            yield return null;
        }
        yield break;
    }

    private void LookPlayer()
    {
        Vector3 dir = (Player.Instance.transform.position - transform.position);
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir.normalized);
    }

    private IEnumerator AllAttack()
    {
        while (sensor.IsCheckPlayer())
        {
            if (isCanAttackAnim)
            {
                float attackDelay = monster.GetMonsterStat().attackStat.AttackDelay;
                yield return new WaitForSeconds(attackDelay);
            }
        }
    }

    private void SpawnBullet()
    {
        float attackCount = monster.GetMonsterStat().attackStat.BulletCount;
        float currentAngle = monster.GetMonsterStat().attackStat.BulletAngle / attackCount;

        if (attackCount > 1)
        {
            for (int i = 0; i < attackCount; i++)
            {
                GameObject bullet = ObjectPooling.Instance.SpawnObject(monster.MonsterBullet, monster.BulletPos.position, Quaternion.Euler(transform.eulerAngles + new Vector3(0, currentAngle * i)));
                bullet.GetComponent<TestBullet>().Init(bullet.transform.forward);
            }
        }
        else
        {
            BulletCreate();
        }

        AudioManager.Instance.Play(shootSFXName, SoundType.SFX);
    }

    private void BulletCreate()
    {
        GameObject bullet = ObjectPooling.Instance.SpawnObject(monster.MonsterBullet, monster.BulletPos.position, Quaternion.LookRotation((Player.Instance.transform.position - monster.BulletPos.position).normalized));
        bullet.GetComponent<TestBullet>().Init(bullet.transform.forward);
    }
    #endregion

    #region ViewGizmos
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, monster.GetMonsterStat().check_Dis_Ang_Dir_Stat.CheckAttackRange);
        }
    }

    protected bool CheckPlayerAttackRange()
    {
        Collider sphereRay = Physics.OverlapSphere(transform.position, monster.GetMonsterStat().check_Dis_Ang_Dir_Stat.CheckAttackRange, playerLayerMask).FirstOrDefault();

        return sphereRay;
    }
    #endregion
}
