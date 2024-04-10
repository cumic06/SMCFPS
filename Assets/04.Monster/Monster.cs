using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamageable
{
    public delegate void OnMonsterDamaged();
    public event OnMonsterDamaged onDamaged;

    #region º¯¼ö
    [Header("Data")]
    [SerializeField] private MonsterStatData monsterStatData;
    public MonsterStatData MonsterStatData => monsterStatData;

    [Header("MovePos")]
    [SerializeField] protected List<Transform> movePos = new();
    public List<Transform> MovePos => movePos;
    public Transform StartPos { get; set; }

    [Header("CurrnetHp")]
    protected float currentHp;
    public float CurrentHp => currentHp;

    [Header("CurrnetMoveSpeed")]
    protected float CurrentMoveSpeed;

    [Header("NavMesh")]
    protected NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    [Header("Prefab")]
    [SerializeField] private GameObject monsterBullet;
    public GameObject MonsterBullet => monsterBullet;

    [Header("BulletPos")]
    [SerializeField] private Transform bulletPos;
    public Transform BulletPos => bulletPos;

    [Header("Animation")]
    protected Animator anim;
    public Animator Anim => anim;

    public static event Action OnAnyMonsterDead;

    [Header("Effect")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject deadEffect;
    #endregion

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        ResetStat();
    }

    protected void OnValidate()
    {
        ResetStat();
    }

    #region ReSetStat
    public void ResetStat()
    {
        ResetHp();
        ResetMoveSpeed();
    }

    public void ResetMoveSpeed()
    {
        CurrentMoveSpeed = monsterStatData.MonsterStatInfo.monsterStat.moveSpeedStat.MoveSpeed;
        //navMeshAgent.speed = CurrentMoveSpeed;
    }

    public void ResetHp()
    {
        currentHp = monsterStatData.MonsterStatInfo.monsterStat.hpStat.MaxHp;
    }
    #endregion

    #region GetStat
    public MonsterStat GetMonsterStat()
    {
        return monsterStatData.MonsterStatInfo.monsterStat;
    }

    public MonsterAttackType GetMonsterAttackType()
    {
        return monsterStatData.MonsterTypeInfo.monsterAttackType;
    }

    public MonsterCheckType GetMonsterCheckType()
    {
        return monsterStatData.MonsterTypeInfo.monsterCheckType;
    }
    #endregion

    #region NavMesh
    public void SetNavMeshStoppingDistance(float distanceValue)
    {
        navMeshAgent.stoppingDistance = distanceValue;
    }

    public void SetNavMeshIsMove(bool isStop, bool updatePos, bool updateRot)
    {
        navMeshAgent.isStopped = isStop;
        navMeshAgent.updatePosition = updatePos;
        navMeshAgent.updateRotation = updateRot;
    }

    public void SetNavMeshMoveSpeed(float moveSpeed)
    {
        navMeshAgent.speed = moveSpeed;
    }
    #endregion

    #region Hp
    public virtual void TakeDamage(float damage)
    {
        ChangeHp(damage);
        if(hitEffect != null)
            ObjectPooling.Instance.SpawnObject(hitEffect, transform.position + Vector3.up);

        if (damage <= 0)
        {
            onDamaged?.Invoke();
        }
        if (currentHp < 0)
        {
            Dead();
            OnAnyMonsterDead?.Invoke();
        }
    }

    protected virtual void ChangeHp(float changeValue)
    {
        ClampHp(changeValue);
        currentHp += changeValue;
    }

    protected virtual void ClampHp(float hpValue)
    {
        Mathf.Clamp(currentHp, 0, GetMonsterStat().hpStat.MaxHp);

        if (currentHp + hpValue > GetMonsterStat().hpStat.MaxHp)
        {
            currentHp = GetMonsterStat().hpStat.MaxHp;
        }
        if (currentHp + hpValue < GetMonsterStat().hpStat.MinHp)
        {
            currentHp = GetMonsterStat().hpStat.MinHp;
        }
    }
    #endregion

    protected virtual void Dead()
    {
        if(deadEffect != null)
            ObjectPooling.Instance.SpawnObject(deadEffect);

        //ObjectPooling.Instance.DestroyObject(gameObject);
        gameObject.SetActive(false);
    }

    public float GetDistanceFromPlayer() => Vector3.Distance(transform.position, Player.Instance.transform.position);

    public Vector3 GetDirectionFromPlayer() => (Player.Instance.transform.position - transform.position).normalized;

    public void MovePosition(Vector3 pos) => transform.position = pos;

    public void LookDirection(Vector3 dir) => transform.rotation = Quaternion.LookRotation(dir);
    public Quaternion GetLookRotation(Vector3 dir) => Quaternion.LookRotation(dir);
}