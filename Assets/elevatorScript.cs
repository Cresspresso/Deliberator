using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class elevatorScript : MonoBehaviour
{
    public bool up = true; //bool for if its going up on next button push

    public float elevatorTravelLength = 5.0f; //amount of units on the Y axis elevator is going up
    public float elevatorTravelTime = 2.0f; //how long elevator travel takes in seconds

    public GameObject topDoor; //gameObject for door on the top
    public GameObject btmDoor; //gameObject for door on the bottom

    private GameObject player; //stores reference to player

    private Transform buttonParent; //stores reference to buttons parent object

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        buttonParent = gameObject.transform.parent;

        if (up == true)
        {
            btmDoor.GetComponent<ElevatorDoor>().OpenDoor();
        }
        else
        {
            topDoor.GetComponent<ElevatorDoor>().OpenDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When it is clicked on
    private void OnMouseDown()
    {
        float distanceToButton = Vector3.Distance(player.transform.position, gameObject.transform.position);
        //Debug.Log(distanceToButton);

        float buttonParentYPosition = buttonParent.transform.position.y;

        if (distanceToButton < 2.0f && up == true)
        {
            Debug.Log("Elevator Going Up");
            buttonParent.DOMoveY(buttonParentYPosition + elevatorTravelLength,
                elevatorTravelLength, false);

            btmDoor.GetComponent<ElevatorDoor>().CloseDoor();

            StartCoroutine(WaitToOpen(topDoor));

            up = false;
        }
        else if (distanceToButton < 2.0f && up == false)
        {
            Debug.Log("Elevator Going Down");
            buttonParent.DOMoveY(buttonParentYPosition - elevatorTravelLength,
                elevatorTravelLength, false);

            topDoor.GetComponent<ElevatorDoor>().CloseDoor();

            StartCoroutine(WaitToOpen(btmDoor));

            up = true;
        }
    }

    IEnumerator WaitToOpen(GameObject _door)
    {
        yield return new WaitForSeconds(elevatorTravelTime);
        _door.GetComponent<ElevatorDoor>().OpenDoor();
        //Debug.Log("Opening Elevator Door");
    }
}
