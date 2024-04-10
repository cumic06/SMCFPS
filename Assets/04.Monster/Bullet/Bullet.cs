using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage { get; set; }

    [SerializeField] private float bulletMoveSpeed;

    private void OnEnable()
    {
        StartCoroutine(BulletMove());
        StartCoroutine(DestroyBullet());
        //Debug.Log("<color=blue> SpawnBullet </color>");
    }

    private IEnumerator BulletMove()
    {
        while (true)
        {
            Vector3 moveVec = bulletMoveSpeed * Time.fixedDeltaTime * Vector3.forward;
            transform.Translate(moveVec);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
#if UNITY_EDITOR
            Debug.Log("Hit");
#endif
            bool isPlayer = other.CompareTag("Player");
            if (isPlayer)
            {
                other.GetComponent<Player>().TakeDamage(BulletDamage);
                ObjectPooling.Instance.DestroyObject(gameObject);
            }
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5f);
        ObjectPooling.Instance.DestroyObject(gameObject);
    }
}
