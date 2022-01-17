using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
  public event EventHandler<int> OnObstaclePassed;

  private List<Obstacle> obstacles = new List<Obstacle>();
  private float obstacleRangeLimit = 6f;
  private int obstaclesSpawned;
  private int obstaclesPassed;
  private bool isTheGameBeingPlayed;

  [SerializeField] private float obstacleSpeed = 5f;
  [SerializeField] [Range(.2f, 5f)] private float obstacleSpawnTimer;
  [SerializeField] private GameObject obstacleParentPrefab;
  [SerializeField] private GameObject obstacleHeadPrefab;
  [SerializeField] private GameObject obstacleBodyPrefab;
  [SerializeField] [ReadOnly] private Difficulty difficulty = Difficulty.Easy;

  private void Update()
  {
    if (isTheGameBeingPlayed)
      ObstacleMovement();
  }

  private void ObstacleMovement()
  {
    foreach (var obstacle in obstacles.ToList())
    {
      bool isPlayerPassTheObstacle = obstacle.GetObstacleXPosition() > LevelConsts.PlayerXPosition;
      obstacle.Move();
      if (isPlayerPassTheObstacle && obstacle.GetObstacleXPosition() <= LevelConsts.PlayerXPosition)
      {
        obstaclesPassed++;
        OnObstaclePassed?.Invoke(this, obstaclesPassed);
      }

      if (obstacle.GetObstacleXPosition() < LevelConsts.ObstacleDestroyPosition)
      {
        Destroy(obstacle.gameObject);
        obstacles.Remove(obstacle);
      }
    }
  }

  public void StartSpawning()
  {
    SetDifficulty();
    InvokeRepeating(nameof(ObstacleSpawning), 0f, obstacleSpawnTimer);
    isTheGameBeingPlayed = true;
  }

  public void StopSpawning()
  {
    isTheGameBeingPlayed = false;
    CancelInvoke(nameof(ObstacleSpawning));
  }


  private void SetDifficulty()
  {
    CalculateDifficulty();
    switch (difficulty)
    {
      case Difficulty.Easy:
        obstacleRangeLimit = 10f;
        break;
      case Difficulty.Medium:
        obstacleRangeLimit = 8f;
        break;
      case Difficulty.Hard:
        obstacleRangeLimit = 6f;
        break;
      case Difficulty.Impossible:
        obstacleRangeLimit = 5f;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
    }
  }

  private void CalculateDifficulty()
  {
    if (obstaclesSpawned >= 30) difficulty = Difficulty.Impossible;
    if (obstaclesSpawned >= 20) difficulty = Difficulty.Hard;
    if (obstaclesSpawned >= 10) difficulty = Difficulty.Medium;
    difficulty = Difficulty.Easy;
  }

  private void ObstacleSpawning()
  {
    var heightEdgeLimit = 2f;
    var minHeight = obstacleRangeLimit * .5f + heightEdgeLimit;
    var maxHeight = LevelConsts.ObstacleHeightLimit - obstacleRangeLimit * .5f - heightEdgeLimit;
    var randomHeight = Random.Range(minHeight, maxHeight);
    CreateObstacle(randomHeight);
    obstaclesSpawned++;
    SetDifficulty();
  }

  private void CreateObstacle(float randomHeight)
  {
    var obstacleParent = Instantiate(obstacleParentPrefab);
    var obstacleScript = obstacleParent.GetComponent<Obstacle>();
    obstacleScript.Initialize(obstacleBodyPrefab, obstacleHeadPrefab, randomHeight, obstacleRangeLimit, obstacleSpeed);
    obstacles.Add(obstacleScript);
  }

  public int GetObstaclesPassed()
  {
    return obstaclesPassed;
  }
}
