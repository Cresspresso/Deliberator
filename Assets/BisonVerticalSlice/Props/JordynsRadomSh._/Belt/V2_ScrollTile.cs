using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_ScrollTile : MonoBehaviour
{
    public float speedX = 0.7f;
    public float speedY = 0.5f;
    private float curX;
    private float curY;

    void Start()
    {
        curX = GetComponent<Renderer>().material.mainTextureOffset.x;
        curY = GetComponent<Renderer>().material.mainTextureOffset.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        curX += Time.deltaTime * speedX;
        float OffsetY = Time.deltaTime * speedY;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex" , new Vector2(curX, curY));
    }
}

//public float ScrollX = 0.5f;
//public float ScrollY = 0.5f;

// Update is called once per frame
//void Update()
//{
    //float OffsetX = Time.time * ScrollX;
    //float OffsetY = Time.time * ScrollY;
    //GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
//}


