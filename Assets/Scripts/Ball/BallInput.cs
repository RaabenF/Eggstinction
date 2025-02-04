﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInput : MonoBehaviour {

  [SerializeField]
  private float minCharge = 0;
  [SerializeField]
  private float maxCharge = 500;
  [SerializeField]
  private float chargeRate = 0.1f;
  [SerializeField]
  private ArrowSprite arrowSprite;

  private Vector3 lastPosition;

  bool mouseDown;
  bool fallSoundTriggered = false;
  bool endGame = false;

  Vector3 finalDirection;

  private Vector3 finalHitPoint;
  float currentCharge;

  private Plane plane = new Plane(Vector3.up, Vector3.zero);

  void OnEnable()
  {
    GameController.OnResetBallPosition += OnResetLastPosition;
  }

  void OnDisable()
  {
    GameController.OnResetBallPosition -= OnResetLastPosition;
  }

  public bool preparedToPlayGolf;


  // Update is called once per frame
  void Update()
  {
    if (!mouseDown && preparedToPlayGolf && Input.GetMouseButtonDown(0))
    {
          arrowSprite.EnableArrow(true);
          preparedToPlayGolf = false;
        mouseDown = true;
        currentCharge = 0;
    }

    if (mouseDown)
    {
      getDirection();
      chargeShot();

      if (Input.GetMouseButtonUp(0))
      {
        GameController.Instance.BallShot();
        mouseDown = false;
        StartCoroutine(waitToApplyForce());
        StartCoroutine(waitToReturnPlayerControl());
      }
    }

    if (!endGame && transform.position.y < -5)
    {
      fallSoundTriggered = false;
      GameController.Instance.ResetBallPosition();
    }
    else if (!endGame && transform.position.y < -1.5)
    {
      if (!fallSoundTriggered)
      {
        fallSoundTriggered = true;
        GameController.Instance.BallFall();
      }
      CameraController.Instance.FollowBall();
    }
  }

  private void getDirection() {

    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    plane.SetNormalAndPosition(plane.normal, new Vector3(0f,transform.position.y, 0f));
    float enter;
    if (plane.Raycast(ray, out enter))
    {
      finalHitPoint = ray.GetPoint(enter);
      finalDirection = finalHitPoint - transform.position;
      finalDirection = Vector3.Normalize(finalDirection);
      finalDirection.y = 0;
      arrowSprite.RotateArrow(finalDirection);
    }
  }

  private void chargeShot()
  {
    lastPosition = transform.position;  //from Ready2Shoot, now in GameCtrl
    currentCharge = (finalHitPoint - transform.position).magnitude;
    currentCharge = currentCharge * chargeRate;
    currentCharge = Mathf.Clamp(currentCharge, minCharge, maxCharge);
    arrowSprite.ScaleArrow(currentCharge, minCharge, maxCharge);
  }

  private IEnumerator waitToApplyForce()
  {
    yield return new WaitForSeconds(0.5f);
    GetComponent<BallMovement>().MoveBall(-finalDirection * currentCharge);
  }

  private IEnumerator waitToReturnPlayerControl()
  {
    yield return new WaitForSeconds(1f);
    GameController.Instance.PlayerMovementTurn();
  }

  private void OnDrawGizmos()
  {
    if (mouseDown)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawRay(transform.position, -finalDirection);
    }
    Gizmos.DrawRay(transform.position, Vector3.down);
  }

  private void OnResetLastPosition()
  {
    transform.position = lastPosition;
    RaycastHit hit;
    if (!Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
    {
      GameController.Instance.PlayerMovementTurn();
      GameController.Instance.PlayerDead();
      lastPosition.y = -100;
      transform.position = lastPosition;
      endGame = true;
      StartCoroutine(CallEndOfGame());
    }
    else
    {
      transform.position = lastPosition;
    }
  }

  private IEnumerator CallEndOfGame()
  {
    yield return new WaitForSeconds(2f);
    GameController.Instance.EndGame();
  }
}
