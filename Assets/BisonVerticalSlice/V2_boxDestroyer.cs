using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class V2_boxDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "BoxBlue" || col.tag == "BoxRed" || col.tag == "BoxGreen")
        {
            Destroy(col.gameObject);
        }
    }
}
