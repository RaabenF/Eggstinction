﻿using UnityEngine;
using System.Collections;

public class CameraController : Singleton<CameraController>
{
  void Start()
  {
      GameController.OnPlayerMovementTurn += ZoomIn;
      GameController.OnMinigolfTurn += ZoomOut;
      GameController.OnResetBallPosition += ResetPosition;

      camera = GetComponent<Camera>();
      LookForPlayer();
      LookForBall();

      camera.orthographicSize = minZoom;

      zoomOffset = normalZoom - minZoom;
  }

  void OnDestroy()
  {
    GameController.OnPlayerMovementTurn -= ZoomIn;
    GameController.OnMinigolfTurn -= ZoomOut;
    GameController.OnResetBallPosition -= ResetPosition;
    MapGenerator.OnPlayerSpawned -= LookForPlayer;
    GameController.OnBallReady -= LookForBall;
  }

  private void LookForPlayer()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    if(player == null)
    {
      MapGenerator.OnPlayerSpawned += LookForPlayer;
      return;
    }
    else
    {
      FollowCharacter();
    }
  }

  private void LookForBall()
  {
    if(GameController.Instance.ball == null)
    {
      GameController.OnBallReady += LookForBall;
    }
  }

  void Update()
  {
    if (cameraTarget != null)
    {
        finalPosition = cameraTarget.transform.position + new Vector3(10.0f, 8.5f, 10.0f);
        transform.position = finalPosition;
    }
  }

  void ResetPosition()
  {
    FollowCharacter();
  }


  public void FollowCharacter()
  {
      Time.timeScale = 1f;
      cameraTarget = player;
  }

  public void FollowBall()
  {
      if (cameraTarget != GameController.Instance.ball)
      {
          cameraTarget = GameController.Instance.ball;
          Time.timeScale = 0.5f;
      }
  }

  private void ZoomIn()
  {
      StopAllCoroutines();
      if (camera.orthographicSize != minZoom)
      {
          StartCoroutine(ZoomInCamera(zoomOffset));
      }
  }

  private void ZoomOut()
  {
      StopAllCoroutines();
      if (camera.orthographicSize != normalZoom)
      {
          StartCoroutine(ZoomOutCamera(zoomOffset));
      }
  }

  private void StopZoom()
  {
      StopAllCoroutines();
  }

  private IEnumerator ZoomInCamera(float zoom)
  {
      float time = 0.0f;
      float cameraZoom = camera.orthographicSize;

      while (time < timeTransition)
      {
          camera.orthographicSize = cameraZoom - zoom * zoomCurve.Evaluate(time / timeTransition);
          time += Time.deltaTime;
          yield return null;
      }
      camera.orthographicSize = minZoom;
  }

  private IEnumerator ZoomOutCamera(float zoom)
  {
      float time = 0.0f;
      float cameraZoom = camera.orthographicSize;

      while (time < timeTransition)
      {
          camera.orthographicSize = cameraZoom + zoom * zoomCurve.Evaluate(time / timeTransition);
          time += Time.deltaTime;
          yield return null;
      }
      camera.orthographicSize = normalZoom;
  }

  [SerializeField]
  private AnimationCurve zoomCurve;
  [SerializeField]
  private float minZoom;
  [SerializeField]
  private float normalZoom;
  [SerializeField]
  private float timeTransition;

  private GameObject player;
  //private GameObject ball;
  private GameObject cameraTarget;

  private Vector3 finalPosition;

  new Camera camera;

  private float zoomOffset;

}