using System;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float cameraSensitivity = 1;
    
    public GameObject settingsMenu;

    public TMP_InputField cameraSensitivityInputField;

    private void Start()
    {
        settingsMenu.SetActive(false);
    }

    public void OnCenterCamera()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
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

