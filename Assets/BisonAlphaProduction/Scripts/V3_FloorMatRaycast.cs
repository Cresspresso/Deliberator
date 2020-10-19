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
    [SerializeField] private AudioClip[] concreteClips;
    [SerializeField] private AudioClip[] metalClips;
    [SerializeField] private AudioClip[] woodClips;
    [SerializeField] private AudioClip[] carpetClips;


    [SerializeField] private AudioSource source;

    [HideInInspector] public string foundMat;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5))
        {
            if (hit.transform.gameObject.GetComponent<V3_FloorMatSound>()) //checks if not null
            {
                foundMat = hit.transform.gameObject.GetComponent<V3_FloorMatSound>().GetMat();
                //Debug.Log("FMS found " + foundMat);
            }
            else
            {
                foundMat = "concrete";
            }
        }
    }

    public void PlaySound(string _material)
    {
        if (!source.isPlaying && checkArrayLengths())
        {
            switch (_material)
            {
                case "concrete":
                    source.PlayOneShot(concreteClips[Random.Range(0, 4)]);
                    break;

                case "metal":
                    source.PlayOneShot(metalClips[Random.Range(0, 4)]);
                    break;

                case "wood":
                    //source.PlayOneShot(woodClips[Random.Range(0, 4)]);
                    break;

                case "carpet":
                    //source.PlayOneShot(carpetClips[Random.Range(0, 4)]);
                    break;

                default:
                    Debug.LogError("Cannot find sound to play");
                    break;
            }
        }
    }

    private bool checkArrayLengths()
    {
        if (concreteClips.Length != 0 && metalClips.Length != 0 &&
            woodClips.Length != 0 && carpetClips.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
