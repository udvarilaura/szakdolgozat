using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{

    public List<string> detectedObjectList = new();
    string CheckPoint;
    Transform VisionColor;
    public Material searchingMaterial, spottedMaterial;
    public bool isCheckpointChecked = false;
    
    void Start()
    {
        VisionColor = transform.GetComponent<Transform>();
        VisionColor.GetComponentInParent<MeshRenderer>().material = searchingMaterial;
        CheckPoint = GameObject.FindGameObjectWithTag("CheckPoint").tag;
        Debug.Log("The CameraDetection script is wroking, or at least starting");
         

        if(CheckPoint != null){
            //Debug.Log("The tagged object we were looking for: " + CheckPoint);
        }
        else{
            Debug.Log("The detectable tag can not be found."); //Beware of seeing this sign, mortals!!!
        }
    }

    void OnTriggerStay (Collider coll)
    {
        if(coll.gameObject.CompareTag(CheckPoint))
        {
            VisionColor.GetComponentInParent<MeshRenderer>().material = spottedMaterial;

            if(!detectedObjectList.Contains(coll.gameObject.name))
                {
                    detectedObjectList.Add(coll.gameObject.name);
                    isCheckpointChecked = true;
                    Debug.Log("Működik a 49. sor a cameradetection-ben, azaz a checkpoint megtalálva");
                }
        }
    }

    private void OnTriggerExit(Collider coll) 
    {
        if(coll.transform.tag == CheckPoint)
        {
            VisionColor.GetComponentInParent<MeshRenderer>().material = searchingMaterial;
        }        
    }


    private void OnApplicationQuit() 
    {
        for(int i = 0; i < detectedObjectList.Count; i++)
            Debug.Log(detectedObjectList[i]);

        Debug.Log("Number of objects detected: "+detectedObjectList.Count);
    } 
}


/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{

    public List<string> detectedObjectList = new List<string>();

    string CheckPoint;

    public Material searchingMaterial, spottedMaterial;

    Transform lens;
    
    void Start()
    {
        
        lens = transform.parent.GetComponent<Transform>();

        lens.GetComponentInParent<MeshRenderer>().material = searchingMaterial;
        CheckPoint = GameObject.FindGameObjectWithTag("CheckPoint").tag;
        Debug.Log("The CameraDetection script is wroking, or at least starting");
         

        if(CheckPoint != null){
            Debug.Log("The tagged object we were looking for: " + CheckPoint);
        }
        else{
            Debug.Log("The detectable tag can not be found."); //Beware of seeing this sign, mortals!!!
        }
        
    }

    void OnTriggerEnter (Collider coll)
    {
        Debug.Log("Idáig okés (34. sor)");
        if(coll.gameObject.tag == CheckPoint)
        {
            Debug.Log("Működik a 40. sor");
            Vector3 direction = coll.transform.position - lens.position;
            RaycastHit hit;

            if(Physics.Raycast(lens.transform.position, direction.normalized, out hit, 5000))
            {
                if(hit.collider.gameObject.tag == CheckPoint)
                {
                    Debug.Log("Checkpoint megtalálva (44. sor)");
                    lens.GetComponentInParent<MeshRenderer>().material = spottedMaterial;
                    if(!detectedObjectList.Contains(hit.collider.gameObject.name))
                    {
                        detectedObjectList.Add(hit.collider.gameObject.name);
                        foreach(string detectedObject in detectedObjectList)
                            Debug.Log("Detected object: " +detectedObject);
                    }
                }

                else
                {
                    lens.GetComponentInParent<MeshRenderer>().material = searchingMaterial;
                }
            }
        }
    }

    private void OnTriggerExit(Collider coll) 
    {
        if(coll.transform.tag == CheckPoint)
        {
            lens.GetComponentInParent<MeshRenderer>().material = searchingMaterial;
        }        
    }
/*     public override string ToString(){
        string temp = "";
        foreach(string item in detectedObjectList)
        {
            temp += "[";
            temp += item;
            temp += "] ";
        }
        
        return temp;
    }

    private void OnApplicationQuit() 
    {
        for(int i = 0; i < detectedObjectList.Count; i++)
            Debug.Log(detectedObjectList[i]);

        Debug.Log(detectedObjectList.Count);
    } 
}
 */