using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HackingData", menuName = "Data/Hacking")]
public class HackingDataSO : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    private HackingObject owner;

    public string Name => name;
    public Sprite Icon => icon;
    public HackingObject Owner => owner;

    public HackingDataSO Clone(HackingObject owner)
    {
        this.owner = owner;
        HackingDataSO clone = Instantiate(this);

        return clone;
    }
}
