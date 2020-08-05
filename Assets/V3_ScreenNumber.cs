using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is for selecting the number matieral to be displayed on the attached gameObject Renderer.

/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_ScreenNumber : MonoBehaviour
{
    public int number;

    [SerializeField] private Material[] numMaterials;

    private new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();

        SetMaterial(number);
    }

    /// <summary>
    /// This function sets the renderer material to the specified material number in the inspector
    /// </summary>
    /// <param name="matNumber"></param>
    void SetMaterial(int matNumber)
    {
        renderer.material = numMaterials[matNumber];
    }
}
