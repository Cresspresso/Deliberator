using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> </author>

public class V3_ViewBobbing : MonoBehaviour
{
    public float bobSpeed = 5;
    public float bobAmount = 0.5f;
    public V2_FirstPersonCharacterController characterController;

    private bool active = true;
    private float characterVelocity = 0.0f;
    private float defaultYPos = 0.0f;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultYPos = gameObject.transform.localPosition.y;
        characterVelocity = characterController.GetComponent<CharacterController>().velocity.x;
    }

    // Update is called once per frame
    void Update()
    {
        //if bobbing is active
        if (active)
        {
            //If the player is moving
            if (characterVelocity > 0.1f) 
            {
                time = Time.deltaTime * bobSpeed;
                //Debug.Log(time);
                gameObject.transform.localPosition = new Vector3(transform.localPosition.x,
                    defaultYPos + (Mathf.Sin(time) * bobAmount), transform.localPosition.z);
            }
            //If the player is idling
            else
            {
                time = Time.deltaTime * bobSpeed;
                //Debug.Log(time);
                gameObject.transform.localPosition = new Vector3(transform.localPosition.x,
                    defaultYPos + (Mathf.Sin(time) * (bobAmount)), transform.localPosition.z);
                Debug.Log(defaultYPos + (Mathf.Sin(time) * (bobAmount)));
            }
        }
    }
}
