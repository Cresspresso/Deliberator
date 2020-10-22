using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This following script is for the ValveWheel object where when interacted with
//it gets activated and also triggers and animation

/// <author>Lorenzo Sae-Phoo Zemp</author>
[RequireComponent(typeof(V3_ProximityCalculator))]
[RequireComponent(typeof(V2_Handle))]
public class V3_ValveWheel : MonoBehaviour
{
    delegate void DidChangeState(); //delegate function 
    DidChangeState didChangeState; //delegate variable

    public float interactableDistance = 2.5f;
    public V3_Condensation targetObj; // change datatype depending on what component i want to access
    public ParticleSystem steamParticle;

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
            gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);

            animator.SetTrigger("Activate");
            activated = true;
            activatable = false;
            if (steamParticle != null)
            {
                steamParticle.Play();
            }

            hoverHandle.hoverInfo = new V2_HandleHoverInfo("Opened", null);

            didChangeState(); //call delegate
        }
    }

    /// <summary>
    /// Delegate Function
    /// </summary>
    /// 
    /// <changelog>
    ///		<log author="Elijah Shadbolt" date="20/10/2020">
    ///			<para>Added Dependable and set its firstLiteral to true.</para>
    ///			<para>Made the targetObj optional (ignored if null).</para>
    ///		</log>
    /// </changelog>
    /// 
    #region Delegates
    void activateCondensation()
    {
        if (targetObj)
        {
            targetObj.Activate();
        }

        var dependable = GetComponent<Dependable>();
        if (dependable) {
            dependable.firstLiteral = true;
        }
    }
    #endregion

    /// <summary>
    /// set wether object can be activated, can be used to lock or unlock 
    /// </summary>
    /// <param name="_bool"></param>
    public void setActivatable(bool _bool)
    {
        activatable = _bool;
    }

    /// <summary>
    /// returns wether or not the valve has been activates
    /// </summary>
    /// <returns></returns>
    public bool getActivated()
    {
        return activated;
    }
}
