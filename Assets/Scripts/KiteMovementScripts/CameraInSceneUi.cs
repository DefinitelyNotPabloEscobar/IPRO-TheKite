using UnityEngine;

public class CameraInSceneUi : MonoBehaviour
{
    public GameObject itemPrefab;
    public int numberOfItems = 5;
    public float distanceFromCamera = 5f;
    public float itemSpacing = 0.6f;
    public float yOffset = -0.6f;

    public float currentY;

    private Transform cameraTransform;
    private GameObject[] items;

    public int numberOfItemsInUse = 5;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        numberOfItemsInUse = numberOfItems;
        currentY = yOffset;
        SpawnItems();
    }

    private void LateUpdate()
    {
        UpdateItemPositions();
    }

    void SpawnItems()
    {
        if (itemPrefab == null || cameraTransform == null || numberOfItems <= 0)
        {
            Debug.LogError("Please assign the required variables in the inspector.");
            return;
        }

        items = new GameObject[numberOfItems];

        float totalWidth = (numberOfItems - 1) * itemSpacing;

        for (int i = 0; i < numberOfItems; i++)
        {
            // Calculate the position of the item based on its index and spacing.
            float xPos = -totalWidth / 2 + i * itemSpacing;
            float yPos = yOffset; // Apply the vertical offset here.

            Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera + cameraTransform.right * xPos + cameraTransform.up * yPos;
            Quaternion spawnRotation = cameraTransform.rotation;

            GameObject itemGO = Instantiate(itemPrefab, spawnPosition, spawnRotation);
            items[i] = itemGO;
        }
    }

    void UpdateItemPositions()
    {
        if (items == null || items.Length == 0)
            return;

        for (int i = 0; i < items.Length; i++)
        {
            float xPos = -((items.Length - 1) * itemSpacing) / 2 + i * itemSpacing;
            float yPos = yOffset;

            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera + cameraTransform.right * xPos + cameraTransform.up * yPos;

            items[i].transform.position = newPosition;
            items[i].transform.rotation = cameraTransform.rotation;
        }
    }
}
