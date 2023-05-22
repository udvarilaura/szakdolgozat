using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDetection : MonoBehaviour
{

    public bool amIDetected = false;
   
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "Vision")
        {
            amIDetected = true;
            Debug.Log("I got detected");
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "Vision")
        {
            amIDetected = false;
            Debug.Log("Am no longer detected");

        }
    }

}
