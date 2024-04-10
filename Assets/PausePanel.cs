using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button endButton;
    [SerializeField] private EStage stage;

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        endButton.onClick.AddListener(OnEndButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        controller.PauseSetting();
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
