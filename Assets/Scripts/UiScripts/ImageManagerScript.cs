using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject imagePrefabCorrect;
    public GameObject imagePrefabNeutral;
    public GameObject imagePrefabWrong;
    public int numberOfImages = 5;
    public RectTransform canvasRectTransform;

    private void Start()
    {
        SpawnImages();
    }

    void SpawnImages()
    {
        if (imagePrefabCorrect == null || canvasRectTransform == null || numberOfImages <= 0)
        {
            Debug.LogError("Please assign the required variables in the inspector.");
            return;
        }

        float canvasWidth = canvasRectTransform.rect.width;
        float imageWidth = canvasWidth / numberOfImages;

        for (int i = 0; i < numberOfImages; i++)
        {
            GameObject imageGO = Instantiate(imagePrefabCorrect, canvasRectTransform);
            RectTransform imageRectTransform = imageGO.GetComponent<RectTransform>();

            // Position the image within the canvas
            float xPos = (i + 0.5f) * imageWidth - canvasWidth / 2;
            imageRectTransform.anchoredPosition = new Vector2(xPos, 0);

            // You may need to adjust the size of the image based on your needs
            // imageRectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
