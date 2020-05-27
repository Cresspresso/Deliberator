using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class boxDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ConveyerBeltItem")
        {
            Destroy(col.gameObject);
        }
    }
}
