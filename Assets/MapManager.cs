using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the Camera
    public Tilemap initialTilemap; // The starting visible Tilemap
    public float minZoom = 5f; // Minimum camera orthographic size
    private float maxZoom; // Maximum camera orthographic size, dynamically updated
    public float cameraSpeed = 5f; // Speed of camera movement
    public float zoomSpeed = 5f; // Speed of zooming with the scroll wheel
    public float dragSensitivity = 1.0f; // Multiplier for drag sensitivity
    public List<Tilemap> lockedTilemaps; // List of Tilemaps representing locked areas

    private Bounds currentBounds; // Combined bounds of unlocked Tilemaps
    private List<Tilemap> unlockedTilemaps = new List<Tilemap>(); // List of unlocked Tilemaps
    private Vector3 lastMousePosition; // Store the last mouse position for dragging

    void Start()
    {
        if (initialTilemap == null)
        {
            Debug.LogError("Initial Tilemap is not assigned in the MapManager script. Please assign it in the Inspector.");
            return;
        }

        initialTilemap.CompressBounds(); // Ensure bounds reflect only active tiles
        unlockedTilemaps.Add(initialTilemap);

        UpdateCameraBoundsAndZoom();
        CenterCameraOnBounds(currentBounds);
    }

    void Update()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    void HandleCameraMovement()
    {
        Vector3 dragDelta = Vector3.zero;

        if (Input.touchCount == 1) // Touch drag
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                dragDelta = mainCamera.ScreenToWorldPoint(touch.position) - mainCamera.ScreenToWorldPoint(touch.position - touch.deltaPosition);
            }
        }
        else if (Input.GetMouseButton(0)) // Mouse drag
        {
            if (lastMousePosition != Vector3.zero)
            {
                dragDelta = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.ScreenToWorldPoint(lastMousePosition);
            }
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            lastMousePosition = Vector3.zero; // Reset if not dragging
        }

        if (dragDelta != Vector3.zero)
        {
            Vector3 newPosition = mainCamera.transform.position - dragDelta * dragSensitivity;

            // Clamp camera position within bounds
            ClampCameraPosition(ref newPosition);

            mainCamera.transform.position = newPosition;
        }
    }

    void HandleCameraZoom()
    {
        float zoomDelta = 0;

        if (Input.touchCount == 2) // Touch zoom
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);
            float previousPinchDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);

            zoomDelta = previousPinchDistance - currentPinchDistance;
        }
        else // Mouse zoom
        {
            zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }

        if (Mathf.Abs(zoomDelta) > 0)
        {
            float newSize = Mathf.Clamp(mainCamera.orthographicSize - zoomDelta, minZoom, maxZoom);
            mainCamera.orthographicSize = newSize;

            // Clamp camera position after zoom
            Vector3 clampedPosition = mainCamera.transform.position;
            ClampCameraPosition(ref clampedPosition);
            mainCamera.transform.position = clampedPosition;
        }
    }

    public void UnlockNextArea()
    {
        if (lockedTilemaps.Count > 0)
        {
            Tilemap tilemapToUnlock = lockedTilemaps[0];

            if (tilemapToUnlock == null)
            {
                Debug.LogError("One of the locked Tilemaps is missing or unassigned. Please check the Locked Tilemaps list.");
                return;
            }

            tilemapToUnlock.GetComponent<TilemapRenderer>().enabled = true;
            tilemapToUnlock.CompressBounds();

            unlockedTilemaps.Add(tilemapToUnlock);
            UpdateCameraBoundsAndZoom();

            lockedTilemaps.RemoveAt(0);
        }
        else
        {
            Debug.Log("No more areas to unlock!");
        }
    }

    private void UpdateCameraBoundsAndZoom()
    {
        // Recalculate combined bounds of all unlocked Tilemaps
        currentBounds = unlockedTilemaps[0].localBounds;
        foreach (Tilemap tilemap in unlockedTilemaps)
        {
            tilemap.CompressBounds();
            currentBounds.Encapsulate(tilemap.localBounds);
        }

        // Calculate max zoom to prevent showing the void
        float screenRatio = (float)Screen.width / Screen.height;
        float verticalSize = currentBounds.size.y / 2f;
        float horizontalSize = (currentBounds.size.x / 2f) / screenRatio;

        // Max zoom is the smaller of the vertical or horizontal size
        maxZoom = Mathf.Min(verticalSize, horizontalSize);
    }

    private void CenterCameraOnBounds(Bounds bounds)
    {
        Vector3 center = bounds.center;
        center.z = mainCamera.transform.position.z;
        mainCamera.transform.position = center;

        ClampCameraPosition(ref center);
        mainCamera.transform.position = center;
    }

    private void ClampCameraPosition(ref Vector3 position)
    {
        float cameraHalfHeight = mainCamera.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;

        position.x = Mathf.Clamp(position.x, currentBounds.min.x + cameraHalfWidth, currentBounds.max.x - cameraHalfWidth);
        position.y = Mathf.Clamp(position.y, currentBounds.min.y + cameraHalfHeight, currentBounds.max.y - cameraHalfHeight);
        position.z = mainCamera.transform.position.z;
    }
}
