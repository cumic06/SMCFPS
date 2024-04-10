using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private const string doorOpenParameter = "character_nearby";

    [SerializeField] private Animator[] doorAnims;
    [SerializeField] private Transform[] enemyParents;
    
    private int index = 0;
    private int[] enemyCounts;

    private void Start()
    {
        Monster.OnAnyMonsterDead += AnyMonsterDead;

        enemyCounts = new int[enemyParents.Length];
        for (int i = 0; i < enemyParents.Length; i++)
        {
            enemyCounts[i] = enemyParents[i].childCount;
        }
    }

    private void AnyMonsterDead()
    {
        if (index >= enemyCounts.Length)
            return;

        if(--enemyCounts[index] == 0)
        {
            doorAnims[index++].SetBool(doorOpenParameter, true);
        }
    }
}
