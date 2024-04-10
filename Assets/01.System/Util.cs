using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector3 GetArcVelocity(Vector3 player, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(player.x, 0, player.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = player.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }

    public static void LookAt2D(this Transform myPos, Vector3 targetPos)
    {
        Vector3 targetDistance = targetPos - myPos.position;
        float angle = (Mathf.Atan2(targetDistance.y, targetDistance.x) * Mathf.Rad2Deg) / 10;
        Debug.Log("angle" + angle);
        myPos.eulerAngles = new Vector3(0, angle, 0);
    }

    private static Dictionary<float, WaitForSeconds> waitDict = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float key)
    {
        if(!waitDict.ContainsKey(key))
        {
            waitDict.Add(key, new WaitForSeconds(key));
        }

        return waitDict[key];
    }
}
