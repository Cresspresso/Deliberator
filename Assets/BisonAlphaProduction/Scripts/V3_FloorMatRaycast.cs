﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script raycasts a ray down from the player and checks if the component the player is walking on has 
/// V3_FloorMatSound and then plays the sound.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_FloorMatRaycast : MonoBehaviour
{
    [Tooltip("Only 4 sounds! In following order, Concrete, Metal, Wood, Carpet")]
    [SerializeField] private AudioClip[] sounds;

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
        if (!source.isPlaying && sounds.Length != 0)
        {
            switch (_material)
            {
                case "concrete":
                    source.PlayOneShot(sounds[0]);
                    break;
                case "metal":
                    source.PlayOneShot(sounds[1]);
                    break;
                case "wood":
                    source.PlayOneShot(sounds[2]);
                    break;
                case "carpet":
                    source.PlayOneShot(sounds[3]);
                    break;

                default:
                    Debug.LogError("Cannot find sound to play");
                    break;
            }
        }
    }
}
