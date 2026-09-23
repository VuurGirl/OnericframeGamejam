using UnityEngine;

public class ZoneAnimationController : MonoBehaviour
{
    public int zoneId; // moet matchen met WordData.correctZoneId

    [Tooltip("Index 0 = level 1, Index 1 = level 2, Index 2 = level 3, Index 3 (laatste) = voltooid")]
    public GameObject[] levelAnimations;

    void Start()
    {
        ShowLevel(1); // meteen bij start de eerste animatie van elke zone tonen
    }

    public void ShowLevel(int level)
    {
        int index = Mathf.Clamp(level - 1, 0, levelAnimations.Length - 2); // nooit per ongeluk de "voltooid"-animatie via level triggeren
        SetActiveIndex(index);
    }

    public void ShowCompleted()
    {
        SetActiveIndex(levelAnimations.Length - 1); // laatste animatie, blijft daarna definitief staan
    }

    private void SetActiveIndex(int index)
    {
        for (int i = 0; i < levelAnimations.Length; i++)
        {
            levelAnimations[i].SetActive(i == index);
        }
    }
}