using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the interactions between the player and the drawer
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_DrawerOpener : MonoBehaviour
{
    private Animator animator;
    private V2_Handle handle;

    private bool isOpen = false;

    [SerializeField] private AudioSource sourceOpen;
    [SerializeField] private AudioSource sourceClose;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        handle = gameObject.GetComponent<V2_Handle>();
    }

    public void TriggerAnimation()
    {
        isOpen = !isOpen;

        if(isOpen)
        {
            handle.hoverInfo = new V2_HandleHoverInfo("Close", null);
            animator.SetTrigger("TriggerOpen");
            sourceOpen.Play();
        } 
        else
        {
            handle.hoverInfo = new V2_HandleHoverInfo("Open", null);
            animator.SetTrigger("TriggerClose");
            sourceClose.Play();
        }

    }
}
