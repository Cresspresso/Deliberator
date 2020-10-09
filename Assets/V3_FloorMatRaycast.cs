using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script raycasts a ray down from the player and checks if the component the player is walking on has 
/// V3_FloorMatSound and then plays the sound.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_FloorMatRaycast : MonoBehaviour
{
    [Tooltip("Only 4 sounds! In following order, Concrete, Metal, Wood, Glass")]
    [SerializeField] private AudioClip[] sounds;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5))
        {
            if (hit.transform.gameObject.GetComponent<V3_FloorMatSound>())
            {
                string foundMat = hit.transform.gameObject.GetComponent<V3_FloorMatSound>().GetMat();
                Debug.Log("FMS found " + foundMat);

            }
        }
    }

    private void PlaySound(string _material)
    {
        switch (_material)
        {
            case "concrete":

            case "glass":

            case "metal":

            case "wood":

            default:
                Debug.LogError("Cannot find sound to play");
                break;
        }
    }
}
