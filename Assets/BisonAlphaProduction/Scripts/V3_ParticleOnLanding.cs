using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script spawns and emits a particle system when something falls and lands.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_ParticleOnLanding : MonoBehaviour
{
    [SerializeField] private GameObject particleSystemPrefab;

    [SerializeField] private float minMagnitude = 1.0f;

    private GameObject spawnedSystem;

    // Start is called before the first frame update
    void Start()
    {
        //spawnedSystem = Instantiate()
    }

    private void Update()
    {

    }

    /// <summary>
    /// On collision of object
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("object relative velocity is: " + collision.relativeVelocity.magnitude);

        if (collision.relativeVelocity.magnitude > minMagnitude)
        {
            spawnedSystem = Instantiate(particleSystemPrefab, collision.transform.position, Quaternion.identity);
            //spawnedSystem.GetComponent<ParticleSystem>().Play();
            //StartCoroutine(WaitToDestroy());
        }
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        if (spawnedSystem.GetComponent<ParticleSystem>().isPlaying)
        {
            StartCoroutine(WaitToDestroy());
        }
        else
        {
            Destroy(spawnedSystem);
            spawnedSystem = null;
        }
    }
}
