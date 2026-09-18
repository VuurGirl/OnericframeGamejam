using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordEntry
{
    public string word;
    public Sprite sprite;
}

public class WordSpawner : MonoBehaviour
{
    public Camera targetCamera;              // camera van scherm 2
    public GameObject wordPrefab;            // prefab met SpriteRenderer + BoxCollider2D
    public List<WordEntry> words;            // koppel hier per woord de sprite
    public float spawnPadding = 1f;          // marge tot de rand van het scherm, in world units

    [Header("Overlap check")]
    public float minDistanceBetweenWords = 1.5f; // minimale afstand tussen twee woorden
    public int maxSpawnAttempts = 30;             // hoeveel keer opnieuw proberen bij overlap

    [Header("Start gedrag")]
    public bool spawnFirstWordOnStart = false;

    private int nextIndex = 0;
    private readonly List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        if (spawnFirstWordOnStart)
        {
            SpawnNextWord();
        }
    }

    // Roep deze aan om het eerstvolgende woord uit de lijst te spawnen
    public GameObject SpawnNextWord()
    {
        if (nextIndex >= words.Count)
        {
            Debug.LogWarning("Alle woorden zijn al gespawned.");
            return null;
        }

        GameObject obj = SpawnWord(words[nextIndex]);
        nextIndex++;
        return obj;
    }

    // Of roep deze aan om een specifiek woord op naam te spawnen
    public GameObject SpawnWord(string wordName)
    {
        WordEntry entry = words.Find(w => w.word == wordName);
        if (entry == null)
        {
            Debug.LogWarning($"Woord '{wordName}' niet gevonden in de lijst.");
            return null;
        }
        return SpawnWord(entry);
    }

    private GameObject SpawnWord(WordEntry entry)
    {
        Vector3 pos = GetNonOverlappingPosition();
        GameObject obj = Instantiate(wordPrefab, pos, Quaternion.identity);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = entry.sprite;
        }

        obj.name = entry.word;
        spawnedPositions.Add(pos);
        return obj;
    }

    private Vector3 GetNonOverlappingPosition()
    {
        Vector3 pos = GetRandomPositionOnScreen();

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            bool overlaps = false;
            foreach (Vector3 existing in spawnedPositions)
            {
                if (Vector3.Distance(pos, existing) < minDistanceBetweenWords)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                return pos;
            }

            pos = GetRandomPositionOnScreen();
        }

        Debug.LogWarning("Geen vrije plek gevonden na max pogingen, woord wordt alsnog geplaatst.");
        return pos;
    }

    private Vector3 GetRandomPositionOnScreen()
    {
        float distance = targetCamera.nearClipPlane + 1f;
        Vector3 bottomLeft = targetCamera.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 topRight = targetCamera.ViewportToWorldPoint(new Vector3(1f, 1f, distance));

        float x = Random.Range(bottomLeft.x + spawnPadding, topRight.x - spawnPadding);
        float y = Random.Range(bottomLeft.y + spawnPadding, topRight.y - spawnPadding);

        return new Vector3(x, y, 0f);
    }
}

    // Reset