using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
  [SerializeField] [ReadOnly] private GameStates state = GameStates.Menu;
  private bool isGameStarted;
  private int highScore;

  private void Start()
  {
    highScore = PlayerPrefs.GetInt("HighScore", 0);
    InputManager.Instance.OnClick += OnGameStarted;
    Player.Instance.OnPlayerDied += OnPlayerDied;
    UIManager.Instance.OnRetry += OnRetry;
  }

  public int GetHighScore()
  {
    return highScore;
  }

  private void OnRetry(object sender, EventArgs e)
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  private void OnPlayerDied(object sender, EventArgs e)
  {
    state = GameStates.End;
    HandleStates();
  }

  private void OnGameStarted(object sender, bool e)
  {
    if (!isGameStarted)
    {
      isGameStarted = !isGameStarted;
      state = GameStates.InGame;
      HandleStates();
    }
  }

  private void HandleStates()
  {
    switch (state)
    {
      case GameStates.Menu:
        break;
      case GameStates.InGame:
        UIManager.Instance.OnGameStart();
        Player.Instance.PlayerCanMove = true;
        LevelManager.Instance.StartSpawning();
        break;
      case GameStates.End:

        if (LevelManager.Instance.GetObstaclesPassed() > highScore)
        {
          highScore = LevelManager.Instance.GetObstaclesPassed();
          PlayerPrefs.SetInt("HighScore", highScore);
        }

        Player.Instance.PlayerCanMove = false;
        LevelManager.Instance.StopSpawning();
        UIManager.Instance.OnGameOver();
        break;
    }
  }
}
