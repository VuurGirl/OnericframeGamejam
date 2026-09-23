using System.Collections.Generic;
using UnityEngine;

public class ZoneVisualManager : MonoBehaviour
{
    public static ZoneVisualManager Instance { get; private set; }

    public List<ZoneAnimationController> zoneControllers;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateZone(int zoneId, int level, bool completed)
    {
        ZoneAnimationController controller = zoneControllers.Find(z => z.zoneId == zoneId);
        if (controller == null)
        {
            Debug.LogWarning($"Geen ZoneAnimationController gevonden voor zoneId {zoneId}");
            return;
        }

        if (completed) controller.ShowCompleted();
        else controller.ShowLevel(level);
    }
}