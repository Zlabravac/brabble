using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the Camera
    public GameObject tilemap; // Reference to the Tilemap GameObject
    public float minZoom = 5f; // Minimum camera orthographic size
    public float maxZoom = 15f; // Maximum camera orthographic size
    public float cameraSpeed = 5f; // Speed of camera movement
    public float zoomSpeed = 5f; // Speed of zooming with the scroll wheel
    private Bounds currentBounds; // Bounds of the unlocked Tilemap
    private Vector2 lastTouchPosition; // Last touch position
    private Vector3 lastMousePosition; // Last mouse position
    private List<Bounds> unlockedAreas = new List<Bounds>(); // List of unlocked areas

    void Start()
    {
        // Initialize the first unlocked area
        Bounds initialBounds = tilemap.GetComponent<Renderer>().bounds;
        currentBounds = initialBounds;
        unlockedAreas.Add(initialBounds);
    }

    void Update()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    void HandleCameraMovement()
    {
        // Touch controls for movement
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 delta = mainCamera.ScreenToWorldPoint(touch.position) - mainCamera.ScreenToWorldPoint(lastTouchPosition);
                Vector3 newPosition = mainCamera.transform.position - delta;

                // Clamp camera position within bounds
                newPosition.x = Mathf.Clamp(newPosition.x, currentBounds.min.x, currentBounds.max.x);
                newPosition.y = Mathf.Clamp(newPosition.y, currentBounds.min.y, currentBounds.max.y);
                newPosition.z = mainCamera.transform.position.z;

                mainCamera.transform.position = newPosition;
            }
            lastTouchPosition = touch.position;
        }

        // Mouse controls for movement
        if (Input.GetMouseButton(0)) // Left mouse button drag
        {
            Vector3 mouseDelta = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.ScreenToWorldPoint(lastMousePosition);
            Vector3 newPosition = mainCamera.transform.position - mouseDelta;

            // Clamp camera position within bounds
            newPosition.x = Mathf.Clamp(newPosition.x, currentBounds.min.x, currentBounds.max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, currentBounds.min.y, currentBounds.max.y);
            newPosition.z = mainCamera.transform.position.z;

            mainCamera.transform.position = newPosition;
        }
        lastMousePosition = Input.mousePosition;
    }

    void HandleCameraZoom()
    {
        // Touch controls for zoom
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate pinch distance
            float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);
            float previousPinchDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);

            // Zoom camera
            float zoomDelta = previousPinchDistance - currentPinchDistance;
            float newSize = Mathf.Clamp(mainCamera.orthographicSize + zoomDelta * Time.deltaTime, minZoom, maxZoom);
            mainCamera.orthographicSize = newSize;
        }

        // Mouse controls for zoom
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            float newSize = Mathf.Clamp(mainCamera.orthographicSize - scrollDelta * zoomSpeed, minZoom, maxZoom);
            mainCamera.orthographicSize = newSize;
        }
    }

    public void UnlockNewArea(Bounds newBounds)
    {
        // Add the new bounds to the unlocked areas
        unlockedAreas.Add(newBounds);

        // Update the current bounds to encompass all unlocked areas
        UpdateCameraBounds();
    }

    private void UpdateCameraBounds()
    {
        // Calculate the combined bounds of all unlocked areas
        currentBounds = unlockedAreas[0];
        foreach (Bounds bounds in unlockedAreas)
        {
            currentBounds.Encapsulate(bounds);
        }
    }
}
