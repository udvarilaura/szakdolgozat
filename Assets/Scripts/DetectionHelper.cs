using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHelper : MonoBehaviour
{
    [SerializeField] List<GameObject> CheckPoints;
    public int CurrentCheckedCheckPoints; //The number of the CheckPoint children of this object that are triggered by a camera vision

    /*void FixedUpdate()
    {
        CurrentCheckedCheckPoints = getNumberOfDetectedCheckPoints();
        
        //Test
        Debug.Log("DetectionHelper: currentCheckedCheckPoints: " + CurrentCheckedCheckPoints);
    }*/

    public int getNumberOfDetectedCheckPoints()
    {
        int checkedCheckPoints = 0;
        
        foreach (GameObject checkPoint in CheckPoints)
        {
            bool isDetected = checkPoint.GetComponent<CheckpointDetection>().amIDetected;
            if (isDetected == true)
            {
                checkedCheckPoints += 1;
            }
        }

        return checkedCheckPoints;
    }

    public int getNumberOfCheckPoints()
    {
        return CheckPoints.Count;
    }

    public void NullChildren()
    {
        foreach (GameObject checkPoint in CheckPoints)
        {
            checkPoint.GetComponent<CheckpointDetection>().amIDetected = false;
        }

    }
}

