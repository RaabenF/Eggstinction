using UnityEngine;
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
    //if (cameraTarget != null)
    //if(GameController.Instance.ball == null){
    if(GameObject.Find("goal") != null){
      finalPosition = GameObject.Find("Goal").transform.position;
        Debug.LogWarning("2");
      finalPosition = Vector3.Cross(player.transform.position, finalPosition);
    }
    else{
        //finalPosition = Vector3.Lerp(player.transform.position, GameController.Instance.ball.transform.position, 0.5f);
        //finalPosition = GameController.Instance.ball.transform.position;//GameObject.FindGameObjectWithTag("Goal").transform.position;
        if(null != finalPosition) finalPosition = Vector3.Lerp(player.transform.position, finalPosition, 0.5f) + camOffset;
        finalPosition = cameraTarget.transform.position + camOffset;
    }
    transform.position = finalPosition;
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

  private Vector3 camOffset = new Vector3(10.0f, 8.5f, 10.0f);
  private GameObject player;
  private GameObject cameraTarget;

  private Vector3 finalPosition;

  new Camera camera;

  private float zoomOffset;

}