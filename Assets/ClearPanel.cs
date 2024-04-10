using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Button mainButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button endButton;
    [SerializeField] private EStage stage;

    private void Start()
    {
        controller.ClearSetting();
        mainButton.onClick.AddListener(OnMainButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        endButton.onClick.AddListener(OnEndButtonClicked);
    }

    public void OnMainButtonClicked()
    {
        Time.timeScale = 1f;
        AsynceLoadSystem.LoadGameScene(EStage.MainScene);
    }


    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        AsynceLoadSystem.LoadGameScene(stage);
    }

    private void OnEndButtonClicked()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
