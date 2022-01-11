using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Awake()
    {
        allowMovement = false;
        GameController.OnMinigolfTurn += StopMovement;
        GameController.OnPlayerMovementTurn += ResumeMovement;
        GameController.OnPlayerDead += StopMovement;
        GameController.OnPlayerWin += StopMovement;
    }
    void Start()
    {
        //playerSpawned();
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

        //move player in forward vector direction
        if (allowMovement){
            bool LMouse=Input.GetMouseButton(0);
            if (LMouse||Input.GetKey ("w")||Input.GetKey ("a")||Input.GetKey ("s")||Input.GetKey ("d")||Input.GetKeyDown(KeyCode.Space) ){
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
                //turn direction of player-forward-vector
                if(Input.GetKey ("w") ){
                    keyDir+=Vector3.left;
                }
                if(Input.GetKey ("a") ){
                    keyDir+=Vector3.back;
                }
                if(Input.GetKey ("s") ){
                    keyDir+=Vector3.right;
                }
                if(Input.GetKey ("d") ){    //|| keyDir==Vector3.zero){ //prevents Nullvector, but blocks opposite dir
                    keyDir+=Vector3.forward;
                }
                if(keyDir!=null){// keyDir+=Vector3.forward;
                keyDir.Normalize();
                targetRotation.SetLookRotation(keyDir );        //set target quaternion to Vector3 direction
                targetRotation *= Quaternion.Euler(0,-45,0);    //Rotation Offset: -45° to be in screen direction
                }
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
            //fixed time inputs always accessible:
            if (0==jumpstep && Input.GetKeyDown(KeyCode.Space) ){
                jumpstep=1;
            }
            if(0 < jumpstep && jumpstep < jumpheight){
                transform.position += Time.deltaTime * speed * transform.up;
                jumpstep++;
            }
            else{
                jumpstep=0;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime); //spherical interpolate (rotation steps)
        }
    }
    private int jumpstep;
    const int jumpheight=20;

    public void StopMovement()
    {
        allowMovement = false;
        GameController.Instance.PlayerStop();
    }

    public void ResumeMovement()
    {
        allowMovement = true;
    }

    public void playerSpawned()
    {
        allowMovement = true;
        if(!ballDeployed){
            GameController.Instance.DeployBall(ballTransform);
            ballDeployed=true;
        }
    }

    [SerializeField]
    bool ballDeployed=false;

    [SerializeField]
    private Transform ballTransform;
    
    [SerializeField]
    Vector3 keyDir=Vector3.forward;
    
    [SerializeField]
    private bool allowMovement;

    [SerializeField]
    private bool keyControl=false;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float speedRotation;

}
