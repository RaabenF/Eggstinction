﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUIController : MonoBehaviour
{
  void Awake()
  {
    retryButton.onClick.AddListener(OnRetryButtonClick);
    nextButton.onClick.AddListener(OnNextButtonClick);
    menuButton.onClick.AddListener(OnMenuButtonClick);
    GameOverPanel.SetActive(false);
  }

  void Start()
  {
    GameController.OnEndGame += ShowGameOver;
    GameController.OnPlayerWin += ShowWin;
  }
  
  private void OnNextButtonClick()
  {
    GameController._levelWon = true;
    GameController.Instance.ButtonClick();
    ReloadScene();
  }

  private void OnRetryButtonClick()
  {
    GameController.Instance.ButtonClick();
    ReloadScene();
  }

  private void OnMenuButtonClick()
  {
    GameController.Instance.ButtonClick();
    UnityEngine.SceneManagement.SceneManager.LoadScene(0);  //scene 0 = main menu scene
  }

  private void ShowGameOver()
  {
    GameOverPanel.SetActive(true);
    winText.SetActive(false);
    loseText.SetActive(true);
    retryButton.gameObject.SetActive(true);
  }

  private void ShowWin()
  {
    GameOverPanel.SetActive(true);
    winText.SetActive(true);
    loseText.SetActive(false);
    retryButton.gameObject.SetActive(false);
  }

  private void ReloadScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  void OnDestroy()
  {
    retryButton.onClick.RemoveListener(OnRetryButtonClick);
    nextButton.onClick.RemoveListener(OnNextButtonClick);
    menuButton.onClick.RemoveListener(OnMenuButtonClick);
    GameController.OnEndGame -= ShowGameOver;
    GameController.OnPlayerWin -= ShowWin;
  }

  [SerializeField]
  private GameObject winText;
  [SerializeField]
  private GameObject loseText;
  [SerializeField]
  private Button retryButton;
  [SerializeField]
  private Button nextButton;
  [SerializeField]
  private Button menuButton;


  [SerializeField]
  private GameObject GameOverPanel;

}
