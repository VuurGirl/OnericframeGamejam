using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    public Camera targetCamera;
    public GameObject wordPrefab;
    public List<WordData> allWords;   // eenmalig invullen in Inspector
    public float spawnPadding = 1f;

    [Header("Overlap check")]
    public float minDistanceBetweenWords = 1.5f;
    public int maxSpawnAttempts = 30;

    [Header("Start gedrag")]
    public bool spawnFirstWordOnStart = false;

    [Header("Stip op display 1")]
    public Camera dotCamera;
    public GameObject dotPrefab;

    private Queue<WordData> queue = new Queue<WordData>();
    private readonly List<Transform> activeWords = new List<Transform>();

    void Awake()
    {
        foreach (WordData w in allWords)
        {
            w.level = 1;
            queue.Enqueue(w);
        }
    }

    void Start()
    {
        if (spawnFirstWordOnStart) SpawnNextWord();
    }

    public GameObject SpawnNextWord()
    {
        if (queue.Count == 0)
        {
            Debug.Log("Geen woorden meer in de queue.");
            return null;
        }

        return SpawnWord(queue.Dequeue());
    }

    private GameObject SpawnWord(WordData data)
    {
        Vector3 pos = GetNonOverlappingPosition();
        GameObject obj = Instantiate(wordPrefab, pos, Quaternion.identity);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = data.GetCurrentSprite();

        obj.name = $"{data.word}_lvl{data.level}";

        WordController controller = obj.AddComponent<WordController>();
        controller.data = data;
        controller.spawner = this;

        GameObject dotObj = Instantiate(dotPrefab);
        dotObj.name = data.word + "_Dot";
        WordDotFollower follower = obj.AddComponent<WordDotFollower>();
        follower.Init(dotObj.transform, targetCamera, dotCamera);

        activeWords.Add(obj.transform);
        return obj;
    }

    public void OnWordResolved(WordData data, bool wasCorrect)
    {
        activeWords.RemoveAll(t => t == null);

        bool completed = wasCorrect && data.level > data.MaxLevel;
        if (completed)
        {
            Debug.Log(data.word + " is voltooid!");
        }
        else
        {
            queue.Enqueue(data);
        }

        SpawnNextWord();
    }

    private Vector3 GetNonOverlappingPosition()
    {
        activeWords.RemoveAll(t => t == null);
        Vector3 pos = GetRandomPositionOnScreen();

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            bool overlaps = false;
            foreach (Transform t in activeWords)
            {
                if (Vector3.Distance(pos, t.position) < minDistanceBetweenWords)
                {
                    overlaps = true;
                    break;
                }
            }
            if (!overlaps) return pos;
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