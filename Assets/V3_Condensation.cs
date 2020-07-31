using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_Condensation : MonoBehaviour
{
    [SerializeField] private bool activate = false;
    [SerializeField] private float fadeSpeed = 0.1f;

    private Material condensationMaterial;
    private Color colorTarget = new Color(1.0f, 1.0f, 1.0f, 1.0f); //fully visible
    private Color colorStart;
    private Color colorCurrent;
    private float timer = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        condensationMaterial = gameObject.GetComponent<Renderer>().material;
        colorStart = gameObject.GetComponent<Renderer>().material.color;
        colorCurrent = colorStart;
    }

    void Update()
    {
        if (activate && timer <= 1.0)
        {
            colorCurrent = Color.Lerp(colorStart, colorTarget, timer);
            //Debug.Log(colorCurrent);
            condensationMaterial.SetColor("_BaseColor", colorCurrent);

            timer += Time.deltaTime * fadeSpeed;
           // Debug.Log(condensationMaterial.color);
        }
    }

    public void Activate()
    {
        activate = true;
    }
}
