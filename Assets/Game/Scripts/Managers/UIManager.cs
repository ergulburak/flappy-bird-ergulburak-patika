using System;
using TMPro;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{
  public event EventHandler OnRetry;

  [SerializeField] private GameObject inGamePanel;
  [SerializeField] private GameObject gameOverPanel;
  [SerializeField] private GameObject mainMenuPanel;


  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI playerScoreText;
  [SerializeField] private TextMeshProUGUI highScoreText;

  private void Start()
  {
    LevelManager.Instance.OnObstaclePassed += OnObstaclePassed;
  }

  private void OnObstaclePassed(object sender, int e)
  {
    scoreText.text = e.ToString();
  }

  public void OnGameStart()
  {
    mainMenuPanel.SetActive(false);
    inGamePanel.SetActive(true);
  }

  public void OnGameOver()
  {
    inGamePanel.SetActive(false);
    gameOverPanel.SetActive(true);
    playerScoreText.text = $"SCORE: {scoreText.text}";
    highScoreText.text = $"HIGH SCORE: {GameManager.Instance.GetHighScore()}";
  }

  public void OnRetryButton()
  {
    OnRetry?.Invoke(this, EventArgs.Empty);
  }
}
