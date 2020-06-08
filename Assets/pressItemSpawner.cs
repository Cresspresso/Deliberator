using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class pressItemSpawner : MonoBehaviour
{
    public GameObject blueItem;
    public GameObject redItem;
    public GameObject greenItem;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "BoxBlue")
        {
            Destroy(col.gameObject);
            Instantiate(blueItem, gameObject.transform);
        }
        else if (col.tag == "BoxGreen")
        {
            Destroy(col.gameObject);
            Instantiate(greenItem, gameObject.transform);
        }
        else if (col.tag == "BoxRed")
        {
            Destroy(col.gameObject);
            Instantiate(redItem, gameObject.transform);

        }
    }
}
