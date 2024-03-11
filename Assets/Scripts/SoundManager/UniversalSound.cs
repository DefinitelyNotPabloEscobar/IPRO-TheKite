using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalSound : MonoBehaviour
{
    private static UniversalSound instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
