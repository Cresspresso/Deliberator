using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Zemp </author>
public class V3_ViewBobbing : MonoBehaviour
{
    public float walkBobSpeed = 0.18f; // how fast it bobs while walking
    public float walkBobAmount = 0.2f; // how high it bobs while walking

    public float idleBobSpeed = 0.08f; // how fast it bobs while idling
    public float idleBobAmount = 0.02f; // how high it bobs while idling

    private float timer = 0.0f;
    private bool walking = false;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //if player is moving
        if (Mathf.Abs(horizontal) != 0 || Mathf.Abs(vertical) != 0)
        {
            walking = true;
        }
        else //if the player isnt moving
        {
            walking = false;
        }

        bob(walking);
    }

    void bob(bool _walking)
    {
        float waveslice = 0.0f;
        float walkTranslateChange = 0.0f;
        float idleTranslateChange = 0.0f;

        bool justSwitched = false;

        switch (_walking)
        {
            case true:
                //attempt to smoothly interpolate from idling to walking
                justSwitched = true;
                if(justSwitched)
                {
                    walkTranslateChange = Mathf.Lerp(idleTranslateChange, walkTranslateChange, 1.0f);
                    justSwitched = false;
                }

                waveslice = Mathf.Sin(timer);

                timer = timer + walkBobSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }

                walkTranslateChange = waveslice * walkBobAmount;
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, walkTranslateChange, gameObject.transform.localPosition.z);

                //Debug.Log("Walking");
                break;

            case false:

                //attempt to smoothly interpolate from walking to idling 
                justSwitched = true;
                if (justSwitched)
                {
                    idleTranslateChange = Mathf.Lerp(walkTranslateChange, idleTranslateChange, 1.0f);
                    justSwitched = false;
                }

                waveslice = Mathf.Sin(timer);

                timer = timer + idleBobSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }

                idleTranslateChange = waveslice * idleBobAmount;
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, idleTranslateChange, gameObject.transform.localPosition.z);

                //Debug.Log("Idling");
                break;
        }
    }
}
