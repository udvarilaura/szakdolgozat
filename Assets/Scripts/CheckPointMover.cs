using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMover : MonoBehaviour
{
    // Start is called before the first frame update
    public void MoveObjectUp(Transform objectToMove, float height)
    {
        //objectToMove.transform.position = transform.position + new Vector3(0, 10, 0);

        objectToMove.Translate(new Vector3(0, height, 0));

        //Debug.Log("After moving up" + objectToMove.transform.position.ToString());

    }

    public void MoveObjectDown(Transform objectToMove, float height)
    {
        //Debug.Log("Before moving down" + CheckpointParent.transform.position.ToString());

        //objectToMove.transform.position = transform.position + new Vector3(0, -10, 0);

        objectToMove.Translate(new Vector3(0, -height, 0));


        //TEST
        //Debug.Log("I should have moved the CheckpointParent down but I'm not clever enough, duhh");

        //Debug.Log("After moving down" + objectToMove.transform.position.ToString());

    }
}
