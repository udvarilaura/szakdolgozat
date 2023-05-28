using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDetection : MonoBehaviour
{

    public bool amIDetected = false;

    void FixedUpdate()
    {
        Debug.Log("Checkpoint position" + transform.position.ToString());
    }

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Kiskutya");
        if (coll.gameObject.name == "Vision")
        {
            amIDetected = true;
            Debug.Log("CheckpointDetection : I got detected");
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "Vision")
        {
            amIDetected = false;
            Debug.Log("CheckpointDetection : Am no longer detected");

        }
    }
    

}
