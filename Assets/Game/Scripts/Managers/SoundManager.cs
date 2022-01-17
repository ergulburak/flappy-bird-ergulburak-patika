using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
  private AudioSource audioSource => GetComponent<AudioSource>();
  [SerializeField] private AudioClip flipSound;
  [SerializeField] private AudioClip obstaclePassedSound;
  [SerializeField] private AudioClip onPlayerDiedSound;

  private void Start()
  {
    InputManager.Instance.OnClick += OnClick;
    LevelManager.Instance.OnObstaclePassed += OnObstaclePassed;
    Player.Instance.OnPlayerDied += OnPlayerDied;
  }

  private void OnPlayerDied(object sender, EventArgs e)
  {
    audioSource.PlayOneShot(onPlayerDiedSound);
  }

  private void OnObstaclePassed(object sender, int e)
  {
    audioSource.PlayOneShot(obstaclePassedSound);
  }

  private void OnClick(object sender, bool e)
  {
    if (e) audioSource.PlayOneShot(flipSound);
  }
}
