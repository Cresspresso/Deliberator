using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class boxSpawner : MonoBehaviour
{
    public GameObject box;

    private GameObject[] boxes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        boxes = GameObject.FindGameObjectsWithTag("ConveyerBeltItem");

        if (boxes.Length == 0)
        {
            Instantiate(box, gameObject.transform);
        }
    }
}
