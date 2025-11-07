using TMPro;
using UnityEngine;

public class S_TimerAndScore : MonoBehaviour
{
    [Header("UI Refs")]
    public TMP_Text TimerText;
    public TMP_Text ScoreText;
    [Header("Debug Info:")]
    public float CurrentTime = 0f;
    public int CurrentScore = 0;
    void Start()
    {
        if (TimerText && ScoreText == null)
        {
            Debug.LogError("S_TimerAndScore: TimerText or ScoreText is not assigned in the inspector.", this);
        }
    }

    public void UpdateTimer()
    {
        CurrentTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(CurrentTime / 60F);
        int seconds = Mathf.FloorToInt(CurrentTime - minutes * 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateScore()
    {

    }
}
