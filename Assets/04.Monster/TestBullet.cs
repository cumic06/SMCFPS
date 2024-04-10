using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float timeLimit = 5f;
    [SerializeField] private float speed = 15f;
    protected Rigidbody rb;
    private Vector3 dir;
    protected float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Init(Vector3 dir, float speed = -1f)
    {
        this.dir = dir;

        if (speed > 0f)
            this.speed = speed;

        rb.velocity = dir * this.speed;
        //Destroy(gameObject, timeLimit);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            ObjectPooling.Instance.DestroyObject(gameObject);
        }
        //Destroy(gameObject);
    }
}
