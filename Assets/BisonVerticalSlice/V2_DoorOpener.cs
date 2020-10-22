using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <author> Elijah Shadbolt </author>
/// <author> Lorenzo Zemp </author>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="22/10/2020">
///			<para>Added delay to sync up with player animation.</para>
///		</log>
/// </changelog>
/// 
[System.Obsolete("Deprecated")]
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
                OnToggleDoor();
            }
        }
    }

    private bool m_isOpen = false;
    public bool isOpen {
        get => m_isOpen;
        set
        {
            m_isOpen = value;
            buttonHandle.handle.hoverInfo = isOpen ? openedHoverInfo : (isLocked ? lockedHoverInfo : closedHoverInfo);
        }
    }

    public Animator[] anim;
    public AudioSource lockedSound;
    public AudioSource openingSound;
    public AudioSource closingSound;

    public AudioSource lockedSound2;
    public float delayLockedSound2 = 0.05f;
    public AudioSource openingSound2;
    public float delayOpeningSound2 = 0.05f;
    public AudioSource closingSound2;
    public float delayClosingSound2 = 0.05f;

    public V2_HandleHoverInfo lockedHoverInfo = new V2_HandleHoverInfo("Locked");
    public V2_HandleHoverInfo openedHoverInfo = new V2_HandleHoverInfo("Close");
    public V2_HandleHoverInfo closedHoverInfo = new V2_HandleHoverInfo("Open");

    private V2_ButtonHandle m_buttonHandle;
    public V2_ButtonHandle buttonHandle { get { PrepareButtonHandle(); return m_buttonHandle; } }
    private void PrepareButtonHandle()
    {
        if (!m_buttonHandle)
        {
            m_buttonHandle = GetComponentInChildren<V2_ButtonHandle>();
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
            switch (doorType)
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
            Invoke(nameof(PlayLockedSound2), delayLockedSound2);
        }
        else
        {
            OnToggleDoor();
        }
    }

    private void PlayLockedSound2() { if (lockedSound2) { lockedSound2.Play(); } }
    private void PlayOpeningSound2() { if (openingSound2) { openingSound2.Play(); } }
    private void PlayClosingSound2() { if (closingSound2) { closingSound2.Play(); } }

    public void ToggleDoor()
    {
        if (!isLocked)
        {
            OnToggleDoor();
        }
    }

    private void OnToggleDoor()
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

        // audio
        if (isOpen)
        {
            if (openingSound) { openingSound.Play(); }
            Invoke(nameof(PlayOpeningSound2), delayOpeningSound2);
        }
        else
        {
            if (closingSound) { closingSound.Play(); }
            Invoke(nameof(PlayClosingSound2), delayClosingSound2);
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
