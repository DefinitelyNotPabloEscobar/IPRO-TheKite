using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PageSwipperSimpleUp : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float ThresholdOfMovement = 5f;
    public float easing = 0.5f;

    private Vector3 panelLocation;
    private Vector3 initPanelLocation;

    private bool isPanelDown = true;

    public PageSwipperSimple swipperHorizontal;
    public bool moving = false;


    void Start()
    {
        panelLocation = transform.position;
        initPanelLocation = transform.position;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (swipperHorizontal.moving) return;

        float difference = eventData.pressPosition.y - eventData.position.y;

        if(transform.position.y < initPanelLocation.y + Screen.height/1.5)
            transform.position = panelLocation - new Vector3(0, difference, 0);

    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (swipperHorizontal.moving) return;

        if(isPanelDown)
        {
            if (transform.position.y > initPanelLocation.y + ((Screen.height/2) - (Screen.height / 2.25)))
            {
                panelLocation += new Vector3(0, Screen.height / 2, 0);
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
                isPanelDown = false;
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
            }
        }
        else
        {
            if (transform.position.y <= initPanelLocation.y + Screen.height / 2.25)
            {
                panelLocation += new Vector3(0, -Screen.height / 2, 0);
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
                isPanelDown = true;
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
            }
        }
    }


    IEnumerator SmoothMove(Vector3 startPos,  Vector3 endPos, float seconds)
    {
        float t = 0f;
        moving = true;

        while(t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f,1f,t));
            yield return null;
        }

        moving = false;

    }


}