using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 0.5f;
    public float zoomSpeed = 200f;

    public float minY = 5f;
    public float maxY = 50f;

    Vector3 lastMousePos;

    void Update()
    {
        HandleDrag();
        HandleZoom();
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;

            // 🔥 ruch przeciwny do myszki (jak w RTS)
            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * dragSpeed * Time.deltaTime;

            transform.Translate(move, Space.World);

            lastMousePos = Input.mousePosition;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) < 0.01f) return;

        Vector3 pos = transform.position;

        pos.y -= scroll * zoomSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}