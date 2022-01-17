using System;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
  private float obstacleSpeed;
  private GameObject topObstacleBody;
  private GameObject topObstacleHead;
  private GameObject bottomObstacleBody;
  private GameObject bottomObstacleHead;

  public void Initialize(GameObject obstacleBodyPrefab, GameObject obstacleHeadPrefab, float yHeight, float rangeSize,
    float speed)
  {
    obstacleSpeed = speed;
    var bottomHeight = yHeight - rangeSize / 2f;
    var topHeight = LevelConsts.CameraOrthograficSize * 2f - yHeight - rangeSize / 2f;

    var bottomList = CreateObstacle(obstacleBodyPrefab, obstacleHeadPrefab, bottomHeight, true);
    var topList = CreateObstacle(obstacleBodyPrefab, obstacleHeadPrefab, topHeight, false);

    transform.position = new Vector3(LevelConsts.ObstacleSpawnPosition, 0);

    var pipeRigidBody = gameObject.AddComponent<Rigidbody2D>();
    pipeRigidBody.isKinematic = true;

    SetTopObstacle(topList);
    SetBottomObstacle(bottomList);
  }

  private List<GameObject> CreateObstacle(GameObject pipeBodyPrefab, GameObject pipeHeadPrefab, float height,
    bool isLookingUp)
  {
    float pipeHeadYPos;
    float pipeBodyYPos;

    if (isLookingUp)
    {
      pipeHeadYPos = -LevelConsts.CameraOrthograficSize + height - LevelConsts.ObstacleHeadHeight * .45f;
      pipeBodyYPos = -LevelConsts.CameraOrthograficSize;
    }
    else
    {
      pipeHeadYPos = +LevelConsts.CameraOrthograficSize - height + LevelConsts.ObstacleHeadHeight * .45f;
      pipeBodyYPos = +LevelConsts.CameraOrthograficSize;
    }

    var pipeHead = Instantiate(pipeHeadPrefab, transform);
    pipeHead.transform.localPosition = new Vector3(0, pipeHeadYPos);

    var pipeBody = Instantiate(pipeBodyPrefab, transform);
    pipeBody.transform.localPosition = new Vector3(0, pipeBodyYPos);
    pipeBody.transform.localScale = new Vector3(1, isLookingUp ? height : -height, 1);

    var pipeBodyList = new List<GameObject>() { pipeBody, pipeHead };
    return pipeBodyList;
  }

  public void SetTopObstacle(List<GameObject> bodyList)
  {
    if (!bodyList.Count.Equals(2))
    {
      return;
    }

    topObstacleBody = bodyList[0];
    topObstacleHead = bodyList[1];
  }

  public void SetBottomObstacle(List<GameObject> bodyList)
  {
    if (!bodyList.Count.Equals(2))
    {
      return;
    }

    bottomObstacleBody = bodyList[0];
    bottomObstacleHead = bodyList[1];
  }

  public void Move()
  {
    transform.Translate(new Vector3(-1, 0, 0) * obstacleSpeed * Time.deltaTime);
  }

  public float GetObstacleXPosition()
  {
    return transform.position.x;
  }
}
