using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public delegate void OnBossDied();
    public event OnBossDied onBossDied;

    [Space, Space]
    [Header("--- Boss ---")]
    [SerializeField] protected float coolTime;
    [SerializeField] protected Transform turret;
    [SerializeField] protected Transform turretMount;
    [SerializeField] protected Transform turretHead;
    [SerializeField] protected GameObject bulletPrefab;

    protected Pattern[] patterns;
    protected BossFsmController controller;
    protected TimeEndTrigger coolTimeTrigger;
    protected bool patternAble = true;
    protected bool isP = false;
    protected bool turretHeadRotateMode = false;

    public float CoolTime => coolTime;
    public bool PatternAble => patternAble;
    public GameObject BulletPrefab => bulletPrefab;
    public BossFsmController Controller => controller;
    public virtual bool IsHalfOverHp => !isP && MonsterStatData.MonsterStatInfo.monsterStat.hpStat.MaxHp * 0.5f < currentHp;
    public Transform Turret => turret;
    public Transform TurretHead => turretHead;
    public Transform TurretMount => turretMount;

    [SerializeField] MeshRenderer[] mesh;
    Coroutine changeCor;
    [SerializeField] GameObject clearPanel;

    protected override void Awake()
    {
        anim = GetComponent<Animator>();
        patterns = GetComponentsInChildren<Pattern>(false);
        Debug.Log(patterns.Length);
        controller = GetComponent<BossFsmController>();
    }

    protected override void Start()
    {
        base.Start();

        coolTimeTrigger = new TimeEndTrigger(coolTime);
        coolTimeTrigger.onEnd += () =>
        {
            coolTimeTrigger.timer.CurrentTime = 0f;
            patternAble = true;
            ResetPatternCondition();
            Debug.Log("CoolTimeEnd");
        };

        controller.anyPatternEnded += OnAnyPatternEnded;
    }

    // Test
    private void Update()
    {
        if (turretHeadRotateMode)
        {
            turretHead.rotation = Quaternion.Lerp(turretHead.rotation, GetLookRotation((Player.Instance.transform.position - turretHead.position).normalized), 0.05f);
        }
    }

    public void TryPatternStart(Pattern pattern)
    {
        if (controller.TryPattern(pattern))
        {
            patternAble = false;
            ResetPatternCondition();
        }

        if (pattern is Pattern_Device4)
        {
            Debug.Log("A");
            StartTimer();
        }
    }

    public void ResetPatternCondition()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            if (patterns[i].Priority == PatternPriority.Low)
                patterns[i].ResetPatternCondition();
        }
    }

    protected virtual void OnAnyPatternEnded(Pattern pattern)
    {
        //pattern.ResetPatternCondition();
        if (pattern.Priority == PatternPriority.Low)
        {
            StartTimer();
        }

        turretHeadRotateMode = false;
    }

    public void StartTimer()
    {
        coolTimeTrigger.StartTimer();
    }

    public void SetP()
    {
        Debug.Log("p");
        isP = true;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (changeCor != null)
        {
            StopCoroutine(changeCor);
        }
        changeCor = StartCoroutine(ChangeColor());

        IEnumerator ChangeColor()
        {
            for (int i = 0; i < 2; i++)
            {
                mesh.ToList().ForEach((a) => a.material.color = new Color(1, 0, 0, 1));
                yield return new WaitForSeconds(0.2f);
                mesh.ToList().ForEach((a) => a.material.color = new Color(1, 1, 1, 1));
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
        }

        if (currentHp <= 0f)
        {
            clearPanel.SetActive(true);
            onBossDied?.Invoke();
        }
    }

    public IEnumerator TurretRotate()
    {
        yield return TurretRotate(turretMount);
        yield return TurretRotate(turretHead, true);

        turretHeadRotateMode = true;
    }

    public IEnumerator TurretRotate(Transform targetTf, bool isX = false, float speed = 5f)
    {
        float timer = 0f;
        Quaternion startRot = targetTf.rotation;

        Player player = Player.Instance;
        Quaternion targetRot = new Quaternion();
        while (timer <= 1f)
        {
            targetRot = GetLookRotation((player.transform.position - targetTf.position).normalized);
            if (isX)
                targetRot.x = startRot.x;

            timer += Time.deltaTime * speed;
            targetTf.rotation = Quaternion.Lerp(startRot, targetRot, timer);

            yield return null;
        }
    }

    public IEnumerator TurretRotate(Transform targetTf, Quaternion targetRot, float speed = 10f)
    {
        float timer = 0f;
        Quaternion startRot = targetTf.localRotation;

        while (timer <= 1f)
        {
            timer += Time.deltaTime * speed;
            targetTf.localRotation = Quaternion.Lerp(startRot, targetRot, timer);

            yield return null;
        }
    }
}