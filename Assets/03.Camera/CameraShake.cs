using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakeDirection
{
    Horizon,
    Verti,
    All
}

public class CameraShake : MonoSigleton<CameraShake>
{
    private Vector3 StartPos;

    public void Shake(ShakeDirection shakeType, float shakeValue, float shakeTime)
    {
        float t = 0;

        StartPos = transform.position;

        StartCoroutine(ShakeTime());

        IEnumerator ShakeTime()
        {
            while (t <= shakeTime)
            {
                t += Time.deltaTime;
                if (ShakeDirection.Verti == shakeType)
                {
                    transform.localPosition = StartPos + new Vector3(0, Random.Range(-1, 1) * shakeValue);
                }
                else if (ShakeDirection.Horizon == shakeType)
                {
                    transform.localPosition = StartPos + new Vector3(Random.Range(-1, 1) * shakeValue, 0);
                }
                else
                {
                    transform.localPosition = StartPos + Random.insideUnitSphere * shakeValue;
                }
                yield return null;
            }
        }
    }
}
