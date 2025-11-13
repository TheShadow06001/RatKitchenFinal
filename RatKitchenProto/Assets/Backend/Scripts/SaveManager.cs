using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]

public struct SaveData
{
    public float masterSoundValue;
    public float musicSoundValue;
    public float SFXSoundValue;
}

public class SaveManager : MonoBehaviour
{

    [SerializeField] private SaveData data;
    [SerializeField] string fileName;

    public float GetMasterSoundValue => data.masterSoundValue;
    public float GetMusicSoundValue => data.musicSoundValue;
    public float GetSFXSoundValue => data.SFXSoundValue;
    string GetPath()
    {
        return Application.persistentDataPath + "/" + fileName + ".json";
    }

    public void LoadData()
    {
        if (!File.Exists(GetPath()))
        {
            SaveGameFile();
            return;
        }
        string jsonfile = File.ReadAllText(GetPath());
        data = JsonUtility.FromJson<SaveData>(jsonfile);
    }

    public void SaveGameFile()
    {
        string jsonfile = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), jsonfile);
    }

    public void SetMasterSoundValue(float MasterVolume)
    {
        data.masterSoundValue = MasterVolume; SaveGameFile();
    }
    public void SetMusicSoundValue(float MusicVolume)
    {
        data.musicSoundValue = MusicVolume; SaveGameFile();
    }
    public void SetSFXSoundValue(float SFXVolume)
    {
        data.SFXSoundValue = SFXVolume; SaveGameFile();
    }

}
