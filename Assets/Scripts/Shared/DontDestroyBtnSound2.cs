using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBtnSound2 : MonoBehaviour
{
    private static DontDestroyBtnSound2 instance;
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
