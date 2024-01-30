using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IconView : MonoBehaviour
{
    public Image image;
    public float imageApperanceSpeed = 1.0f;

    private bool appearing = false;
    
    public void Appear()
    {
        if(appearing) return;

        image.enabled = true;
        Color color = image.color;
        color.a = 1;
        image.color = color;

        appearing = true;
    }

    public void Update()
    {
        if (!appearing) return;

        Color color = image.color;
        color.a -= imageApperanceSpeed*Time.deltaTime;
        image.color = color;

        if( image.color.a <= 0)
        {
            image.enabled = false;
            appearing = false;
        }

    }
}
