using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsynceLoadSystem : MonoBehaviour
{
    [SerializeField] private float loadTime;

    [SerializeField] private Slider loadingHandler;

    [SerializeField] private Text loadingText;

    [SerializeField] private LoadTextScriptable loadTextScriptable;

    private static int nextScene;

    public static void LoadGameScene(EStage stageEnum)
    {
        nextScene = (int)stageEnum;
        SceneManager.LoadScene((int)EStage.LoadScene);
    }

    private void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        float currentTime = 0;
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;
        loadingText.text = $"{loadTextScriptable.loadTextDatas.GetRandomText()}";
        yield return null;

        while (!async.isDone)
        {
            currentTime += Time.deltaTime;
            loadingHandler.value = currentTime / loadTime;

            if (currentTime > loadTime && async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
