using System;

[Serializable]
public struct HpStat
{
    public int MaxHp;
    public int MinHp;
}

[Serializable]
public struct AttackStat
{
    public int AttackPower;
    public float AttackRange;
    public float AttackDelay;
    public int BulletCount;
    public int BulletAngle;
}

[Serializable]
public struct MoveSpeedStat
{
    public float MoveSpeed;
}

[Serializable]
public struct GuardPowerStat
{
    public float guardPower;
}

[Serializable]
public struct Check_Dis_Ang_Dir_Stat
{
    public float CheckAttackRange;
    public float CheckPeakAngle;
}