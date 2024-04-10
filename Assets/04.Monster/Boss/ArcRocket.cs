using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcRocket : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float upValue = 3f;
    [SerializeField] private float damage = 400f;

    [Space]
    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float range = 2.5f;
    [SerializeField] private LayerMask playerLayer;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Coroutine bulletFireCor;

    public void Init(Vector3 targetPosition)
    {
        startPosition = transform.position;
        this.targetPosition = targetPosition;

        bulletFireCor = StartCoroutine(C_BulletFire());
    }

    private IEnumerator C_BulletFire()
    {
        var fixedWait = new WaitForFixedUpdate();

        for (float t = 0; t < 1f; t += Time.fixedDeltaTime * speed)
        {
            Vector3 position =  Vector3.Lerp(startPosition, targetPosition, t);
            position.y += Mathf.Sin(t* 180 * Mathf.Deg2Rad) * upValue;
            transform.LookAt(position);
            transform.position = position; 
            yield return fixedWait;
        }

        InitExpObj();
        yield return null;
        ExpDamage();
        gameObject.SetActive(false);
    }


    private void InitExpObj()
    {
        GameObject expObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(expObj, 1.5f);
    }

    private void ExpDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, playerLayer);

        if (colliders.Length != 0)
            Player.Instance.TakeDamage(damage);

        AudioManager.Instance.Play("Grenade10Short", SoundType.SFX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletFireCor != null)
            StopCoroutine(bulletFireCor);

        InitExpObj();
        ExpDamage();
        gameObject.SetActive(false);
    }
}
