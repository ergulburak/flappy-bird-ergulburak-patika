using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  private SpriteRenderer renderer => GetComponent<SpriteRenderer>();
  [SerializeField] private Sprite flipSprite;
  [SerializeField] private Sprite flopSprite;

  private void Start()
  {
    InputManager.Instance.OnClick += OnClickAnimation;
  }

  private void OnClickAnimation(object sender, bool e)
  {
    renderer.sprite = e ? flipSprite : flopSprite;
  }
}
