﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : Singleton<GameController>
{
  public static event Action OnStartGame = delegate { };
  public static event Action OnBallReady = delegate { };
  public static event Action OnPlayerRunning = delegate { };
  public static event Action OnPlayerStop = delegate { };
  public static event Action OnPlayerMovementTurn = delegate { };
  public static event Action OnMinigolfTurn = delegate { };
  public static event Action OnEndGame = delegate { };
  public static event Action OnBallShot = delegate { };
  public static event Action OnStopBall = delegate { };
  public static event Action OnResetBallPosition = delegate { };
  public static event Action OnPlayerDead = delegate { };
  

  // Sounds events

  public static event Action OnCharacterLava = delegate { };
  public static event Action OnBallLava = delegate { };
  public static event Action OnBallFall = delegate { };
  public static event Action OnBallHit = delegate { };
  public static event Action OnPlayerWin = delegate { };
  public static event Action OnButtonClicked = delegate { };

  protected new void Awake()
  {
    base.Awake();
  }

  void Start()
  {
    if(_levelsList == null)
    {
      _levelsList = new List<int>();
      _levelsList.Add(0);
      for(int i = 1; i < _gameSettings.Levels.Count; i++)
      {
        _levelsList.Insert(UnityEngine.Random.Range(1,_levelsList.Count), i);
        Debug.Log(i);
      }
      string asd = "";
      foreach(int i in _levelsList)
      {
        asd = asd + i + ",";
      }
      Debug.Log(asd);
    }
    StartGame();
  }

  private static List<int> _levelsList = null;

  public void StartGame()
  {
    if (_tutorialPlayed)
    {
      if (_levelWon)
      {
        _levelToPlay++;
        if(_levelToPlay >= _levelsList.Count)
        {
          _levelToPlay = 1;
        }
        _levelWon = false;
      }
    }
    else
    {
      _levelToPlay = 0;   //0=tutorial
      _tutorialPlayed = true;
    }
    Debug.Log("Loading level " + _levelsList[_levelToPlay]);
    //GameSettings.cs and MapGen loads maptiles from .txt file
    _mapGenerator.Init(_gameSettings.Levels[_levelsList[_levelToPlay]], _gameSettings.DestructionTime);
    OnStartGame();
  }

  public static bool _tutorialPlayed = false;
  public static bool _levelWon = false;
  public static int _levelToPlay=0;

  public void Ready2shoot() //PlayerController playerController)
  {
    if (player == null)
    {
      player = GetComponentInChildren<PlayerController>();
    }
    StopBall();
    MinigolfTurn();
    ball = GameObject.FindGameObjectWithTag("Ball");
    ball.GetComponentInChildren<BallInput>().preparedToPlayGolf = true;
  }

  public void DeployBall(Vector3 playerpos)
  {
    playerpos.z -=0.8f;
    if(ball==null)_mapGenerator.InstantiateBall(playerpos);
    OnBallReady();
    //Info: BallTrigger -instanciates-> Ballinput
  }

    [SerializeField]
    private GameObject _ballPrefab;

  public void PlayerMovementTurn()
  {
    OnPlayerMovementTurn();
  }

  public void PlayerRun()
  {
    OnPlayerRunning();
  }

  public void PlayerStop()
  {
    OnPlayerStop();
  }

  public void MinigolfTurn()
  {
    OnMinigolfTurn();
  }

  public void BallShot()
  {
    OnBallShot();
  }
  public void StopBall(){
    OnStopBall();
  }

  public void ResetBallPosition()
  {
    OnResetBallPosition();
  }

  public void EndGame()
  {
    OnEndGame();
  }

  public void PlayerDead()
  {
    OnPlayerDead();
  }

  public void CharacterLava()
  {
    OnCharacterLava();
  }

  public void BallLava()
  {
    OnBallLava();
  }

  public void BallFall()
  {
    OnBallFall();
  }

  public void BallHit()
  {
    OnBallHit();
  }

  public void PlayerWin()
  {
    _levelWon = true;
    OnPlayerWin();
  }

  public void ButtonClick()
  {
    OnButtonClicked();
  }

  //[SerializeField]
  public PlayerController player;

  //[SerializeField]
  public GameObject ball;

  [SerializeField]
  private MapGenerator _mapGenerator;

  [SerializeField]
  private GameSettings _gameSettings;

}
