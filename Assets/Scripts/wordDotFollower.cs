using UnityEngine;

public class WordDotFollower : MonoBehaviour
{
    private Transform dot;
    private Camera wordCamera;
    private Camera dotCamera;

    public void Init(Transform dotTransform, Camera wordCam, Camera dotCam)
    {
        dot = dotTransform;
        wordCamera = wordCam;
        dotCamera = dotCam;
        UpdateDotPosition();
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
        dotWorldPos.z = dot.position.z;

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