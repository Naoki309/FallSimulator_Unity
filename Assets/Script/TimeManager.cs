using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float startTime;
    private float elapsedTime;
    private float pausedTime; // 一時停止時の経過時間を記録
    private bool timerRunning = false;

    public void ResetTimer()
    {
        timerRunning = false;
        pausedTime = 0;
        elapsedTime = 0;
        timeText.text = "Time:0.00 Seconds";
    }

    void Update()
    {
        if (timerRunning)
        {
            // 経過時間を更新
            elapsedTime = Time.time - startTime;
            // 経過時間をテキストとして表示
            timeText.text = $"Time: {elapsedTime:F2} Seconds";
        }
    }

    public void ShowNoProgressMessage(string message)
    {
        timeText.text = message;
    }

    public void StartTimer()
    {
        if (!timerRunning)
        {
            // タイマーを開始
            startTime = Time.time - pausedTime;
            timerRunning = true;
        }
    }

    public void StopTimer()
    {
        // タイマーを停止
        timerRunning = false;
        // 一時停止時の経過時間をリセット
        pausedTime = 0;
    }

    public void PauseTimer()
    {
        if (timerRunning)
        {
            // 一時停止時の経過時間を保存
            pausedTime = Time.time - startTime;
            timerRunning = false;
        }
    }

    public void ResumeTimer()
    {
        if (!timerRunning)
        {
            // タイマーを再開
            startTime = Time.time - pausedTime;
            timerRunning = true;
        }
    }
}
