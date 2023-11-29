using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBtnSound1 : MonoBehaviour
{
    private static DontDestroyBtnSound1 instance;
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
