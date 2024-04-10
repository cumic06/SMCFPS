using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ControlModule : MonoBehaviour
{
    private static float waitTime = 6f;

    [SerializeField] private Image barImage;
    [SerializeField] private float findArange;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private GameObject hackingUI;
    [SerializeField] private GameObject hackingBarUI;
    [SerializeField] private HackingText hackingTextUI;

    public void StartHacking()
    {
        StartCoroutine(C_WaitForHacking());
    }

    private IEnumerator C_WaitForHacking()
    {
        float timer = 0f;
        hackingUI.SetActive(true);
        hackingBarUI.SetActive(true);

        while (timer <= waitTime)
        {
            yield return null;

            timer += Time.deltaTime;
            barImage.fillAmount = timer / waitTime;
        }

        barImage.fillAmount = 1f;
        HackingObject hackObj = FindHackingObject();
        hackObj.Interact();
        hackingBarUI.SetActive(false);
        hackingTextUI.Init(hackObj);

        yield return null;
        barImage.fillAmount = 0f;
    }

    private HackingObject FindHackingObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, findArange, interactableLayerMask);

        float minDis = float.MaxValue;
        int idx = -1;
        for (int i = 0; i < colliders.Length; i++)
        {
            float dis = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (dis < minDis)
            {
                minDis = dis;
                idx = i;
            }
        }

        return colliders[idx].GetComponent<HackingObject>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, findArange);
    }
}
