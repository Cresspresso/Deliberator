using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script assigns the object it is attached to a selectable material choice, which in turn will
/// change the sound that is played when the player walks over it.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
[ExecuteInEditMode]
public class V3_FloorMatSound : MonoBehaviour
{
    enum Material
    {
        Concrete,
        Metal, 
        Wood,
        Carpet
    }

    [SerializeField] private Material chosenMat;

    //[Tooltip("Only 4 sounds! In following order, Concrete, Metal, Wood, Glass")]
    //[SerializeField] private AudioClip[] sounds;

    public string GetMat()
    {
        switch (chosenMat)
        {
            case Material.Concrete:
                return ("concrete");
            case Material.Wood:
                return ("wood");
            case Material.Metal:
                return ("metal");
            case Material.Carpet:
                return ("carpet");
            default:
                return ("null");
        }
    }
}
