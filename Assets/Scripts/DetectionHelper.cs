using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHelper : MonoBehaviour
{
    public int CurrentCheckedCheckPoints; //The number of the CheckPoint children of this object that are triggered by a camera vision
    public List<GameObject> CheckPoints = new();

    private void Start()
    {

        // Get all the children of the parent object
        Debug.Log("DetectionHelper ran Start method");
        if (CheckPoints.Count == 0) { 
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                CheckPoints.Add(child);
                //Debug.Log("child: "+child);
            }
        }
        else
        {
            Debug.Log("Why are you running");
        }

        Debug.Log("Detection Helper - childCount: " +transform.childCount);
    }

    void FixedUpdate()
    {

        //Debug.Log("DetectionHelper GameObject Name: " + gameObject.name);

        CurrentCheckedCheckPoints = getNumberOfDetectedCheckPoints();

        //Test
        Debug.Log("DetectionHelper: currentCheckedCheckPoints: " + CurrentCheckedCheckPoints);
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

