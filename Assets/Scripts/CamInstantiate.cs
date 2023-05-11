using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamInstantiate : MonoBehaviour
{
    public GameObject CameraInstantiate(GameObject CameraToPlace, Transform CameraPlaceholder)
    {
        // Get the rotation of the placeholder
        Quaternion placeholderRotation = CameraPlaceholder.rotation;

        //Debug.Log("Current placeholderRotation of"+ CameraPlaceholder +": "+ placeholderRotation.eulerAngles.y);

        // Calculate the opposite direction for the placeholder's rotation

        Quaternion oppositeRotation = Quaternion.Euler(0, placeholderRotation.eulerAngles.y + 180, 0);
        // Instantiate the camera
        return (GameObject)Instantiate(CameraToPlace, CameraPlaceholder.position + CameraPlaceholder.TransformDirection(new Vector3(0, 0, 0)), oppositeRotation);
    }
}
