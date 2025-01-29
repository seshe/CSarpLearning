using UnityEngine;
using System.IO;

public class SettingsSerializer
{
    private string settingsFilePath;

    public SettingsSerializer()
    {
        settingsFilePath = Application.persistentDataPath + "/graphicsSettings.json";
    }

    public void SaveSettings(GraphicsSettingsSO settingsSO)
    {
        string jsonData = JsonUtility.ToJson(settingsSO, true);
        File.WriteAllText(settingsFilePath, jsonData);
    }

    public bool LoadSettings(GraphicsSettingsSO settingsSO)
    {
        if (File.Exists(settingsFilePath))
        {
            string jsonData = File.ReadAllText(settingsFilePath);
            JsonUtility.FromJsonOverwrite(jsonData, settingsSO);
            return true;
        }
        return false;
    }
}
