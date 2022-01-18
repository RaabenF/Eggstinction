using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggLayed : MonoBehaviour
{
    //called when player spawned animation is complete
    public void SaveEgg()
    {
        gameObject.GetComponentInParent<PlayerController>().playerSpawned();
    }
    
}
