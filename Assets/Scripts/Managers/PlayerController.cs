using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Awake()
    {
        allowMovement = false;
    }
    void Start()
    {
        GameController.OnMinigolfTurn += StopMovement;
        GameController.OnPlayerMovementTurn += ResumeMovement;
        GameController.OnPlayerDead += StopMovement;
        GameController.OnPlayerWin += StopMovement;
    }

    void OnDestroy()
    {
        GameController.OnMinigolfTurn -= StopMovement;
        GameController.OnPlayerMovementTurn -= ResumeMovement;
        GameController.OnPlayerDead -= StopMovement;
        GameController.OnPlayerWin -= StopMovement;
    }



    // public float panSpeed = 20f;
    void Update(){

        if (transform.position.y < -1)
        {
            GameController.Instance.PlayerDead();
        }
/*
        if (Input.GetKeyDown(KeyCode.Space) ){
        }

        Input.GetKey(KeyCode.Space);

        Input.GetKeyDown(KeyCode.Space);
        Input.GetKeyUp(KeyCode.Space);
  */      
        if (allowMovement){
            //move player while left mousebutton down
            bool LMouse=Input.GetMouseButton(0);
            if (LMouse||Input.GetKey ("w")||Input.GetKey ("a")||Input.GetKey ("s")||Input.GetKey ("d") ){
                if (LMouse){
                    keyControl=false;
                }
                else{
                    keyControl=true;
                }
                transform.position += Time.deltaTime * speed * transform.forward;   //increase fwd
                GameController.Instance.PlayerRun();
            }
            else{
                GameController.Instance.PlayerStop();
            }
        }
    }

    void FixedUpdate()      //fixed time of physics: 0.02 seconds (50 calls per second) is the default
    {
        if (allowMovement)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.position);
            if(keyControl){
                if(Input.GetKey ("w") ){
                    keyDir+=Vector3.left;
                }
                if(Input.GetKey ("a") ){
                    keyDir+=Vector3.back;
                }
                if(Input.GetKey ("s") ){
                    keyDir+=Vector3.right;
                }
                if(Input.GetKey ("d") ){
                    keyDir+=Vector3.forward;
                }
                keyDir.Normalize();
                targetRotation.SetLookRotation(keyDir );
                targetRotation *= Quaternion.Euler(0,-45,0);    //Rotation Offset: -45° to be in screen direction
            }
            else{
                //raycast cursor for looking direction
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float cursordist = 0;

                if (playerPlane.Raycast(ray, out cursordist))      // sets enter to the distance along the ray, where it intersects the plane, or zero
                {
                    Vector3 targetPoint = ray.GetPoint(cursordist);
                    targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                }

            }
            //spherical interpolate (rotation steps):
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }
    }

    public void StopMovement()
    {
        allowMovement = false;
        //GameController.Instance.PlayerStop();
    }

    public void ResumeMovement()
    {
        allowMovement = true;
    }
    public void StartMovement()
    {
        allowMovement = true;
        GameController.Instance.BallReady(ballTransform);
    }


    [SerializeField]
    Vector3 keyDir=Vector3.forward;
    
    private bool allowMovement;

    [SerializeField]
    private bool keyControl=false;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float speedRotation;

    [SerializeField]
    private Transform ballTransform;
}
