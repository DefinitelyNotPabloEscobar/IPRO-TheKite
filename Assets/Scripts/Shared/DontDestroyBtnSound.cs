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
            StartCoroutine(LetThemFinishTheirSound(2, instances[instanceId].gameObject));
            instances[instanceId] = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    IEnumerator LetThemFinishTheirSound(float seconds, GameObject toDestroy)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(toDestroy);
    }


}
