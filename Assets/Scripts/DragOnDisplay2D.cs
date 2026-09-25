using UnityEngine;
using UnityEngine.InputSystem;

public class DragOnDisplay2D : MonoBehaviour
{
    public Camera dragCamera;
    public int displayIndex = 1;
    public LayerMask dropZoneLayer;

    private Transform dragging;
    private float dragDepth;

    void Update()
    {
        Vector2 rawMouse = Mouse.current.position.ReadValue();
        Vector3 mousePos = Display.RelativeMouseAt(rawMouse);
        bool multiDisplay = Display.displays.Length > 1;
        int currentDisplay = multiDisplay ? (int)mousePos.z : 0;

        if (multiDisplay && currentDisplay != displayIndex) return;

        Vector3 screenPos = multiDisplay
            ? new Vector3(mousePos.x, mousePos.y, 0f)
            : (Vector3)rawMouse;

        Vector3 worldPos = dragCamera.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, dragCamera.nearClipPlane + 1f));
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPos2D);
            if (hit != null && hit.GetComponent<WordController>() != null)
            {
                dragging = hit.transform;
                dragDepth = dragCamera.WorldToScreenPoint(dragging.position).z;
            }
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (dragging != null)
            {
                WordController word = dragging.GetComponent<WordController>();
                word?.HandleDrop(worldPos2D, dropZoneLayer);
            }
            dragging = null;
        }

        if (dragging != null)
        {
            Vector3 newWorldPos = dragCamera.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, dragDepth));
            dragging.position = new Vector3(newWorldPos.x, newWorldPos.y, dragging.position.z);
        }
    }
}