using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using System.Linq;
public class CamPlacerAgent : Agent
{
    /// <summary><c>CameraPlaceholders</c> is the list of the CameraPlaceholder objects the agent can place the cameras on</summary>
    [SerializeField] public List<GameObject> CameraPlaceholders; //List of the camera placeholders the agent can place objects to
    [SerializeField] public GameObject CameraToPlace; //The camera prefab //Note: it does not have a tag set. The copies will get tags only. 
    //[SerializeField] public Transform CheckpointTransform; //Checkpoint
    [SerializeField] public Transform CheckpointParent;
    [SerializeField] public GameObject CameraParent;
    [SerializeField] public int MAXSTEP;

    private List<GameObject> CameraList;
    //private List<int> ActionList; //For storing the actions the agent used in the current episode
    private CamInstantiate CamInstantiate; //script we import the CameraInstantiate function from


    private List<int> UsedCamPlaceholders;
       
    //private CameraDetection CameraDetection;

public override void OnEpisodeBegin()
    {

        moveCheckpointParentUp();

        //GameObject ThisParent = transform.parent.gameObject;
        //Debug.Log("The parent of the current agent is: "+ThisParent.name);

        //Looking for the objects with Camera tag
        GameObject[] CamGameObjects = GameObject.FindGameObjectsWithTag("Camera");

        foreach (GameObject CamGameObject in CamGameObjects)
        {
            //Debug.Log("There was an object to delete in the 29th line, and it's name iiiiis: " + CamGameObject.name +", no, not John Cena");
            Destroy(CamGameObject);
        }
        //Debug.Log("32th row: The length of the array CamGameObjects after being deleted: " + CamGameObjects.Length);


        //Checking if the camera list contains anything, should only be 0 for the first time
        if (CameraList == null)
        {
            Debug.Log("The camera list is empty");
        }
        CameraList = new List<GameObject>();


        CamInstantiate = new CamInstantiate();

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


        GameObject ThisParent = CameraParent;
        //TEST
        //Debug.Log("The parent of the current agent is: "+ThisParent.name);

        //////////////////////USE THIS LATER
        //ParentOfAgent = this.transform.parent.gameObject;
        //Debug.Log("ParentOfAgent" + ParentOfAgent);

        Transform[] AllChildren = ThisParent.GetComponentsInChildren<Transform>();

        //foreach (Transform child in AllChildren) { Debug.Log("CamChild (letevos 74.sor)" + child); }


        int action = actions.DiscreteActions[0];

        //Debug.Log("action we took: " + action);

        //Az action olyan -e, amit az ágens már használt? 
        //Array az OnEpisodeBeginhez -> amit haszálunk (int array), beletesszünk

        GameObject SelectedPlaceholder = CameraPlaceholders[action];
        UsedCamPlaceholders.Add(CameraPlaceholders.IndexOf(SelectedPlaceholder));

        //Debug: 
        //for (int i = 0; i < UsedCamPlaceholders.Count; i++)
        //    Debug.Log(UsedCamPlaceholders[i]);

        //Debug.Log("The current Placeholder is " + SelectedPlaceholder);
        //Debug.Log("The number of selected Placeholders is " + UsedCamPlaceholders.Count);
        
        GameObject Camera = CamInstantiate.CameraInstantiate(CameraToPlace, SelectedPlaceholder.transform);

        Camera.tag = "Camera";
        Camera.transform.parent = CameraParent.transform;

        // CameraList: beledobáljuk a lehelyezett kamerákat /OnEpisodeBegin-nél kitakarítjuk
        CameraList.Add(Camera);

        if (UsedCamPlaceholders.Count == MAXSTEP)
        {
            bool isUnique = isUniqueList(UsedCamPlaceholders);
            //Debug.Log("Egyedi kiválasztás: "+ isUnique);
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


//IMPORTANT 
// What we know rn:
// The cameras we place are not placed yet > we can't really refer to them
// We can not get their components this way either > question: how can we get a component of an object yet to be created? 
// Once created, we need a function to check all cameras


