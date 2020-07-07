using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class ElevatorDoor : MonoBehaviour
{
    public bool isOpen = false;

    public Animator[] doorAnimators; // only 2 is accepted

    public AudioSource openingSound;
    public AudioSource closingSound;

    //called to open the door
    public void OpenDoor()
    {
        isOpen = true;

        doorAnimators[0].SetBool("Open", isOpen);
        doorAnimators[1].SetBool("Open", isOpen);

        openingSound.Play();
    }

    //called to close the door
    public void CloseDoor()
    {
        isOpen = false;

        doorAnimators[0].SetBool("Open", isOpen);
        doorAnimators[1].SetBool("Open", isOpen);

        closingSound.Play();
    }
}
