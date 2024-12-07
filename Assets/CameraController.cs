using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Speed of camera movement
    public float zoomSpeed = 0.1f; // Speed of zooming
    public float minZoom = 5f; // Minimum orthographic size (zoomed in)
    public float maxZoom = 15f; // Maximum orthographic size (zoomed out)

    public Vector2 minBoundary = Vector2.zero; // Minimum boundary for camera movement
    public Vector2 maxBoundary = Vector2.one * 10; // Maximum boundary for camera movement

    private Vector3 touchStart; // Initial touch or click position
    private bool isZooming; // Whether the camera is zooming

    void Update()
    {
        HandleTouchInput();
        HandleMouseInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1 && !isZooming)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = Camera.main.ScreenToWorldPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 currentTouch = Camera.main.ScreenToWorldPoint(touch.position);
                Vector3 direction = touchStart - currentTouch;

                Camera.main.transform.position += direction * panSpeed * Time.deltaTime;
                ClampCameraPosition();
            }
        }

        if (Input.touchCount == 2)
        {
            isZooming = true;

            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevMagnitude = (touch1PrevPos - touch2PrevPos).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Camera.main.orthographicSize -= difference * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }

        if (Input.touchCount < 2)
        {
            isZooming = false;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = touchStart - currentMouse;

            Camera.main.transform.position += direction * panSpeed * Time.deltaTime;
            ClampCameraPosition();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed * 100f * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }

    private void ClampCameraPosition()
    {
        Vector3 pos = Camera.main.transform.position;

        // Ensure the camera stays within the current boundaries
        pos.x = Mathf.Clamp(pos.x, minBoundary.x, maxBoundary.x);
        pos.y = Mathf.Clamp(pos.y, minBoundary.y, maxBoundary.y);

        Camera.main.transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        // Visualize the camera boundaries in the Scene view
        Gizmos.color = Color.red;

        Vector3 bottomLeft = new Vector3(minBoundary.x, minBoundary.y, 0);
        Vector3 bottomRight = new Vector3(maxBoundary.x, minBoundary.y, 0);
        Vector3 topLeft = new Vector3(minBoundary.x, maxBoundary.y, 0);
        Vector3 topRight = new Vector3(maxBoundary.x, maxBoundary.y, 0);

        // Draw boundary rectangle
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
