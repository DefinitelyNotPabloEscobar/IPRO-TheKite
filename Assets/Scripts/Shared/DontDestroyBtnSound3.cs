using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBtnSound3 : MonoBehaviour
{
    private static DontDestroyBtnSound3 instance;
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
