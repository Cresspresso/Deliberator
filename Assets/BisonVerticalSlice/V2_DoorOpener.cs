using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <author> Elijah Shadbolt </author>
/// <author> Lorenzo Zemp </author>
[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_DoorOpener : MonoBehaviour
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

    public V2_HandleHoverInfo lockedHoverInfo = new V2_HandleHoverInfo("Locked");
    public V2_HandleHoverInfo openedHoverInfo = new V2_HandleHoverInfo("Close");
    public V2_HandleHoverInfo closedHoverInfo = new V2_HandleHoverInfo("Open");

    private V2_ButtonHandle m_buttonHandle;
    public V2_ButtonHandle buttonHandle { get { PrepareButtonHandle(); return m_buttonHandle; } }
    private void PrepareButtonHandle()
    {
        if (!m_buttonHandle)
        {
            m_buttonHandle = GetComponent<V2_ButtonHandle>();
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

    private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
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
