using System;
using UnityEngine;

public class Player : Singleton<Player>
{
  public bool PlayerCanMove;
  public event EventHandler OnPlayerDied;

  [SerializeField] private float timeMultiplier = 5f;
  [SerializeField] private float downForce = -10f;
  [SerializeField] private float jumpForce = 3f;
  [SerializeField] private float maxRotationAngle = 50f;

  private bool playerClick;

  private Vector3 direction = Vector3.zero;

  private void Start()
  {
    InputManager.Instance.OnClick += ClickDetect;
  }

  private void ClickDetect(object sender, bool e)
  {
    playerClick = e;
  }

  private void Update()
  {
    if (PlayerCanMove)
    {
      MoveVertical();
      CalculateAngle();
    }
  }

  private void MoveVertical()
  {
    direction.y = playerClick
      ? jumpForce
      : direction.y + (downForce * Time.deltaTime);
    transform.Translate(direction * Time.deltaTime * timeMultiplier, Space.World);
    playerClick = playerClick ? !playerClick : playerClick;
  }

  private void CalculateAngle()
  {
    var rot = Mathf.Clamp(direction.y * 45, -maxRotationAngle, maxRotationAngle);
    transform.eulerAngles = new Vector3(0, 0, rot);
  }

  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.CompareTag("Obstacle"))
    {
      OnPlayerDied?.Invoke(this, EventArgs.Empty);
    }
  }
}
