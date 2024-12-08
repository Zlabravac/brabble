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
    public GameObject particlePrefab; // Prefab for particle effects
    public AudioClip unlockSound; // Sound clip for unlocking areas
    public GameObject soundPrefab; // Prefab with an AudioSource for sound effects

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

            // Add to unlocked Tilemaps and update bounds
            unlockedTilemaps.Add(tilemapToUnlock);

            // Get the position of the new Tilemap's center
            Vector3 newSectionPosition = tilemapToUnlock.localBounds.center;

            // Spawn particles at the screen edge toward the unlocked section
            SpawnParticlesForUnlock(newSectionPosition);

            // Play the unlock sound
            PlaySoundAtPosition(newSectionPosition);

            UpdateCameraBoundsAndZoom();
            lockedTilemaps.RemoveAt(0);
        }
        else
        {
            Debug.Log("No more areas to unlock!");
        }
    }

    private void SpawnParticlesForUnlock(Vector3 targetPosition)
    {
        if (particlePrefab == null)
        {
            Debug.LogWarning("Particle prefab is not assigned!");
            return;
        }

        // Determine direction of the unlock relative to the camera
        Vector3 direction = targetPosition - mainCamera.transform.position;

        // Normalize the direction
        direction.Normalize();

        // Determine the position on the screen edge
        Vector3 screenEdgePosition = Vector3.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal direction (left or right)
            screenEdgePosition.x = direction.x > 0 ? 1 : 0; // Right edge or left edge
            screenEdgePosition.y = 0.5f; // Middle of the screen vertically
        }
        else
        {
            // Vertical direction (top or bottom)
            screenEdgePosition.y = direction.y > 0 ? 1 : 0; // Top edge or bottom edge
            screenEdgePosition.x = 0.5f; // Middle of the screen horizontally
        }

        // Convert screen edge position from Viewport to World space
        Vector3 particleWorldPosition = mainCamera.ViewportToWorldPoint(new Vector3(screenEdgePosition.x, screenEdgePosition.y, mainCamera.nearClipPlane));

        // Spawn the particle system at the calculated position
        Instantiate(particlePrefab, particleWorldPosition, Quaternion.identity);
    }

    private void PlaySoundAtPosition(Vector3 position)
    {
        if (unlockSound != null && soundPrefab != null)
        {
            // Instantiate the sound prefab at the specified position
            GameObject soundObject = Instantiate(soundPrefab, position, Quaternion.identity);

            // Get the AudioSource component and play the sound
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = unlockSound;
                audioSource.Play();

                // Destroy the sound object after the clip finishes
                Destroy(soundObject, unlockSound.length);
            }
        }
    }

    private void UpdateCameraBoundsAndZoom()
    {
        currentBounds = unlockedTilemaps[0].localBounds;
        foreach (Tilemap tilemap in unlockedTilemaps)
        {
            tilemap.CompressBounds();
            currentBounds.Encapsulate(tilemap.localBounds);
        }

        float screenRatio = (float)Screen.width / Screen.height;
        float verticalSize = currentBounds.size.y / 2f;
        float horizontalSize = (currentBounds.size.x / 2f) / screenRatio;

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
