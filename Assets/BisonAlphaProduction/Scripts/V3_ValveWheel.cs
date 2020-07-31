using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This following script is for the ValveWheel object where when interacted with
//it gets activated

/// <author>Lorenzo Sae-Phoo Zemp</author>
[RequireComponent(typeof(V3_ProximityCalculator))]
[RequireComponent(typeof(V2_Handle))]
public class V3_ValveWheel : MonoBehaviour
{
    delegate void DidChangeState(); //delegate function 
    DidChangeState didChangeState; //delegate variable

    public float interactableDistance = 2.5f;
    public V3_Condensation targetObj; // change datatype depending on what component i want to access

    private V2_Handle hoverHandle;
    private V3_ProximityCalculator proximityCalculator;
    private Animator animator;

    [SerializeField] private bool activatable = false;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        proximityCalculator = gameObject.GetComponent<V3_ProximityCalculator>();
        animator = gameObject.GetComponent<Animator>();
        hoverHandle = gameObject.GetComponent<V2_Handle>();

        didChangeState = activateCondensation; //set function to call when delegate 
    }

    // When clicked on
    private void OnMouseDown()
    {
        // if clicked on within range
        if (proximityCalculator.GetDistance() < interactableDistance && activatable)
        {
            Debug.Log("Interacted with");
            animator.SetTrigger("Activate");
            activated = true;
            activatable = false;

            hoverHandle.hoverInfo = new V2_HandleHoverInfo("Opened", null);

            didChangeState(); //call delegate
        }
    }

    #region Delegates
    void activateCondensation()
    {
        targetObj.Activate();
    }
    #endregion

    public void setActivatable(bool _bool)
    {
        activatable = _bool;
    }

    public bool getActivated()
    {
        return activated;
    }
}
