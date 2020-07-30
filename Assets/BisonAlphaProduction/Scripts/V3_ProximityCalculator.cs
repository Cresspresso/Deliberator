using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This following script is to calculate the distance between GameObjects

/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_ProximityCalculator : MonoBehaviour
{
    [Tooltip("Tag for other object to calculate distance")]
    public string obj1Tag; 

    //These are serialized to see if acquiring these objects was successful
#pragma warning disable CS0649
    [SerializeField] private Transform objectTransform;
#pragma warning restore CS0649
    private Transform selfTransform;

    // Start is called before the first frame update
    void Start()
    {
        objectTransform = GameObject.FindGameObjectWithTag(obj1Tag).transform;
        selfTransform = gameObject.transform;
    }

    //Public function to get the distance between self and specified object
    public float GetDistance()
    {
        float distanceToSelf = Vector3.Distance(objectTransform.position, selfTransform.position);
        return distanceToSelf;
    }
}
