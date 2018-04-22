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
        StartGame();
    }

    public void StartGame()
    {
        _mapGenerator.Init(_gameSettings.Levels[0], _gameSettings.DestructionTime);
        OnStartGame();
    }

    public void PlayerMovementTurn()
    {
        OnPlayerMovementTurn();
    }

    public void BallReady(Transform ballPosition)
    {
        _mapGenerator.InstantiateBall(ballPosition.position);
        OnBallReady();
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
        OnPlayerWin();
    }

    public void ButtonClick()
    {
        OnButtonClicked();
    }

    [SerializeField]
    private MapGenerator _mapGenerator;

    [SerializeField]
    private GameSettings _gameSettings;
}
