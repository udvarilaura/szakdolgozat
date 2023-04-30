using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
public class CamPlacerAgent : Agent
{
    /// <summary><c>CameraPlaceholders</c> is the list of the CameraPlaceholder objects the agent can place the cameras on</summary>
    [SerializeField] public List<Transform> CameraPlaceholders; //List of the camera placeholders the agent can place objects to
    [SerializeField] public GameObject CameraToPlace; //The camera prefab //Note: it does not have a tag set. The copies will get tags only. 
    [SerializeField] public Transform CheckpointTransform; //Checkpoint
    [SerializeField] public GameObject CameraParent;
    private List<GameObject> CameraList;
    //private CameraDetection CameraDetection;
    private GameObject ParentOfAgent;

    public override void OnEpisodeBegin(){
        //GameObject ThisParent = transform.parent.gameObject;
        //Debug.Log("The parent of the current agent is: "+ThisParent.name);

        //Looking for the objects with Camera tag
        GameObject[] CamGameObjects = GameObject.FindGameObjectsWithTag("Camera");
        
        foreach (GameObject CamGameObject in CamGameObjects){
            //Debug.Log("There was an object to delete in the 29th line, and it's name iiiiis: " + CamGameObject.name +", no, not John Cena");
            Destroy(CamGameObject);
        }
        //Debug.Log("32th row: The length of the array CamGameObjects after being deleted: " + CamGameObjects.Length);

        //Note: A GC automatikusan megy, de nagyon sokat eszik, egy clean method talán jobb lenne, ki kell találni
        //Checking if the camera list contains anything, should only be 0 for the first time
        if (CameraList == null){
            Debug.Log("The camera list is empty");
        } 
        CameraList = new List<GameObject>();

        //Checking how many objects with the Camera tag are still "in game"
        Debug.Log("The number of CamGameObjects: " +CamGameObjects.Length);
        
        //CheckpointTransform.localPosition=new Vector3(Random.Range(-1f, +1f), 0.375f, Random.Range(-1f, +1f));  //WORKS PERFECTLY
    } 

    public override void CollectObservations(VectorSensor sensor) {

        //We check the CamPlaceholder positions to see where we can put the cameras
        foreach (Transform CameraPlaceholder in CameraPlaceholders){
            sensor.AddObservation(CameraPlaceholder.localPosition.x);
            sensor.AddObservation(CameraPlaceholder.localPosition.y);
            sensor.AddObservation(CameraPlaceholder.localPosition.z);
        }

        //We also check the transform positions of the checkpoint in the scene
        sensor.AddObservation(CheckpointTransform.localPosition.y);
        sensor.AddObservation(CheckpointTransform.localPosition.x);
        sensor.AddObservation(CheckpointTransform.localPosition.z);
    }

    public override void OnActionReceived(ActionBuffers actions){

        //We add the CamPlaceholders - the objects with the CamPlaceholder tag - to the CamPlaceholders array
        //GameObject[] CamPlaceholders = GameObject.FindGameObjectsWithTag("CamPlaceholder");
        //Debug.Log("This debug will show you the CamPlaceholders we added to the CamPlaceholders array:\n" + CameraPlaceholders[0].name +"\n"+ CameraPlaceholders[1] + "\n " + CameraPlaceholders[2]+"\n " +CameraPlaceholders[3]);

        
        GameObject ThisParent = CameraParent;
        //TEST
        //Debug.Log("The parent of the current agent is: "+ThisParent.name);

        //Looking for the objects with Camera tag
        //GameObject[] CamGameObjects = GameObject.FindGameObjectsWithTag("Camera");
        
        //////////////////////USE THIS LATER
        //ParentOfAgent = this.transform.parent.gameObject;
        //Debug.Log("ParentOfAgent" + ParentOfAgent);
        
        Transform[] AllChildren = ThisParent.GetComponentsInChildren<Transform>();
        
        //foreach (Transform child in AllChildren) { Debug.Log("CamChild (letevos 74.sor)" + child); }

        //CAMGAMEOBJECT UPDATE
        List<GameObject> CamPlaceholders = new();

        foreach(Transform CamPlaceholder in CameraPlaceholders)
            {
                CamPlaceholders.Add(CamPlaceholder.transform.gameObject);
            }

        //A fentebbi 5 sor helyettesíti (javítja)
        //List<GameObject> CamPlaceholders = new List<GameObject>();
        //foreach( var child in AllChildren)
        //    {
        //    if (child.gameObject.tag == "CamPlaceholder")
        //    {
        //        CamPlaceholders.Add(child.transform.gameObject);
        //    }
        //    }

        int action = actions.DiscreteActions[0];
    
        //Az action olyan -e, amit az ágens már használt? 
        //Array az OnEpisodeBeginhez -> amit haszálunk (int array), beletesszünk

        GameObject SelectedPlaceholder = CamPlaceholders[action];
        //Debug.Log("Action we take: " + action);

        GameObject Camera = CameraInstantiate(CameraToPlace, SelectedPlaceholder.transform);
        //Debug.Log("The current Placeholder is " + SelectedPlaceholder);
        Camera.tag="Camera";
        Camera.transform.parent = CameraParent.transform;
        
        //TEST Debug.Log("The type of the SelectedPlaceholder: " + Camera.GetType());
        //Yes, it definitely is a transfor00000000000m variable.
 
        CameraList.Add(Camera); 

        // CameraList: beledobáljuk a lehelyezett kamerákat /OnEpisodeBegin-nél kitakarítjuk
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

    public override void Heuristic(in ActionBuffers actionsOut) {

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = 0;

    }

    public GameObject CameraInstantiate(GameObject CameraToPlace, Transform CameraPlaceholder)
    {
        // Get the rotation of the placeholder
        Quaternion placeholderRotation = CameraPlaceholder.rotation;

        //Debug.Log("Current placeholderRotation of"+ CameraPlaceholder +": "+ placeholderRotation.eulerAngles.y);
        
        // Calculate the opposite direction for the placeholder's rotation
        
        Quaternion oppositeRotation = Quaternion.Euler(0, placeholderRotation.eulerAngles.y + 180, 0);
        // Instantiate the camera
        return (GameObject) Instantiate(CameraToPlace, CameraPlaceholder.position + CameraPlaceholder.TransformDirection(new Vector3(0, 0, 0)), oppositeRotation);

    }


    //IMPORTANT 
    // What we know rn:
    // The cameras we place are not placed yet > we can't really refer to them
    // We can not get their components this way either > question: how can we get a component of an object yet to be created? 
    // Once created, we need a function to check all cameras


    //public int CheckListOfCameraDetections(){
    //int DetectedCheckpoints = 0;

    //    foreach (GameObject Camera in CameraList){
    //        GameObject CameraObject = Camera;
    //        IsCheckpointDetected = CameraObject.GetComponent<CameraDetection>();

    //        if(IsCheckpointDetected.isCheckpointChecked == true){
    //            DetectedCheckpoins += 1;
    //        }
    //    }

    //    return DetectedCheckpoints;
    //}
}