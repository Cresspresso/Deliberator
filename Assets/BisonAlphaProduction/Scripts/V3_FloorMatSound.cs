using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class V3_FloorMatSound : MonoBehaviour
{
    enum Material
    {
        Concrete,
        Metal, 
        Wood, 
        Glass
    }

    [SerializeField] private bool wood = false;
    [SerializeField] private bool glass = false;
    [SerializeField] private bool metal = false;
    [SerializeField] private bool concrete = false;

    private Material materialSelected;

    [SerializeField] private AudioClip[] sounds;

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (wood) { 
                glass = false; 
                metal = false;
                concrete = false;
                materialSelected = Material.Wood;
            }
            if (glass) { 
                wood = false; 
                metal = false;
                concrete = false;
                materialSelected = Material.Glass;
            }
            if (metal) { 
                wood = false; 
                glass = false;
                concrete = false;
                materialSelected = Material.Metal;
            }
            if (concrete) {
                wood = false;
                glass = false;
                metal = false;
                materialSelected = Material.Concrete;
            }
        }
    }

    public string GetMat()
    {
        switch (materialSelected)
        {
            case Material.Concrete:
                return ("concrete");
            case Material.Glass:
                return ("glass");
            case Material.Metal:
                return ("metal");
            case Material.Wood:
                return ("wood");
            default:
                return ("null");
        }
    }
}
