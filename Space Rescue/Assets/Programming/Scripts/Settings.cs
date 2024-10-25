using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] TMP_InputField _masterInput;
    [SerializeField] TMP_InputField _musicInput;
    [SerializeField] TMP_InputField _sfxInput;
    [SerializeField] TMP_InputField _robotInput;

    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _robotSlider;

    [Header("Video")]
    List<Resolution> _resolutions = new();
    [SerializeField] TMP_Dropdown _resDropDown;

    [SerializeField] TMP_Dropdown _fullScreenDrowdown;

    [SerializeField] Slider _fpsSlider;
    [SerializeField] TMP_InputField _fpsInput;

    [SerializeField] GameObject _screenPanel;
    [SerializeField] GameObject _audioPanel;



    // Start is called before the first frame update
    void Start()
    {
        GetAndSetResolution();

        #region Audio Start

        _audioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        float masterTemp = PlayerPrefs.GetFloat("MasterVol");
        masterTemp *= 100;
        _masterInput.text = masterTemp.ToString("0");

        _audioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        float musicTemp = PlayerPrefs.GetFloat("MusicVol");
        musicTemp *= 100;
        _musicInput.text = musicTemp.ToString("0");

        _audioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SfxVol"));
        _sfxSlider.value = PlayerPrefs.GetFloat("SfxVol");
        float sfxTemp = PlayerPrefs.GetFloat("SfxVol");
        sfxTemp *= 100;
        _sfxInput.text = sfxTemp.ToString("0");

        _audioMixer.SetFloat("RobotVol", PlayerPrefs.GetFloat("RobotVol"));
        _robotSlider.value = PlayerPrefs.GetFloat("RobotVol");
        float robotTemp = PlayerPrefs.GetFloat("RobotVol");
        robotTemp *= 100;
        _robotInput.text = robotTemp.ToString("0");

        #endregion
    }

    #region Audio
    public void SetMasterVol(float masterLvl)
    {
        _audioMixer.SetFloat("MasterVol", Mathf.Log10(masterLvl) * 20);
        PlayerPrefs.SetFloat("MasterVol", masterLvl);
        _masterInput.text = (_masterSlider.value * 100).ToString("0");
    }

    public void SetMusicVol(float musicLvl)
    {
        _audioMixer.SetFloat("MusicVol", Mathf.Log10(musicLvl) * 20);
        PlayerPrefs.SetFloat("MusicVol", musicLvl);
        _musicInput.text = (_musicSlider.value * 100).ToString("0");
    }
    public void SetSFXVol(float sfxLvl)
    {
        _audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxLvl) * 20);
        PlayerPrefs.SetFloat("SfxVol", sfxLvl);
        _sfxInput.text = (_sfxSlider.value * 100).ToString("0");
    }
    public void SetRobotVol(float robotLvl)
    {
        _audioMixer.SetFloat("RobotVol", Mathf.Log10(robotLvl) * 20);
        PlayerPrefs.SetFloat("RobotVol", robotLvl);
        _robotInput.text = (_robotSlider.value * 100).ToString("0");
    }

    public void SetMasterVolInput()
    {
        float f;

        float.TryParse(_masterInput.text, out f);
        f /= 100;
        if (f < _masterSlider.minValue)
        {
            f = _masterSlider.minValue;
            _masterSlider.value = f;
        }
        else if (f > _masterSlider.maxValue)
        {
            f = _masterSlider.maxValue;
            _masterSlider.value = f;
        }
        else
        {
            _masterSlider.value = f;
        }

        _audioMixer.SetFloat("MasterVol", Mathf.Log10(f) * 20);
        _masterInput.text = (f * 100).ToString("0");
    }

    public void SetMusicVolInput()
    {
        float f;

        float.TryParse(_musicInput.text, out f);
        f /= 100;
        if (f < _musicSlider.minValue)
        {
            f = _musicSlider.minValue;
            _musicSlider.value = f;
        }
        else if (f > _musicSlider.maxValue)
        {
            f = _musicSlider.maxValue;
            _musicSlider.value = f;
        }
        else
        {
            _musicSlider.value = f;
        }
        _audioMixer.SetFloat("MusicVol", Mathf.Log10(f) * 20);
        _musicInput.text = (f * 100).ToString("0");
    }

    public void SetSfxVolInput()
    {
        float f;

        float.TryParse(_sfxInput.text, out f);
        f /= 100;
        if (f < _sfxSlider.minValue)
        {
            f = _sfxSlider.minValue;
            _sfxSlider.value = f;
        }
        else if (f > _sfxSlider.maxValue)
        {
            f = _sfxSlider.maxValue;
            _sfxSlider.value = f;
        }
        else
        {
            _sfxSlider.value = f;
        }
        _audioMixer.SetFloat("SFXVol", Mathf.Log10(f) * 20);

        _sfxInput.text = (f * 100).ToString("0");
    }

    public void SetRobotVolInput()
    {
        float f;

        float.TryParse(_robotInput.text, out f);
        f /= 100;
        if (f < _robotSlider.minValue)
        {
            f = _robotSlider.minValue;
            _robotSlider.value = f;
        }
        else if (f > _robotSlider.maxValue)
        {
            f = _robotSlider.maxValue;
            _robotSlider.value = f;
        }
        else
        {
            _robotSlider.value = f;
        }
        _audioMixer.SetFloat("RobotVol", Mathf.Log10(f) * 20);

        _robotInput.text = (f * 100).ToString("0");
    }

    #endregion

    #region Video

    #region Resolution

    void GetAndSetResolution()
    {
        Resolution[] tempRes = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        for (int i = tempRes.Length - 1; i > 0; i--)
        {
            _resolutions.Add(tempRes[i]);
        }
        _resDropDown.ClearOptions();

        List<string> options = new();

        for (int i = 0; i < _resolutions.Count; i++)
        {
            string Option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(Option);
        }
        // options.Reverse();
        _resDropDown.AddOptions(options);
        _resDropDown.RefreshShownValue();

        Screen.SetResolution(_resolutions[0].width, _resolutions[0].height, true);
        _fpsSlider.maxValue = Screen.resolutions[0].refreshRate;

        Application.targetFrameRate = Screen.resolutions[0].refreshRate;
        _fpsSlider.value = _fpsSlider.maxValue;

        SetScreenOptions(0);
    }

    void SetResolution(int index)
    {
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
        _resDropDown.value = index;
        _resDropDown.RefreshShownValue();
    }

    public void NewResolution(int index)
    {
        _resDropDown.value = index;
        _resDropDown.RefreshShownValue();

        SetResolution(index);
    }
    #endregion

    #region Fullscreen
    public void SetScreenOptions(int index)
    {
        switch (index)
        {
            case 0:
                {
                    FullScreen();
                }
                break;
            case 1:
                {
                    Borderless();
                }
                break;
            case 2:
                {
                    Windowed();
                }
                break;
        }
    }

    void FullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }
    void Borderless()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
    void Windowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    #endregion

    public void DoVsync(bool value)
    {
        if (value)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = _resolutions[0].refreshRate;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 0;
        }
    }

    public void LimitFPS(float value)
    {
        if (QualitySettings.vSyncCount == 0)
            Application.targetFrameRate = (int)value;

        _fpsInput.text = value.ToString("0");
    }

    public void LimitFPSInput()
    {
        float f;

        float.TryParse(_fpsInput.text, out f);
        if (f < _fpsSlider.minValue)
        {
            f = _fpsSlider.minValue;
            _fpsSlider.value = f;
        }
        else if (f > _fpsSlider.maxValue)
        {
            f = _fpsSlider.maxValue;
            _fpsSlider.value = f;
        }
        else
        {
            _fpsSlider.value = f;
        }
    }

    #endregion

    public void ScreenBtn()
    {
        _screenPanel.SetActive(true);
        _audioPanel.SetActive(false);
    }

    public void AudioBtn()
    {
        _screenPanel.SetActive(false);
        _audioPanel.SetActive(true);
    }

}