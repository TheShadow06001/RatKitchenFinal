using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class S_OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown DisplayModeDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle VSyncToggle;
    public GameObject OptionsMenu;

  
    [SerializeField] AudioMixer MasterMixer;
   // [SerializeField] AudioMixer SFXMixer;
    [SerializeField] AudioMixer MusicMixer;
    [SerializeField] GameObject MasterVolumeSlider;
    [SerializeField] GameObject MusicVolumeSlider;
    //[SerializeField] GameObject SFXVolumeSlider;

    private void Start()
    {
        SyncOptions();
        DisplayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        ResolutionDropdown.onValueChanged.AddListener(SetResolution);
        QualityDropdown.onValueChanged.AddListener(SetQuality);
        VSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    public void CloseOptionsMenu() //Return to Main Menu
    {
        OptionsMenu.SetActive(false);
    }

    public void SyncOptions()
    {
        // Display Mode
        var savedDisplay = PlayerPrefs.GetInt("DisplayMode", -1);

        if (savedDisplay >= 0)
        {
            DisplayModeDropdown.SetValueWithoutNotify(savedDisplay);
        }
        else
        {
            var fsm = Screen.fullScreenMode;
            var drop = fsm == FullScreenMode.ExclusiveFullScreen ? 0 :
                fsm == FullScreenMode.FullScreenWindow ? 1 : 2;
            DisplayModeDropdown.SetValueWithoutNotify(drop);
        }

        // Resolution
        var savedRes = PlayerPrefs.GetInt("Resolution", -1);
        if (savedRes >= 0)
        {
            ResolutionDropdown.SetValueWithoutNotify(savedRes);
        }
        else
        {
            var resIndex = Screen.width == 1920 && Screen.height == 1080 ? 1 :
                Screen.width == 1280 && Screen.height == 720 ? 2 : 0;
            ResolutionDropdown.SetValueWithoutNotify(resIndex);
        }

        // Quality
        var savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        QualityDropdown.SetValueWithoutNotify(savedQuality);
        //  VSync  
        var savedVSync = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount > 0 ? 1 : 0);
        VSyncToggle.SetIsOnWithoutNotify(savedVSync == 1);
    }

    public void SetDisplayMode(int i)
    {
        var mode = i == 0 ? FullScreenMode.ExclusiveFullScreen :
            i == 1 ? FullScreenMode.FullScreenWindow :
            i == 2 ? FullScreenMode.Windowed : FullScreenMode.ExclusiveFullScreen;
        Screen.fullScreenMode = mode;
        PlayerPrefs.SetInt("DisplayMode", i);
        PlayerPrefs.Save();
    }

    public void SetResolution(int i)
    {
        var width = i == 0 ? 2560 :
            i == 1 ? 1920 :
            i == 2 ? 1280 : 1920;
        var height = i == 0 ? 1440 :
            i == 1 ? 1080 :
            i == 2 ? 720 : 1080;
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("Resolution", i);
        PlayerPrefs.Save();
    }

    public void SetQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
        PlayerPrefs.SetInt("Quality", i);
        PlayerPrefs.Save();
    }

    public void SetVSync(bool on)
    {
        QualitySettings.vSyncCount = on ? 1 : 0;
        PlayerPrefs.SetInt("VSync", on ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float MasterVolume)
    {
        MasterMixer.SetFloat("MasterVolume", MasterVolume);
    }

    public void SetMusicVolume(float MusicVolume) 
    {
        MusicMixer.SetFloat("MusicVolume", MusicVolume);
    }

    //public void SetSFXVolume(float SFXVolume)
    //{
    //    SFXMixer.SetFloat("SFXVolume", SFXVolume);
    //}
}