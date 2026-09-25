using UnityEngine;

public class WordController : MonoBehaviour
{
    [HideInInspector] public WordData data;
    [HideInInspector] public WordSpawner spawner;

    private GameObject correctFeedbackPrefab;
    private GameObject wrongFeedbackPrefab;
    private float feedbackDuration = 1f;
    private Camera display2Camera;
    private Camera display1Camera;
    private int display1LayerIndex;
    private int display2LayerIndex;

    public void SetFeedbackSettings(GameObject correctPrefab, GameObject wrongPrefab, float duration,
        Camera cam2, Camera cam1, LayerMask layer1, LayerMask layer2)
    {
        correctFeedbackPrefab = correctPrefab;
        wrongFeedbackPrefab = wrongPrefab;
        feedbackDuration = duration;
        display2Camera = cam2;
        display1Camera = cam1;
        display1LayerIndex = LayerMaskToLayer(layer1);
        display2LayerIndex = LayerMaskToLayer(layer2);
    }

    private int LayerMaskToLayer(LayerMask mask)
    {
        int value = mask.value;
        for (int i = 0; i < 32; i++)
        {
            if ((value & (1 << i)) != 0) return i;
        }
        return 0;
    }

    public void HandleDrop(Vector2 worldPosition, LayerMask dropZoneLayer)
    {
        Collider2D zoneCollider = Physics2D.OverlapPoint(worldPosition, dropZoneLayer);
        DropZone zone = zoneCollider != null ? zoneCollider.GetComponent<DropZone>() : null;

        bool correct = zone != null && zone.zoneId == data.correctZoneId;

        SpawnFeedbackOnBothScreens(correct, worldPosition);

        if (correct) data.level++;
        else data.level = Mathf.Max(1, data.level - 1);

        bool completed = correct && data.level > data.MaxLevel;

        ZoneVisualManager.Instance?.UpdateZone(data.correctZoneId, data.level, completed);
        spawner.OnWordResolved(data, correct);
        Destroy(gameObject);
    }

    private void SpawnFeedbackOnBothScreens(bool correct, Vector2 dropPosition)
    {
        GameObject prefab = correct ? correctFeedbackPrefab : wrongFeedbackPrefab;
        if (prefab == null) return;

        GameObject feedback2 = Instantiate(prefab, dropPosition, Quaternion.identity);
        feedback2.layer = display2LayerIndex;
        Destroy(feedback2, feedbackDuration);

        if (display1Camera != null && display2Camera != null)
        {
            Vector3 viewportPos = display2Camera.WorldToViewportPoint(dropPosition);
            float distance = display1Camera.nearClipPlane + 1f;
            Vector3 pos1 = display1Camera.ViewportToWorldPoint(
                new Vector3(viewportPos.x, viewportPos.y, distance));

            GameObject feedback1 = Instantiate(prefab, pos1, Quaternion.identity);
            feedback1.layer = display1LayerIndex;
            Destroy(feedback1, feedbackDuration);
        }
    }
}