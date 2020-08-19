using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;
using UnityEngine;

//This script is for the ReadableXMLCreator Editor tool to create a usable XML for Readable Objects

/// <author>Lorenzo Sae-Phoo Zemp</author>
public class ReadableXMLCreator : EditorWindow
{
    private ReadableProperties saveItems = new ReadableProperties();

    private string fileName; //What the files should be name once it is created

    private string text; //The stored text

    //creates window and toolbar access for the tool
    [MenuItem("Deliberator Tools/Readables XML Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ReadableXMLCreator));
    }

    //Update Loop but for editor UI, kinda
    private void OnGUI()
    {
        //Label at the top of the tool
        GUILayout.Label("XML Creator", EditorStyles.boldLabel);

        text = EditorGUILayout.TextField("Text", text);

        fileName = EditorGUILayout.TextField("File Save Name", fileName);

        //create button to save instructions and check if its being pressed
        if (GUILayout.Button("Create XML"))
        {
            //save changes of the file
            if (text != "" && fileName != "")
            {
                CreateXML();
            }
            else
            {
                Debug.LogError("Did not fill ReadableXMLCreator text fields");
            }
        }
    }

    //Creates and saves XML file based on iputted fileds.
    private void CreateXML()
    {
        saveItems.text = text;

        XmlSerializer serializer = new XmlSerializer(typeof(ReadableProperties));
        Debug.Log(Application.dataPath + "/BisonAlphaProduction/Readables/XML/" + fileName + ".xml");
        FileStream stream = new FileStream(Application.dataPath + "/BisonAlphaProduction/Readables/XML/" + fileName + ".xml", FileMode.Create);
        serializer.Serialize(stream, saveItems);
        stream.Close();
    }
}
