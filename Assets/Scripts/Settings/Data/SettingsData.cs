using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[Serializable]
public class SettingsData
{
    public enum QualityPreset
    {
        Low,
        Medium,
        High,
        Ultra
    }

    public enum TextureQuality
    {
        Low,
        Medium,
        High,
        Ultra
    }

    // General Settings
    public Resolution resolution;
    public bool isFullscreen;
    public bool vsync;
    public QualityPreset qualityPreset;

    // Shadow Settings
    public float shadowDistance;
    public ShadowFilteringQuality shadowQuality;
    public int shadowResolution;
    public int shadowCascades;

    // Lighting Settings
    public bool ssaoEnabled;
    public int ssaoQuality;
    public bool ssgiEnabled;
    public int ssgiQuality;
    public bool volumetricLightingEnabled;
    public int volumetricLightingQuality;

    // Effects Settings
    public bool bloomEnabled;
    public float bloomIntensity;
    public bool motionBlurEnabled;
    public float motionBlurIntensity;
    public bool depthOfFieldEnabled;
    public float depthOfFieldFocusDistance;
    public bool vignetteEnabled;
    public float vignetteIntensity;

    // Texture and Detail Settings
    public TextureQuality textureQuality;
    public float lodBias;

    // First Launch Flag
    public bool isFirstLaunch = true;

    // Default constructor (sets default values from the fallback preset)
    public SettingsData()
    {
        // Initialize with the fallback preset
        SetToPreset(GraphicsSettingsManager.FallbackPreset);

        isFirstLaunch = true;
    }

    // Set settings to a specific preset
    public void SetToPreset(string presetName)
    {
        PresetSettings presetSettings = Resources.Load<TextAsset>("presets").text.FromJson<PresetSettings>();
        
        PresetSettings.PresetSetting preset = presetSettings.presets.Find(p => p.name == presetName);

        if (preset != null)
        {
            // General
            resolution = Screen.resolutions[Screen.resolutions.Length - 1];
            isFullscreen = true;
            vsync = true;
            qualityPreset = (QualityPreset)System.Enum.Parse(typeof(QualityPreset), presetName);

            // Shadows
            shadowDistance = preset.settings.shadowDistance;
            shadowQuality = preset.settings.shadowQuality;
            shadowResolution = preset.settings.shadowResolution;
            shadowCascades = preset.settings.shadowCascades;

            // Lighting
            ssaoEnabled = preset.settings.ssaoEnabled;
            ssaoQuality = preset.settings.ssaoQuality;
            ssgiEnabled = preset.settings.ssgiEnabled;
            ssgiQuality = preset.settings.ssgiQuality;
            volumetricLightingEnabled = preset.settings.volumetricLightingEnabled;
            volumetricLightingQuality = preset.settings.volumetricLightingQuality;

            // Effects
            bloomEnabled = preset.settings.bloomEnabled;
            bloomIntensity = preset.settings.bloomIntensity;
            motionBlurEnabled = preset.settings.motionBlurEnabled;
            motionBlurIntensity = preset.settings.motionBlurIntensity;
            depthOfFieldEnabled = preset.settings.depthOfFieldEnabled;
            depthOfFieldFocusDistance = preset.settings.depthOfFieldFocusDistance;
            vignetteEnabled = preset.settings.vignetteEnabled;
            vignetteIntensity = preset.settings.vignetteIntensity;

            // Texture and Detail
            textureQuality = (TextureQuality)System.Enum.Parse(typeof(TextureQuality), preset.settings.textureQuality.ToString());
            lodBias = preset.settings.lodBias;
        }
        else
        {
            Debug.LogError("Preset not found: " + presetName);
        }
    }
}
