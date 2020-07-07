using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class conveyerSwitcher : MonoBehaviour
{
    public bool activated;

    private Animator switcherAnim;

    // Start is called before the first frame update
    void Start()
    {
        switcherAnim = gameObject.GetComponent<Animator>();
    }

    public void Activate()
    {
        if (activated == false) //Activate Switcher
        {
            activated = true;

            switcherAnim.SetBool("SwitchToggle", activated);
        }
        else //Close Switcher
        {
            activated = false;

            switcherAnim.SetBool("SwitchToggle", activated);
        }
    }
}
