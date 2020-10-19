using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is for selecting the number matieral to be displayed on the attached gameObject Renderer.
//Further functionality to randomize may be added to this script in the future

/// <author>Lorenzo Sae-Phoo Zemp</author>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Edited it to be initialised from NumPad passcode.</para>
///		</log>
/// </changelog>
/// 
public class V3_ScreenNumber : MonoBehaviour
{
    [SerializeField] private Material[] numMaterials;

    [SerializeField] private int m_digitIndex = 0;
    public int digitIndex => m_digitIndex;

    [SerializeField] private V3_INumPadLockRandomizer m_nplr;
    public V3_INumPadLockRandomizer nplr => m_nplr;

    private new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();

        StartCoroutine(Co());
        // Local Function.
        IEnumerator Co()
        {
            // Wait until the passcode has been generated.
            yield return new WaitUntil(() => nplr.isAlive);
            // Get the passcode.
            var code = nplr.numpadLock.passcode;
            // If the digit index is out of range, log an error.
            Debug.Assert(digitIndex >= 0 && digitIndex < code.Length, "invalid " + nameof(digitIndex), this);
            // Get the character from the passcode.
            char c = code[digitIndex];
            // Convert it to an integer index.
            // Set the material accordingly.
            SetMaterial(c - '0');
        }
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
