using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EStage
{
    MainScene,
    LoadScene,
    Stage1Scene,
    BossStage,
}

public class LoadSceneSystem : MonoBehaviour
{
    [SerializeField] private EStage stage;

    public void LoadSceneBtn()
    {
        AsynceLoadSystem.LoadGameScene(stage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            LoadSceneBtn();
    }
}
