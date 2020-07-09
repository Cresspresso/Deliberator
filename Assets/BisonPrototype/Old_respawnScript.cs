using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_respawnScript : MonoBehaviour
{
    public GameObject player;

    private Vector3 playerStartPos;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = playerStartPos;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }

}
