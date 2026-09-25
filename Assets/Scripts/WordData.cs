using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordData
{
    public string word;
    public int correctZoneId;
    public List<Sprite> spritesPerLevel;

    [System.NonSerialized] public int level = 1;

    public int MaxLevel => spritesPerLevel.Count;

    public Sprite GetCurrentSprite()
    {
        int index = Mathf.Clamp(level - 1, 0, spritesPerLevel.Count - 1);
        return spritesPerLevel[index];
    }
}