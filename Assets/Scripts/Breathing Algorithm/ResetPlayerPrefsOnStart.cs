using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefsOnStart : MonoBehaviour
{
    public static ResetPlayerPrefsOnStart Instance {get; private set;}

    public void deleteKeys(){
        PlayerPrefs.DeleteKey("calibrateGraphValue");
        PlayerPrefs.DeleteKey("calibrateMic");
    }
    void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            this.deleteKeys();
        }else{
            Destroy(this.gameObject);
        }
    }
}
