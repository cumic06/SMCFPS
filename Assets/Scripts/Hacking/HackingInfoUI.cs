using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackingInfoUI : MonoBehaviour
{
    private HackingDataSO data;

    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button interactButton;

    public void Initialize(HackingDataSO data)
    {
        this.data = data;

        SetUI();
    }

    private void SetUI()
    {
        iconImage.sprite = data.Icon;
        nameText.text = data.Name;
        interactButton.onClick.AddListener(data.Owner.Interact);
    }
}
