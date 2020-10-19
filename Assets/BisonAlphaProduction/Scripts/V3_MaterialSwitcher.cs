using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_MaterialSwitcher : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial; //highlight object shadergraph material

    [SerializeField]private MeshRenderer meshRenderer;
    [SerializeField]private Material originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        originalMaterial = gameObject.GetComponent<MeshRenderer>().material;

        highlightMaterial.SetTexture("Texture2D_2B8148A2", originalMaterial.mainTexture);
    }

    //Test function, OnMouseOver is called every frame that the mouse is over the collider of the object
    void OnMouseOver()
    {
        Debug.Log("Hovering over interactive object");
        if (originalMaterial != highlightMaterial)
        {
            meshRenderer.material = highlightMaterial;
        }
    }

    //OnMouseExit is called when the mouse leaves the collider of the object
    void OnMouseExit()
    {
        meshRenderer.material = originalMaterial;
        Debug.Log("material back to original");
    }
}
