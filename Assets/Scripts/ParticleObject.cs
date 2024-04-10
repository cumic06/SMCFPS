using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    public Action<ParticleObject> onDiable;
    ParticleSystem _particleSystem;
    [SerializeField] float activeTime;
    Coroutine disableCor = null;
    WaitForSeconds ws;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        ws = new WaitForSeconds(activeTime);
    }
    IEnumerator C_Disable()
    {
        yield return ws;
        disableCor = null;
        onDiable?.Invoke(this);
    }
    private void OnEnable()
    {
        if (disableCor == null)
            StartCoroutine(C_Disable());
        _particleSystem.Play();
    }
}