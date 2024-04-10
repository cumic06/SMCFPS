using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device9 : Pattern_Special
{
    [SerializeField] private GameObject explosionPrefab;

    protected override void Start()
    {
        base.Start();

        owner.onBossDied += OnBossDied;
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        GameObject go = Instantiate(explosionPrefab, owner.transform.position + Vector3.up, Quaternion.identity);
        go.SetActive(true);
        yield return Util.GetWait(0.35f);
        Destroy(go, 3f);
        owner.gameObject.SetActive(false);
    }

    private void OnBossDied()
    {
        patternAble = true;
    }
}
