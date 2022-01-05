using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    private float velocityMagnitudeWin = 1.5f;

    [SerializeField]
    private Rigidbody eggRigid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && eggRigid == null)
        {
            eggRigid = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (eggRigid.velocity.magnitude < velocityMagnitudeWin)
            {
                if (!_enterInHole)
                {
                    _enterInHole = true;
                    GameController.Instance.PlayerWin();
                }
                eggRigid.useGravity = false;
                eggRigid.transform.position = Vector3.Lerp(eggRigid.transform.position, _ballEndTransform.position, 5f * Time.deltaTime);
            }
        }
    }

    [SerializeField]
    private Transform _ballEndTransform;

    private bool _enterInHole = false;
}
