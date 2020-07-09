using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class V2_switcherActivator : MonoBehaviour
{
    public GameObject switcher;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //When it is clicked on
    private void OnMouseDown()
    {
        float distanceToButton = Vector3.Distance(player.transform.position, gameObject.transform.position);
        //Debug.Log(distanceToButton);

        if (distanceToButton < 2.0f)
        {
            //Debug.Log("Activate Switcher");

            switcher.GetComponent<V2_conveyerSwitcher>().Activate();

            //Activate AnimationTrigger
            gameObject.GetComponent<Animator>().SetTrigger("Active");
        }
    }
}
