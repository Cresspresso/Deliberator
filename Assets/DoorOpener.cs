﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <author> Elijah Shadbolt </author>
/// <author> Lorenzo Zemp </author>
[RequireComponent(typeof(ButtonHandle))]
public class DoorOpener : MonoBehaviour
{
    public int doorType; //Only 1 or 2, 1 as in single door and 2 for double

    public bool openOnAwake = false;

    [FormerlySerializedAs("isLocked")]
    [SerializeField]
    private bool m_isLocked = false;
    public bool isLocked {
        get => m_isLocked;
        set
        {
            m_isLocked = value;
            if (isOpen && m_isLocked)
            {
                ToggleDoor();
            }
        }
    }

    public bool isOpen { get; private set; } = false;

    public Animator[] anim;
    public AudioSource lockedSound;
    public AudioSource openingSound;
    public AudioSource closingSound;

    public HandleHoverInfo lockedHoverInfo = new HandleHoverInfo("Locked");
    public HandleHoverInfo openedHoverInfo = new HandleHoverInfo("Close");
    public HandleHoverInfo closedHoverInfo = new HandleHoverInfo("Open");

    private ButtonHandle m_buttonHandle;
    public ButtonHandle buttonHandle { get { PrepareButtonHandle(); return m_buttonHandle; } }
    private void PrepareButtonHandle()
    {
        if (!m_buttonHandle)
        {
            m_buttonHandle = GetComponent<ButtonHandle>();
            if (m_buttonHandle)
            {
                m_buttonHandle.onClick -= OnClick;
                m_buttonHandle.onClick += OnClick;
            }
            else
            {
                Debug.LogError("ButtonHandle is null", this);
            }
        }
    }

    private void Awake()
    {
        PrepareButtonHandle();
    }

    private void OnClick(ButtonHandle buttonHandle, HandleController handleController)
    {
        if (isLocked)
        {
            switch(doorType)
            {
                case 1:
                    anim[0].SetTrigger("TriedToOpen");
                    break;
                case 2:
                    anim[0].SetTrigger("TriedToOpen");
                    anim[1].SetTrigger("TriedToOpen");
                    break;
                default:
                    Debug.Log("Incorrect Door Type? Check Door Type Integer, only 1 or 2");
                    break;
            }

            buttonHandle.handle.hoverInfo = lockedHoverInfo;

            if (lockedSound) { lockedSound.Play(); }
        }
        else
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;

        switch (doorType)
        {
            case 1:
                anim[0].SetBool("Open", isOpen);
                break;
            case 2:
                anim[0].SetBool("Open", isOpen);
                anim[1].SetBool("Open", isOpen);
                break;
            default:
                Debug.Log("Incorrect Door Type? Check Door Type Integer, only 1 or 2");
                break;
        }

        buttonHandle.handle.hoverInfo = isOpen ? openedHoverInfo : closedHoverInfo;

        // audio
        if (isOpen)
        {
            if (openingSound) { openingSound.Play(); }
        }
        else
        {
            if (closingSound) { closingSound.Play(); }
        }
    }

    private void OnEnable()
    {
        isOpen = openOnAwake;

        switch (doorType)
        {
            case 1:
                anim[0].SetBool("Open", isOpen);
                break;
            case 2:
                anim[0].SetBool("Open", isOpen);
                anim[1].SetBool("Open", isOpen);
                break;
            default:
                Debug.Log("Incorrect Door Type? Check Door Type Integer, only 1 or 2");
                break;
        }
    }

    private void OnDisable()
    {
        if (m_buttonHandle)
        {
            m_buttonHandle.onClick -= OnClick;
        }
    }
}
