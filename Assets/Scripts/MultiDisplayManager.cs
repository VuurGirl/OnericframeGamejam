using UnityEngine;

public class MultiDisplayManager : MonoBehaviour
{
    void Start()
    {
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            // Optioneel: resolutie forceren per scherm
            Display.displays[i].SetRenderingResolution(1920, 1080);
        }
    }
}