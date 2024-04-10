using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MonsterStatData
{
    [SerializeField] private MonsterTypeInfo monsterTypeInfo;
    public MonsterTypeInfo MonsterTypeInfo => monsterTypeInfo;

    [SerializeField] private MonsterStatInfo monsterStatInfo;
    public MonsterStatInfo MonsterStatInfo => monsterStatInfo;
}

[Serializable]
public class MonsterStatInfo
{
    public MonsterStat monsterStat;
}

[Serializable]
public struct MonsterStat
{
    public HpStat hpStat;
    public AttackStat attackStat;
    public MoveSpeedStat moveSpeedStat;
    public GuardPowerStat guardPowerStat;
    public Check_Dis_Ang_Dir_Stat check_Dis_Ang_Dir_Stat;
}

[Serializable]
public class MonsterTypeInfo
{
    public MonsterType monsterType;
    public MonsterAttackType monsterAttackType;
    public MonsterCheckType monsterCheckType;
}

public enum MonsterType
{
    NormalMonster,
    SpecialMonster,
    Boss
}

public enum MonsterCheckType
{
    PeakType,
    FindType
}

public enum MonsterAttackType
{
    MeleeAttack,
    FarAttack,
    All
}