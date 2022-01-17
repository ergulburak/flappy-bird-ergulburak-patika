using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
  public event EventHandler<bool> OnClick;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
      PlayerOnClick(true);

    if (Input.GetMouseButtonUp(0))
      PlayerOnClick(false);
  }

  private void PlayerOnClick(bool click)
  {
    OnClick?.Invoke(this, click);
  }
}
