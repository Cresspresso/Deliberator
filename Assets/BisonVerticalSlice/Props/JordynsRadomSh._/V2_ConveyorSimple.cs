using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_ConveyorSimple : MonoBehaviour
{
    public float speed;
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = rBody.position;
        rBody.position += transform.forward * speed * Time.fixedDeltaTime;
        rBody.MovePosition(pos);
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
