using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

  [SerializeField]
  private Rigidbody eggRigid;

  private void OnCollisionEnter(Collision collision)
  {
    GameController.Instance.BallHit();
  }

  private void Awake()
  {
    eggRigid = GetComponent<Rigidbody>();
  }

  public void MoveBall(Vector3 forceDirection)
  {
    eggRigid.AddForce(forceDirection);
  }

  public void StopMovement()
  {
    eggRigid.velocity        = Vector3.zero;
    eggRigid.angularVelocity = Vector3.zero;
  }
}
