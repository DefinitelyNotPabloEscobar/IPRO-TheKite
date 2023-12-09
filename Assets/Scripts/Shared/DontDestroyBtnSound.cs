using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBtnSound: MonoBehaviour
{
    private static Dictionary<int,DontDestroyBtnSound> instances = new Dictionary<int, DontDestroyBtnSound>();
    public int instanceId;
    private void Awake()
    {
        if (!instances.ContainsKey(instanceId))
        {
            instances.Add(instanceId, this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instances[instanceId].gameObject);
            instances[instanceId] = this;
            DontDestroyOnLoad(gameObject);
        }
    }


}
