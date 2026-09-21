using UnityEngine;

public class WordDotFollower : MonoBehaviour
{
    private Transform dot;
    private Camera wordCamera; // camera die het woord toont (display 2)
    private Camera dotCamera;  // camera die de stip toont (display 1)

    public void Init(Transform dotTransform, Camera wordCam, Camera dotCam)
    {
        dot = dotTransform;
        wordCamera = wordCam;
        dotCamera = dotCam;
        UpdateDotPosition(); // meteen goed zetten, niet pas volgend frame
    }

    void LateUpdate()
    {
        UpdateDotPosition();
    }

    private void UpdateDotPosition()
    {
        if (dot == null || wordCamera == null || dotCamera == null) return;

        Vector3 viewportPos = wordCamera.WorldToViewportPoint(transform.position);

        float dotDistance = dotCamera.nearClipPlane + 1f;
        Vector3 dotWorldPos = dotCamera.ViewportToWorldPoint(
            new Vector3(viewportPos.x, viewportPos.y, dotDistance));
        dotWorldPos.z = dot.position.z; // eigen Z van de stip behouden (sorting)

        dot.position = dotWorldPos;
    }

    void OnDestroy()
    {
        if (dot != null)
        {
            Destroy(dot.gameObject);
        }
    }
}
