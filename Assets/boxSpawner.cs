using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class boxSpawner : MonoBehaviour
{
    public int spawnLimit = 10; //upper limit of how many can be spawned

    public float timeBetweenSpawns  = 7.0f; //how many seconds between spawns

    public GameObject box; //Item to Spawn

    public string itemTag; //Tag for conveyerbelt item that is supposed to spawn so it can keep count

    private float timer = 0.0f;

    private GameObject[] boxes; //Add items with the same tag to this array to count how many items are actively in the gameworld

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        boxes = GameObject.FindGameObjectsWithTag(itemTag);

        //if (boxes.Length == 0)
        //{
        //    Instantiate(box, gameObject.transform);
        //}
        if (boxes.Length != spawnLimit && timer >= timeBetweenSpawns)
        {
            Instantiate(box, gameObject.transform);

            timer = 0.0f;
        }
    }
}
