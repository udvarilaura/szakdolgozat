using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlaceHolderParent : MonoBehaviour
{
    public List<GameObject> CamPlaceHolders = new();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            CamPlaceHolders.Add(child);
        }

        //Debug.Log(CamPlaceHolders.Count);
    }
}
