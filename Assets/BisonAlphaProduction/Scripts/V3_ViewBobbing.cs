using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Zemp </author>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="22/10/2020">
///			<para>Made it stop head bobbing when in a trigger zone.</para>
///			<para>Added property <see cref="isBobbingEnabled"/>.</para>
///		</log>
/// </changelog>
/// 
public class V3_ViewBobbing : MonoBehaviour
{
    public float walkBobSpeed = 7.0f; // how fast it bobs while walking
    public float walkBobAmount = 0.05f; // how high it bobs while walking

    public float idleBobSpeed = 0.5f; // how fast it bobs while idling
    public float idleBobAmount = 0.05f; // how high it bobs while idling

    /// <seealso cref="V4_StopViewBobZone"/>
    public bool isBobbingEnabled = true;

    private float timer = Mathf.PI / 2;
    private bool walking = false;

    private Vector3 initialLocalPosition;

    private V3_FloorMatRaycast fmRay;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition;
    }

    void Start()
    {
        fmRay = FindObjectOfType<V3_FloorMatRaycast>(); //Find the FloorMatRaycast script, should be attached to FPC
    }

    void Update()
    {
        if (V5_FreeCameraManager.instance.isFree)
            return;

        if (isBobbingEnabled)
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
        else
        {
            /// <author>Elijah Shadbolt</author>
            /// move to return to start position.
            var pos = transform.localPosition;
            var dst = pos;
            dst.y = 0;
            pos = Vector3.MoveTowards(pos, dst, walkBobSpeed * Time.deltaTime);
            transform.localPosition = pos;

            timer = 0;
        }
    }

    void bob(bool _walking)
    {
        float waveslice = Mathf.Sin(timer);

        if (_walking)
        {
            timer += (Time.deltaTime * walkBobSpeed);

            float walkTranslateChange = waveslice * walkBobAmount;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, initialLocalPosition.y + Mathf.Abs(walkTranslateChange), gameObject.transform.localPosition.z);

            fmRay.PlaySound(fmRay.foundMat);

            //Debug.Log(Mathf.Abs(walkTranslateChange));
            //Debug.Log("Walking");
        }
        else
        {
            timer += (Time.deltaTime * idleBobSpeed);

            float idleTranslateChange = waveslice * idleBobAmount;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, initialLocalPosition.y + Mathf.Abs(idleTranslateChange), gameObject.transform.localPosition.z);

            //Debug.Log(Mathf.Abs(idleTranslateChange));
            //Debug.Log("Idling");
        }
    }
}
