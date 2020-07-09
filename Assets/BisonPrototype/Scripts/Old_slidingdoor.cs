using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_slidingdoor : MonoBehaviour
{
    public float waitTime = 2.0f;

    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetBool("doorOpen", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(countdown());
    }

    IEnumerator countdown()
    { 
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("doorOpen", false);
    }
}
