using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Device3 : Pattern
{
    [SerializeField] private float limitDistance = 2f;
    [SerializeField] private float angle = 45f;
    [SerializeField] private int bombCount = 4;
    [SerializeField] private Bomb bombPrefab;
    [SerializeField] private float bombValue = 3f;

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        Vector3 pos = transform.position;
        pos.y += 2f;

        Vector3 bombPos = new Vector3(bombValue, 0, 0);
        Vector3 euler = new Vector3(0, 90, 0);
        Instantiate(bombPrefab, pos, Quaternion.Euler(euler)).SetVelocity(Util.GetArcVelocity(pos, pos + bombPos, angle));
        Instantiate(bombPrefab, pos, Quaternion.Euler(-euler)).SetVelocity(Util.GetArcVelocity(pos, pos + -bombPos, angle));

        bombPos.x = 0f;
        bombPos.z = bombValue;
        euler.y += 90;
        Instantiate(bombPrefab, pos, Quaternion.identity).SetVelocity(Util.GetArcVelocity(pos, pos + bombPos, angle));
        Instantiate(bombPrefab, pos, Quaternion.Euler(-euler)).SetVelocity(Util.GetArcVelocity(pos, pos + -bombPos, angle));

        yield return Util.GetWait(3f);
    }

    protected override bool Condition()
    {
        return owner.IsHalfOverHp && owner.GetDistanceFromPlayer() < limitDistance;
    }
}
