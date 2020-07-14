﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Zemp </author>
public class V3_ViewBobbing : MonoBehaviour
{
    public float walkBobSpeed = 7.0f; // how fast it bobs while walking
    public float walkBobAmount = 0.05f; // how high it bobs while walking

    public float idleBobSpeed = 0.5f; // how fast it bobs while idling
    public float idleBobAmount = 0.05f; // how high it bobs while idling

    private float timer = Mathf.PI / 2;
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

        switch (_walking)
        {
            case true:

                waveslice = Mathf.Sin(timer);

                timer += (Time.deltaTime * walkBobSpeed);

                walkTranslateChange = waveslice * walkBobAmount;
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, Mathf.Abs(walkTranslateChange), gameObject.transform.localPosition.z);

                //Debug.Log(Mathf.Abs(walkTranslateChange));
                //Debug.Log("Walking");
                break;

            case false:

                waveslice = Mathf.Sin(timer);

                timer += (Time.deltaTime * idleBobSpeed);

                idleTranslateChange = waveslice * idleBobAmount;
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, Mathf.Abs(idleTranslateChange), gameObject.transform.localPosition.z);

                //Debug.Log(Mathf.Abs(idleTranslateChange));
                //Debug.Log("Idling");
                break;
        }
    }
}
