using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <authors>Lorenzo Sae-Phoo Zemp, Elijah Shadbolt</authors>
public class V3_SettingsMenu : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
    private AudioMixer audioMixer;
#pragma warning restore CS0649

    private GameObject player;

    [SerializeField] private Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;

    private float mouseSensitivity;

    //called before start
    void Awake()
    {
        availableResolutions = Screen.resolutions;
        SetUpResolutionSettings();

        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            Debug.Log("MouseSensitivity Exists");
            mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        }
    }

    //called on start
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Fills in the resolution settings dropdown menu with options
    /// </summary>
    void SetUpResolutionSettings()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + "x" + availableResolutions[i].height;
            resolutionOptions.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    
    /// <summary>
    /// sets the resolution to the resolution selected
    /// </summary>
    /// <param name="_resolutionIndex"></param>
    public void SetResolution (int _resolutionIndex)
    {
        Resolution resolution = availableResolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// sets wether or not the game is in fullscreen
    /// </summary>
    /// <param name="_fullscreen"></param>
    public void SetFullscreen (bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }

    /// <summary>
    /// sets graphical quality of the game
    /// </summary>
    /// <param name="_qualityIndex"></param>
    public void SetQuality (int _qualityIndex) // 0 low 1 medium 2 high
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

    /// <summary>
    /// sets the volume of the game
    /// </summary>
    /// <param name="_volume"></param>
    public void SetVolume (float _volume)
    {
        audioMixer.SetFloat("MasterVolume", _volume);
    }

    /// <summary>
    /// sets the mouse sensitivity in the game
    /// </summary>
    /// <param name="_sensitivity"></param>
    public void SetSensitivity (float _sensitivity)
    {
        Debug.Log("Sensitivity Changed");

        PlayerPrefs.SetFloat("MouseSensitivity", _sensitivity);
        mouseSensitivity = _sensitivity;
        player.GetComponent<V2_FirstPersonCharacterController>().mouseSensitivity = mouseSensitivity;
    }
}
