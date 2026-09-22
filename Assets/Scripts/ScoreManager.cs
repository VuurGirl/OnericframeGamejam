using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Referenties")]
    public WordSpawner wordSpawner;

    [Header("Instellingen")]
    public int maxWrongAllowed = 5;

    [Header("UI - Display 1")]
    public GameObject gameOverPanelDisplay1;
    public GameObject gameWonPanelDisplay1;
    public TMP_Text scoreTextDisplay1;

    [Header("UI - Display 2")]
    public GameObject gameOverPanelDisplay2;
    public GameObject gameWonPanelDisplay2;
    public TMP_Text scoreTextDisplay2;

    private int totalWords;
    private int correctCount = 0;
    private int wrongCount = 0;
    private int completedWords = 0;
    private bool gameEnded = false;

    public bool IsGameEnded => gameEnded;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        totalWords = wordSpawner != null ? wordSpawner.allWords.Count : 0;

        gameOverPanelDisplay1.SetActive(false);
        gameOverPanelDisplay2.SetActive(false);
        gameWonPanelDisplay1.SetActive(false);
        gameWonPanelDisplay2.SetActive(false);

        UpdateScoreUI();
    }

    public void RegisterResult(bool wasCorrect, bool wordCompleted)
    {
        if (gameEnded) return;

        if (wasCorrect) correctCount++;
        else wrongCount++;

        if (wordCompleted) completedWords++;

        UpdateScoreUI();

        Debug.Log($"[SCORE] Correct: {correctCount} | Fout: {wrongCount}/{maxWrongAllowed} | Voltooid: {completedWords}/{totalWords}");

        if (wrongCount >= maxWrongAllowed)
        {
            TriggerGameOver();
        }
        else if (totalWords > 0 && completedWords >= totalWords)
        {
            TriggerGameWon();
        }
    }

    private void UpdateScoreUI()
    {
        string scoreString = $"Goed: {correctCount}   Fout: {wrongCount}/{maxWrongAllowed}   Voltooid: {completedWords}/{totalWords}";

        if (scoreTextDisplay1 != null) scoreTextDisplay1.text = scoreString;
        if (scoreTextDisplay2 != null) scoreTextDisplay2.text = scoreString;
    }

    private void TriggerGameOver()
    {
        gameEnded = true;
        gameOverPanelDisplay1.SetActive(true);
        gameOverPanelDisplay2.SetActive(true);
    }

    private void TriggerGameWon()
    {
        gameEnded = true;
        gameWonPanelDisplay1.SetActive(true);
        gameWonPanelDisplay2.SetActive(true);
    }
}