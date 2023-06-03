using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDetection : MonoBehaviour
{

    public bool amIDetected = false;
    public LayerMask checkLayerMask;

    void FixedUpdate()
    {
        //Debug.Log("Checkpoint position" + transform.position.ToString());

        MyCollisions();
    }

    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, checkLayerMask);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            //Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;

            //Debug.Log("Number of hit colliders : " + hitColliders.Length);
        }
        if(hitColliders.Length > 0)
        {
            amIDetected = true;
        }
        else
        {
            amIDetected = false;
        }
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    /*private void OnTriggerStay(Collider coll)
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
    }*/
    

}
