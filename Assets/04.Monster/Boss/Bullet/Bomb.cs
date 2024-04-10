using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Animator anim;

    [Space]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damage = 100f;

    public void SetVelocity(Vector3 velocity)
    {
        rigid.velocity = velocity;
        anim.SetTrigger("attack01");
        StartCoroutine(C_Exp());
    }

    private IEnumerator C_Exp()
    {
        yield return Util.GetWait(3f);

        Collider[] colls = Physics.OverlapSphere(transform.position, radius, playerLayer);
        if (colls.Length != 0)
            Player.Instance.TakeDamage(damage);

        AudioManager.Instance.Play("Grenade2Short", SoundType.SFX);

        gameObject.SetActive(false);
    }
}
