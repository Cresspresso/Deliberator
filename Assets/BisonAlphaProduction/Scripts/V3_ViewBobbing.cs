using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> </author>

public class V3_ViewBobbing : MonoBehaviour
{
    public float bobbingSpeed = 0.18f; // how fast it bobs
    public float bobbingAmount = 0.2f; // how high it bobs
    public float midpoint = 0.0f; // changes height of the camera 0 is default

    private float timer = 0.0f;

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, midpoint + translateChange, gameObject.transform.localPosition.z);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, midpoint, gameObject.transform.localPosition.z);
        }
    }
}
