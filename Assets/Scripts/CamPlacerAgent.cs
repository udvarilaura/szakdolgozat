using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using System.Linq;
//using System.Threading;
public class CamPlacerAgent : Agent
{
    [SerializeField] public List<GameObject> CameraPlaceholders; //List of the camera placeholders the agent can place objects to
    [SerializeField] public GameObject CameraToPlace; //The camera prefab //Note: it does not have a tag set. The copies will get tags only. 
    [SerializeField] public Transform CheckpointParent; // The container parent of the CheckPoint objects. Stores the *important* value of the currently detected checkpoints
    [SerializeField] public GameObject CameraParent; //The parent of the camera objects we add after we place the cameras
    [SerializeField] public int MAXSTEP; // Should be the same as the Max Step of the agent. 

    private List<GameObject> CameraList; // Storing the currently added cameras
    private CamInstantiate CamInstantiate; //script we import the CameraInstantiate function from
    //private CheckPointMover CheckPointMover;
    private List<int> UsedCamPlaceholders;
    private int NumberOfCheckPoints;

public override void OnEpisodeBegin()
    {

        //Debug.Log("In the beginning of the episode - should be 0, 0, 0" + CheckpointParent.transform.position.ToString());


        CamInstantiate = gameObject.AddComponent<CamInstantiate>(); /// !!!!!
        //CheckPointMover = gameObject.AddComponent<CheckPointMover>(); /// !!!!!
        NumberOfCheckPoints = CheckpointParent.GetComponent<DetectionHelper>().getNumberOfCheckPoints();

        CheckpointParent.GetComponent<DetectionHelper>().NullChildren();

        //CheckPointMover.MoveObjectUp(CheckpointParent, 10);

        //Looking for the objects with Camera tag --- This is surely looking for cameras all over the world, check later
        GameObject[] CamGameObjects = GameObject.FindGameObjectsWithTag("Camera");

        foreach (GameObject CamGameObject in CamGameObjects)
        {
            //Debug.Log("There was an object to delete in the 29th line, and it's name iiiiis: " + CamGameObject.name +", no, not John Cena");
            Destroy(CamGameObject);
        }
        //Test
        //Debug.Log("43th row: The length of the array CamGameObjects after being deleted: " + CamGameObjects.Length);

        //Test
        //Checking if the camera list contains anything, should only be 0 for the first time
        //if (CameraList == null)
        //{
        //    Debug.Log("The camera list is empty");
        //}
        CameraList = new List<GameObject>();

        UsedCamPlaceholders = new List<int>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        //We check the CamPlaceholder positions to see where we can put the cameras
        foreach (GameObject CameraPlaceholder in CameraPlaceholders)
        {
            sensor.AddObservation(CameraPlaceholder.transform.localPosition.x);
            sensor.AddObservation(CameraPlaceholder.transform.localPosition.y);
            sensor.AddObservation(CameraPlaceholder.transform.localPosition.z);
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int CurrentCheckedCheckPoints = CheckpointParent.GetComponent<DetectionHelper>().getNumberOfDetectedCheckPoints();

        //Debug.Log("Agent: currentCheckedCheckPoints: " + CurrentCheckedCheckPoints);

        int action = actions.DiscreteActions[0];

        //Test:
        //Debug.Log("action we took: " + action);

        GameObject SelectedPlaceholder = CameraPlaceholders[action];
        //UsedCamPlaceholders.Add(CameraPlaceholders.IndexOf(SelectedPlaceholder));
        //check if SelectedPlaceholder is in UsedCamPlaceholders, negative reward & end EndEpisode

        UsedCamPlaceholders.Add(action);

        //Tests: 
        //for (int i = 0; i < UsedCamPlaceholders.Count; i++)
        //    Debug.Log(UsedCamPlaceholders[i]);

        //Debug.Log("The current Placeholder is " + SelectedPlaceholder);
        //Debug.Log("The number of selected Placeholders is " + UsedCamPlaceholders.Count);
        
        GameObject Camera = CamInstantiate.CameraInstantiate(CameraToPlace, SelectedPlaceholder.transform);

        Camera.tag = "Camera";
        Camera.transform.parent = CameraParent.transform;

        CameraList.Add(Camera);


        if (UsedCamPlaceholders.Count == MAXSTEP)
        {
            //CheckPointMover.MoveObjectDown(CheckpointParent, 10);
            bool isUnique = isUniqueList(UsedCamPlaceholders);
            //Debug.Log("Are the elements of the UsedCamPlaceholders list unique: " + isUnique);
            //Debug.Log("CurrentCheckedCheckPoints " + CurrentCheckedCheckPoints);
            //Debug.Log("NumberOfCheckPoints " + NumberOfCheckPoints);
            SetReward(RewardCalculation(CurrentCheckedCheckPoints, NumberOfCheckPoints));
            EndEpisode();  
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = 0;

    }

    public bool isUniqueList<T>(List<T> list)
    {
        return list.Distinct().Count() == list.Count();
    }

    
    public float RewardCalculation(int CurrentCheckedCheckPoints, int numberOfCheckPointsInEnvironment)
    {
        return (CurrentCheckedCheckPoints / numberOfCheckPointsInEnvironment);  //(0-1)
    }
    
}

