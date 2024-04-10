#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ForceGrid : MonoBehaviour
{
    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        pos.z = (int)pos.z;
        transform.position = pos;
    }
}
#endif