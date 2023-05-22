using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using System.Linq;
public class CamPlacerAgent : Agent
{
    [SerializeField] public List<GameObject> CameraPlaceholders; //List of the camera placeholders the agent can place objects to
    [SerializeField] public GameObject CameraToPlace; //The camera prefab //Note: it does not have a tag set. The copies will get tags only. 
    [SerializeField] public Transform CheckpointParent; // The container parent of the CheckPoint objects. Stores the *important* value of the currently detected checkpoints
    [SerializeField] public GameObject CameraParent; //The parent of the camera objects we add after we place the cameras
    [SerializeField] public int MAXSTEP; // Should be the same as the Max Step of the agent. 

    private List<GameObject> CameraList; // Storing the currently added cameras
    private CamInstantiate CamInstantiate; //script we import the CameraInstantiate function from
    private List<int> UsedCamPlaceholders;

public override void OnEpisodeBegin()
    {

        moveCheckpointParentUp();

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


        CamInstantiate = gameObject.AddComponent<CamInstantiate>();

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

        //We also check the transform positions of the checkpoint in the scene
        //sensor.AddObservation(CheckpointTransform.localPosition.y);
        //sensor.AddObservation(CheckpointTransform.localPosition.x);
        //sensor.AddObservation(CheckpointTransform.localPosition.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        int CurrentCheckedCheckPoints = CheckpointParent.GetComponent<DetectionHelper>().CurrentCheckedCheckPoints;


        int action = actions.DiscreteActions[0];

        //Test:
        //Debug.Log("action we took: " + action);

        GameObject SelectedPlaceholder = CameraPlaceholders[action];
        UsedCamPlaceholders.Add(CameraPlaceholders.IndexOf(SelectedPlaceholder));

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
            bool isUnique = isUniqueList(UsedCamPlaceholders);
            moveCheckpointParentDown();
            //Debug.Log("Are the elements of the UsedCamPlaceholders list unique: "+ isUnique);
        }

    }

    /* private void OnTriggerEnter(Collider other) {
        
        if (CheckListOfCameraDetections() > 0)
        {
            SetReward(+0.2f);
        }
        else
        {
            SetReward(-0.2f);
        }
    } */

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = 0;

    }

    public void moveCheckpointParentUp()
    {
        CheckpointParent.transform.position = transform.position + new Vector3(0, 10, 0);

    }

    public void moveCheckpointParentDown()
    {
        CheckpointParent.transform.position = transform.position + new Vector3(0, -10, 0);

    }

    public bool isUniqueList<T>(List<T> list)
    {
        return list.Distinct().Count() == list.Count();
    }
}

