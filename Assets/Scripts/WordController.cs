using UnityEngine;

public class WordController : MonoBehaviour
{
    [HideInInspector] public WordData data;
    [HideInInspector] public WordSpawner spawner;

    public void HandleDrop(Vector2 worldPosition, LayerMask dropZoneLayer)
    {
        Debug.Log($"[DROP-CHECK] Gedropt op wereldpositie: {worldPosition} | LayerMask waarde: {dropZoneLayer.value}");

        DropZone[] allZones = FindObjectsOfType<DropZone>();
        foreach (DropZone z in allZones)
        {
            Collider2D col = z.GetComponent<Collider2D>();
            Debug.Log($"[ZONE-INFO] {z.name} | ZoneId: {z.zoneId} | Layer: {LayerMask.LayerToName(z.gameObject.layer)} | Bounds: {col.bounds}");
        }

        Collider2D zoneCollider = Physics2D.OverlapPoint(worldPosition, dropZoneLayer);
        DropZone zone = zoneCollider != null ? zoneCollider.GetComponent<DropZone>() : null;

        bool correct = zone != null && zone.zoneId == data.correctZoneId;

        Debug.Log($"[DROP] Woord: {data.word} | Zone gevonden: {(zone != null ? zone.zoneId.ToString() : "GEEN")} | Verwachte zone: {data.correctZoneId} | Correct: {correct} | Level vóór: {data.level}");

        if (correct) data.level++;
        else data.level = Mathf.Max(1, data.level - 1);

        spawner.OnWordResolved(data, correct);
        Destroy(gameObject);
    }
}