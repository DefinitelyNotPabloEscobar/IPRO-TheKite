using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwipper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;

    public List<GameObject> pages;

    private Vector3 panelLocation;
    private int Width;
    void Start()
    {
        if (pages == null || pages.Count < 1) {
            Debug.Log("Error: Pages must bot be null nor less than 1.");
            return;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.position = new Vector3(
                pages[0].transform.position.x + Screen.width * i,
                pages[0].transform.position.y, 
                0f);
        }

        panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector3 (difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (pages == null || pages.Count < 1)
        {
            Debug.Log("Error: Pages must bot be null nor less than 1.");
            return;
        }

        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold) 
        {
            Vector3 newLocation = panelLocation;
            if (percentage > 0 && newLocation.x - Screen.width > -Screen.width * (pages.Count - 1))
            {
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if(percentage < 0 && newLocation.x + Screen.width <= Screen.width/2)
            {
                newLocation += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }


    IEnumerator SmoothMove(Vector3 startPos,  Vector3 endPos, float seconds)
    {
        float t = 0f;

        while(t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f,1f,t));
            yield return null;
        }
    }
}
