using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class boxSpawner : MonoBehaviour
{
    public int spawnLimit = 10; //upper limit of how many can be spawned

    public float timeBetweenSpawns  = 7.0f; //how many seconds between spawns

    public GameObject[] boxes; //Item to Spawn

    public string[] itemTags; //Tag for conveyerbelt item that is supposed to spawn so it can keep count

    private float timer = 0.0f;

    public int boxCount; //Add items with the same tag to this array to count how many items are actively in the gameworld

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        boxCount = GameObject.FindGameObjectsWithTag("BoxBlue").Length +
            GameObject.FindGameObjectsWithTag("BoxGreen").Length +
            GameObject.FindGameObjectsWithTag("BoxRed").Length;

        if (boxCount != spawnLimit && timer >= timeBetweenSpawns)
        {
            Instantiate(boxes[Random.Range(0, 3)], gameObject.transform);

            timer = 0.0f;

            //Debug.Log("boxes in scene: " + boxCount);
        }
    }
}
