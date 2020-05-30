using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    Animator anim;
    public AudioSource buttonSound;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        AudioSource audio = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Active");
            buttonSound.Play();
        }
    }
}
