using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_Readable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//Properties class for Readables
[System.Serializable]
public class ReadableProperties
{
    public string text;
}