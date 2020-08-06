using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Lorenzo Sae-Phoo Zemp</author>
public class Old_yRotationScript : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
