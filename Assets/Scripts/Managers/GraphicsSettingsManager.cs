using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GraphicsSettingsManager : MonoBehaviour
{
    public static event Action OnSettingsChanged;

    [SerializeField] private HDRenderPipelineAsset hdrpAsset;
    [SerializeField] private GraphicsSettingsSO graphicsSettingsSO;
    [SerializeField] private SettingsSerializer settingsSerializer;
    [SerializeField] private SettingsApplier settingsApplier;

    public GraphicsSettingsSO.SettingsData Data => graphicsSettingsSO.Data;
    public PresetSettings presetSettings;

    public const string FallbackPreset = "Low"; // Пресет, який буде використовуватись у разі проблем з json, або для скидання налаштувань

    private Coroutine applyCoroutine;

    private void Awake()
    {
        LoadPresetSettings();

        // Load settings from file or use default values
        LoadSettings();

        // Initialize UI elements in all UIInitializable scripts
        UIInitializable[] uiInitializables = FindObjectsOfType<UIInitializable>(true);
        foreach (UIInitializable uiInitializable in uiInitializables)
        {
            uiInitializable.InitializeUI();
        }
    }

    private void Start()
    {
        // Apply loaded settings
        ApplyAllSettings();

        if (graphicsSettingsSO.Data.isFirstLaunch)
        {
            DetectHardwareAndApplySettings();
            graphicsSettingsSO.Data.isFirstLaunch = false;
            SaveSettings();
        }
    }

    // Applies all settings from settingsData to the HDRP Asset
    public void ApplyAllSettings()
    {
        if (hdrpAsset == null)
        {
            Debug.LogError("HDRP Asset is not assigned!");
            return;
        }

        settingsApplier.ApplySettings(graphicsSettingsSO.Data, hdrpAsset);

        OnSettingsChanged?.Invoke();
    }

    // Schedules the ApplyAllSettings() to be called after a short delay
    public void ScheduleApply()
    {
        if (applyCoroutine != null) StopCoroutine(applyCoroutine);
        applyCoroutine = StartCoroutine(ApplyDelayed());
    }

    private IEnumerator ApplyDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        ApplyAllSettings();
    }

    // Detects hardware and applies appropriate settings
    public void DetectHardwareAndApplySettings()
    {
        Debug.Log("Detecting hardware and applying settings...");

        // GPU
        string gpuName = SystemInfo.graphicsDeviceName;
        int vram = SystemInfo.graphicsMemorySize;
        Debug.Log("GPU: " + gpuName + ", VRAM: " + vram + "MB");

        // CPU
        string cpuName = SystemInfo.processorType;
        int cpuCores = SystemInfo.processorCount;
        Debug.Log("CPU: " + cpuName + ", Cores: " + cpuCores);

        // RAM
        int ram = SystemInfo.systemMemorySize;
        Debug.Log("RAM: " + ram + "MB");

        // Get the detected graphics tier
        var tier = SystemInfo.graphicsDeviceTier;

        // Default to Medium settings
        SettingsData.QualityPreset recommendedPreset = SettingsData.QualityPreset.Medium;

        // Choose preset based on graphics tier and VRAM
        switch (tier)
        {
            case UnityEngine.Rendering.GraphicsTier.Tier1:
                // For Tier 1, consider VRAM
                if (vram >= 2000)
                {
                    recommendedPreset = SettingsData.QualityPreset.Medium;
                }
                else
                {
                    recommendedPreset = SettingsData.QualityPreset.Low;
                }
                break;

            case UnityEngine.Rendering.GraphicsTier.Tier2:
                // For Tier 2, consider VRAM
                if (vram >= 4000)
                {
                    recommendedPreset = SettingsData.QualityPreset.High;
                }
                else
                {
                    recommendedPreset = SettingsData.QualityPreset.Medium;
                }
                break;

            case UnityEngine.Rendering.GraphicsTier.Tier3:
                // For Tier 3, consider VRAM
                if (vram >= 8000)
                {
                    recommendedPreset = SettingsData.QualityPreset.Ultra;
                }
                else
                {
                    recommendedPreset = SettingsData.QualityPreset.High;
                }
                break;
        }

        Debug.Log("Detected Graphics Tier: " + tier + ", applying preset: " + recommendedPreset);
        SetQualityPreset((int)recommendedPreset);
    }

    // Saves the current settings to a file
    public void SaveSettings()
    {
        settingsSerializer.SaveSettings(graphicsSettingsSO);
        Debug.Log("Graphics settings saved.");
    }

    // Loads settings from a file or uses default values if the file doesn't exist
    public void LoadSettings()
    {
        bool loaded = settingsSerializer.LoadSettings(graphicsSettingsSO);
        if (!loaded)
        {
            Debug.Log("No saved settings found. Using default values.");
            ResetToDefaultSettings();
        }
    }

    // Resets all settings to their default values (from ScriptableObject)
    public void ResetToDefaultSettings()
    {
        graphicsSettingsSO.Data.SetToPreset(FallbackPreset);
        ApplyAllSettings();
    }

    public void SetQualityPreset(int index)
    {
        if (index >= 0 && index < presetSettings.presets.Count)
        {
            PresetSetting preset = presetSettings.presets[index];
            graphicsSettingsSO.Data.shadowDistance = preset.settings.shadowDistance;
            graphicsSettingsSO.Data.shadowQuality = preset.settings.shadowQuality;
            graphicsSettingsSO.Data.shadowResolution = preset.settings.shadowResolution;
            graphicsSettingsSO.Data.shadowCascades = preset.settings.shadowCascades;
            graphicsSettingsSO.Data.ssaoEnabled = preset.settings.ssaoEnabled;
            graphicsSettingsSO.Data.ssaoQuality = preset.settings.ssaoQuality;
            graphicsSettingsSO.Data.ssgiEnabled = preset.settings.ssgiEnabled;
            graphicsSettingsSO.Data.ssgiQuality = preset.settings.ssgiQuality;
            graphicsSettingsSO.Data.volumetricLightingEnabled = preset.settings.volumetricLightingEnabled;
            graphicsSettingsSO.Data.volumetricLightingQuality = preset.settings.volumetricLightingQuality;
            graphicsSettingsSO.Data.bloomEnabled = preset.settings.bloomEnabled;
            graphicsSettingsSO.Data.bloomIntensity = preset.settings.bloomIntensity;
            graphicsSettingsSO.Data.motionBlurEnabled = preset.settings.motionBlurEnabled;
            graphicsSettingsSO.Data.motionBlurIntensity = preset.settings.motionBlurIntensity;
            graphicsSettingsSO.Data.depthOfFieldEnabled = preset.settings.depthOfFieldEnabled;
            graphicsSettingsSO.Data.depthOfFieldFocusDistance = preset.settings.depthOfFieldFocusDistance;
            graphicsSettingsSO.Data.vignetteEnabled = preset.settings.vignetteEnabled;
            graphicsSettingsSO.Data.vignetteIntensity = preset.settings.vignetteIntensity;
            graphicsSettingsSO.Data.textureQuality = (SettingsData.TextureQuality)Enum.Parse(typeof(SettingsData.TextureQuality), preset.settings.textureQuality.ToString());
            graphicsSettingsSO.Data.lodBias = preset.settings.lodBias;

            graphicsSettingsSO.Data.qualityPreset = (SettingsData.QualityPreset)index;
        }

        ApplyAllSettings();
    }

    private void LoadPresetSettings()
    {
        string path = "Assets/Settings/presets.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            presetSettings = JsonUtility.FromJson<PresetSettings>(jsonString);
        }
        else
        {
            Debug.LogError("Preset settings file not found: " + path);
        }
    }
    
    // Додано методи для інкапсуляції
    public void SetTextureQuality(int quality)
    {
        if (quality < 0 || quality > 3)
        {
            Debug.LogError("Invalid texture quality index: " + quality);
            return;
        }
        graphicsSettingsSO.Data.textureQuality = (SettingsData.TextureQuality)quality;
        ScheduleApply();
    }
}
