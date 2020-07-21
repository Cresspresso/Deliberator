using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class V3_Randomizer : MonoBehaviour
{
    private static bool wasGenerated = false;
    private static int[] fingerPrintNumbers;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        fingerPrintNumbers = new int[2];

        if (!wasGenerated) { GeneratePrintNum(); }
    }

    void GeneratePrintNum()
    {
        wasGenerated = true;
        fingerPrintNumbers[0] = Random.Range(0, 10);
        fingerPrintNumbers[1] = Random.Range(0, 10);
    }

    void GenerateWhiteBoardThings()
    {

    }

    public int[] getPrintNumbers()
    {
        return fingerPrintNumbers;
    }
}
