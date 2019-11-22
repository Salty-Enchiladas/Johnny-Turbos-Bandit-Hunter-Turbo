using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverPanel;
    [Space]
    public int banditScore;
    public int bankerSaveScore;
    public int bankerDeathPenalty;
    public float timeScaleSpeed;

    [Space, Header("UI")]
    public TextMeshProUGUI killCounter;
    public TextMeshProUGUI scoreTracker;
    public TextMeshProUGUI bankerTracker;
    public TextMeshProUGUI accuracyTracker;
    public TextMeshProUGUI headshotsCounter;

    [Space, Header("Optional")]
    public bool gainHealthOnBankerSave;
    public int healthGained;

    int kills;
    int score;
    int bankersSaved;

    int shotsFired;
    int shotsHit;
    int headshots;

    Health health;
    SpawnManager spawnManager;

    private void Awake()
    {
        Instance = this;
        spawnManager = GetComponent<SpawnManager>();

        health = GameObject.Find("Player").GetComponent<Health>();
    }

    public void EnemyKilled()
    {
        kills++;
        killCounter.text = "Kills: " + kills;

        score += banditScore;
        scoreTracker.text = "Score: " + score;

        spawnManager.EnemyKilled();
    }

    public void PlayerDied()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Camera.main.transform.parent = null;

        headshotsCounter.text = "Headshots: " + headshots;
        float accuracy = 100 - (((float)(shotsFired - shotsHit) / shotsFired) * 100);
        accuracyTracker.text = "Accuracy: " + accuracy + "%";
        gameOverPanel.SetActive(true);
    }

    public void BankerSaved()
    {
        bankersSaved++;
        bankerTracker.text = "Bankers Saved: " + bankersSaved;

        score += bankerSaveScore;
        scoreTracker.text = "Score: " + score;

        if (gainHealthOnBankerSave && health)
            health.GainHealth(healthGained);
    }

    public void BankerKilled()
    {
        if (score - bankerDeathPenalty > 0)
            score -= bankerDeathPenalty;
        else
            score = 0;

        scoreTracker.text = "Score: " + score;
    }

    public void ShotFired()
    {
        shotsFired++;
    }

    public void EnemyHit()
    {
        shotsHit++;
    }

    public void Headshot()
    {
        headshots++;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
