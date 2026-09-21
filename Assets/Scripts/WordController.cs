using UnityEngine;

public class WordController : MonoBehaviour
{
    [HideInInspector] public WordData data;
    [HideInInspector] public WordSpawner spawner;

    public void HandleDrop(Vector2 worldPosition, LayerMask dropZoneLayer)
    {
        Collider2D zoneCollider = Physics2D.OverlapPoint(worldPosition, dropZoneLayer);
        DropZone zone = zoneCollider != null ? zoneCollider.GetComponent<DropZone>() : null;

        bool correct = zone != null && zone.zoneId == data.correctZoneId;

        if (correct)
        {
            data.level++;
            // TODO: hier later de "goed"-animatie triggeren
        }
        else
        {
            data.level = Mathf.Max(1, data.level - 1);
            // TODO: hier later de "fout"-animatie triggeren
        }

        spawner.OnWordResolved(data, correct);
        Destroy(gameObject); // WordDotFollower.OnDestroy ruimt de stip vanzelf op
    }
}