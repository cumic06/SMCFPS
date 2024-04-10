using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HackingText : MonoBehaviour
{
    private TMP_Text text;
    private Color originColor;
    private CCTV cctv;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        originColor = text.color;
    }

    public void Init(HackingObject hackingObject)
    {
        cctv = hackingObject as CCTV;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(C_TextAlpha());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator C_TextAlpha()
    {
        float timer = 0.0f;
        text.color = originColor;

        while(timer <= 1f)
        {
            yield return null;
            
            timer += Time.deltaTime;
        }

        text.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            cctv.ReturnOrigin();
            gameObject.SetActive(false);
        }
    }
}
