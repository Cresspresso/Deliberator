using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This script makes the object attached to it a readable object, the components below are required.
/// User can edit the interactable distance in the inspector window 
/// User needs to give a filename for the XML file in the inspector window
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
[RequireComponent(typeof(V3_ProximityCalculator))]
[RequireComponent(typeof(BoxCollider))]
[System.Obsolete("Use PaperClue instead because it doesn't need XML")] // Elijah added this attribute
public class V3_Readable : MonoBehaviour
{
    [Tooltip("Please enter the name of the XML file exactly")]
    [SerializeField] private string fileName; //file name to find file by

    private string readableText; //stores imported xml string
    private ReadableProperties readableProperties; //stores deserialized XML properties

    //get readable menu in main canvas
    private V3_ReadableMenu m_readableMenu;
    private V3_ReadableMenu readableMenu
    {
        get
        {
            if (!m_readableMenu)
            {
                m_readableMenu = FindObjectOfType<V3_ReadableMenu>();
            }
            return m_readableMenu;
        }
    }

    [SerializeField] private float interactableDistance = 2.5f; //How close the player has to be to interact with this object
    private V3_ProximityCalculator proximityCalculator; //Proximity calculator script

    // Start is called before the first frame update
    void Start()
    {
        proximityCalculator = gameObject.GetComponent<V3_ProximityCalculator>(); //Get proximity calculator

        LoadXML(fileName); //load the xml file
    }

    //If object is clicked on
    private void OnMouseDown()
    {
        print("hi");
        if (proximityCalculator.GetDistance() < interactableDistance)
        {
            //Debug.Log("Interacted with readable object");
            readableMenu.SetText("");
            readableMenu.SetText(readableText);

            readableMenu.Pause();
        }
    }

    //Xml load function, reads xml file and copies to readableText
    private void LoadXML(string _fileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ReadableProperties));
        FileStream stream = new FileStream(Application.dataPath + "/BisonAlphaProduction/Readables/XML/" + _fileName + ".xml", FileMode.Open);
        readableProperties = serializer.Deserialize(stream) as ReadableProperties ;
        stream.Close();

        if (readableProperties != null)
        {
            //Debug.Log(_fileName + " XML Loaded Succesfully");
            readableText = readableProperties.text; //copy text to this class
        } 
        else
        {
            Debug.LogError("Error Loading " + _fileName + " XML");
        }
    }
}

//Properties class for Readables
[System.Serializable]
public class ReadableProperties
{
    public string text;
}