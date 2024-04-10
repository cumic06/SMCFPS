using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : TestBullet
{
    [SerializeField] private int reflexCount = 3;

    private Vector3 lastVelocity;

    private void Update()
    {
        lastVelocity = rb.velocity; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            //Destroy(gameObject);
            ObjectPooling.Instance.DestroyObject(gameObject);
        }

        if (reflexCount <= 0)
        {
            Destroy(gameObject);
        }
        reflexCount--;
        StopAllCoroutines();
        var mag = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(mag, 0f);
    }
}
