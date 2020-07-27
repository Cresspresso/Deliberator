using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This following script is for the ValveWheel object where when interacted with
//it gets activated

/// <author>Lorenzo Sae-Phoo Zemp</author>
[RequireComponent(typeof(V3_ProximityCalculator))]
public class V3_ValveWheel : MonoBehaviour
{
    public float interactableDistance = 2.0f;

    private V3_ProximityCalculator proximityCalculator;
    private Animator animator;

    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        proximityCalculator = gameObject.GetComponent<V3_ProximityCalculator>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When clicked on
    private void OnMouseDown()
    {
        if (proximityCalculator.GetDistance() < interactableDistance && !activated)
        {
            Debug.Log("Interacted with");
            animator.SetTrigger("Activate");
            activated = true;
        }
    }
}
