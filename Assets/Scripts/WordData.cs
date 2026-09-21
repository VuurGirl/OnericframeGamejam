using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordData
{
    public string word;
    public int correctZoneId;           // welk vak (0-4) hoort bij dit woord
    public List<Sprite> spritesPerLevel; // index 0 = level 1, index 1 = level 2, enz.

    [System.NonSerialized] public int level = 1;

    public int MaxLevel => spritesPerLevel.Count;

    public Sprite GetCurrentSprite()
    {
        int index = Mathf.Clamp(level - 1, 0, spritesPerLevel.Count - 1);
        return spritesPerLevel[index];
    }
}