using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static float cameraSensitivity = 1;
    
    public GameObject settingsMenu;

    public TMP_InputField cameraSensitivityInputField;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();
    
    private void Start()
    {
        //Screen.fullScreen = true;
        settingsMenu.SetActive(false);

        resolutions = Screen.resolutions;
        Screen.SetResolution(2560, 1440, true);
        SetResolutionsDropDown();
    }

    private void SetResolutionsDropDown()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution resolution = resolutions[i];
            if((float)resolution.height / ((float)resolution.width / 16) == 9)
                filteredResolutions.Add(resolution);
            
            Debug.Log(resolution.height / (resolution.width / 16));
        }

        int currentResolutionIndex = 0;
        List<String> resolutionOptions = new List<String>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            Resolution resolution = filteredResolutions[i];
            string resolutionOption = resolution.width + "x" + resolution.height;
            resolutionOptions.Add(resolutionOption);

            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    
    public void OnCenterCamera()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 5;
    }

    public void OnChangeCameraSensitivity()
    {
        if (cameraSensitivityInputField.text == "")
        {
            cameraSensitivity = 1;
            cameraSensitivityInputField.text = "1";
            return;
        }
        cameraSensitivity = float.Parse(cameraSensitivityInputField.text);
    }

    public void OnChangeResolution(int index)
    {
        Resolution resolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void OnChangeFullscreenMode(bool value)
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
    
    public void OnOpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    
    public void OnCloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void OnQuit()
    {
        #if !Unity_Editor
            Application.Quit();
        #endif
    }
}

