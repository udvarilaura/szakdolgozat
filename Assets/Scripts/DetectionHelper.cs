using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHelper : MonoBehaviour
{
    [SerializeField] List<GameObject> CheckPoints;
    public int CurrentCheckedCheckPoints; //The number of the CheckPoint children of this object that are triggered by a camera vision

    void Update()
    {
        CurrentCheckedCheckPoints = getNumberOfDetectedCheckPoints();
        
        //Test
        Debug.Log("currentCheckedCheckPoints: "+ CurrentCheckedCheckPoints);
    }

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
}
