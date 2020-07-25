using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class V3_SettingsMenu : MonoBehaviour
{
    private AudioMixer audioMixer;
    private GameObject player;

    public Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;

    void Awake()
    {
        availableResolutions = Screen.resolutions;
        SetUpResolutionSettings();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

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

    public void SetResolution (int _resolutionIndex)
    {
        Resolution resolution = availableResolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen (bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }

    public void SetQuality (int _qualityIndex) // 0 low 1 medium 2 high
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

    public void SetVolume (float _volume)
    {
        audioMixer.SetFloat("MasterVolume", _volume);
    }

    public void SetSensitivity (float _sensitivity)
    {
        player.GetComponent<V2_FirstPersonCharacterController>().mouseSensitivity = _sensitivity;
    }
}
