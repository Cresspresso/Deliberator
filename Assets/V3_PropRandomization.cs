using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Lorenzo Sae-Phoo Zemp
/// </author>
/// This script is for randomizing props and prop placement for example on tabletops.
public class V3_PropRandomization : MonoBehaviour
{
#pragma warning disable CS0649
    [Tooltip("Array of props to spawn")]
    [SerializeField] private GameObject[] props;

    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;
#pragma warning restore CS0649

    private GameObject[] spawnedProps;


    // Start is called before the first frame update
    void Start()
    {
        spawnedProps = new GameObject[props.Length];

        for (int i = 0; i < props.Length; i++)
        {
            
            spawnedProps[i] = Instantiate(props[i], randomize(), Quaternion.identity, gameObject.transform);
        }
    }

    //randomize function that returns a vector
    private Vector3 randomize()
    {

        Vector3 randomPos =center + new Vector3(Random.Range(-size.x / 2, size.x/2),
            -size.y + 0.5f,
            Random.Range(-size.z / 2, size.z / 2));

        return (randomPos);
    }

    //This draws a gizmo when object is selected in debug and also runs in debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        center = gameObject.transform.position;
        Gizmos.DrawCube(center, size);
    }
}
