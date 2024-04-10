using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device4 : Pattern_Special
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject electricAuraPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject electricFieldPrefab;

    [Space]
    [SerializeField] private GameObject[] separationObj;

    protected override void Start()
    {
        base.Start();

        owner.onDamaged += OnBossDamaged;
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        GameObject electricAuraObj = Instantiate(electricAuraPrefab, owner.transform.position + Vector3.up, Quaternion.identity);
        yield return Util.GetWait(1.5f);

        electricAuraObj.SetActive(false);
        GameObject explosionObj = Instantiate(explosionPrefab, owner.transform.position + Vector3.up, Quaternion.identity);
        yield return Util.GetWait(0.7f);

        explosionObj.SetActive(false);

        yield return Util.GetWait(1f);

        foreach (GameObject go in separationObj)
        {
            go.AddComponent<Rigidbody>();
            go.AddComponent<BoxCollider>();
            go.transform.SetParent(null);
        }
        yield return Util.GetWait(0.3f);

        GameObject electricFieldObj = Instantiate(electricFieldPrefab, owner.transform.position + Vector3.up, Quaternion.identity);
        yield return Util.GetWait(1.5f);
        
        Player.Instance.TakeDamage(damage);

        yield return Util.GetWait(1.0f);
        electricFieldObj.SetActive(false);

        owner.SetP();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        patternAble = true;
    //    }
    //}

    private void OnBossDamaged()
    {
        if(!owner.IsHalfOverHp)
        {
            patternAble = true;
        }
    }
}
