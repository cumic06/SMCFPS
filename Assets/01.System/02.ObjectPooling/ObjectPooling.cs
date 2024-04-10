using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject poolObject;

    public Stack<GameObject> poolStack = new();

    public Pool(GameObject poolObject)
    {
        this.poolObject = poolObject;
    }
}

public class ObjectPooling : MonoSigleton<ObjectPooling>
{
    private Dictionary<GameObject, Pool> poolDic = new();

    public GameObject SpawnObject(GameObject poolObject, Vector3 poolPos = default, Quaternion rotation = default)
    {
        if (!poolDic.ContainsKey(poolObject))
        {
            poolDic.Add(poolObject, new Pool(poolObject));
        }

        GameObject returnObject;

        Pool pool = poolDic[poolObject];

        if (pool.poolStack.Count > 0)
        {
            returnObject = pool.poolStack.Pop();
            returnObject.SetActive(true);
        }
        else
        {
            returnObject = Instantiate(pool.poolObject);
            poolDic.Add(returnObject, pool);
        }
        returnObject.transform.SetPositionAndRotation(poolPos, rotation);

        return returnObject;
    }

    public void DestroyObject(GameObject poolObject)
    {
        poolObject.SetActive(false);
        poolDic[poolObject].poolStack.Push(poolObject);
    }
}
