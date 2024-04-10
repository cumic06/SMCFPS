using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using VolumetricLines;

public class Test : MonoBehaviour
{
    [SerializeField] private Monster m;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            m.TakeDamage(10f);
    }
}
