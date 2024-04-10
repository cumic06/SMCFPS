using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : HackingObject
{
    [SerializeField] private Transform turnObject;

    public override void Interact()
    {
        StartCoroutine(C_Open());
    }

    private IEnumerator C_Open()
    {
        Vector3 euler = turnObject.eulerAngles;
        float y = euler.y;
        float maxY = y + 90f;

        while(y <= maxY)
        {
            yield return null;

            y += Time.deltaTime * 45f;
            euler.y = y;
            turnObject.eulerAngles = euler;
        }
        euler.y = maxY;
        turnObject.eulerAngles = euler;
    }
}
