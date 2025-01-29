using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GeneralSettings : SettingsPanel
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private TMP_Dropdown qualityPresetDropdown;

    [Header("Tooltips")]
    [SerializeField] private TooltipTrigger resolutionTooltipTrigger;
    [SerializeField] private TooltipTrigger fullscreenTooltipTrigger;
    [SerializeField] private TooltipTrigger vsyncTooltipTrigger;
    [SerializeField] private TooltipTrigger qualityPresetTooltipTrigger;

    private Resolution[] resolutions;

    private void Start()
    {
        // Instantiate Tooltip prefab
        Instantiate(Resources.Load<GameObject>("TooltipPanel"), FindObjectOfType<Canvas>().transform);
    }

    public override void InitializeUI()
    {
        // Setup Resolution Dropdown
        SetupResolutionDropdown();

        // Setup Fullscreen Toggle
        InitializeToggle(fullscreenToggle, Manager.graphicsSettingsSO.Data.IsFullscreen, OnFullscreenModeChanged);

        // Setup VSync Toggle
        InitializeToggle(vsyncToggle, Manager.graphicsSettingsSO.Data.VSync, OnVSyncChanged);

        // Setup Quality Preset Dropdown
        SetupQualityPresetDropdown();

        // Setup tooltips
        SetupTooltip(resolutionTooltipTrigger, "Resolution_Tooltip");
        SetupTooltip(fullscreenTooltipTrigger, "Fullscreen_Tooltip");
        SetupTooltip(vsyncTooltipTrigger, "VSync_Tooltip");
        SetupTooltip(qualityPresetTooltipTrigger, "QualityPreset_Tooltip");
    }

    private void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.First())
            .ToArray();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Manager.graphicsSettingsSO.Data.Resolution.width &&
                resolutions[i].height == Manager.graphicsSettingsSO.Data.Resolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        InitializeDropdown(resolutionDropdown, options, currentResolutionIndex, OnResolutionChanged);
    }

    private void SetupQualityPresetDropdown()
    {
        List<string> presetOptions = new List<string>();
        foreach (var preset in Manager.presetSettings.presets)
        {
            presetOptions.Add(LocalizationManager.Instance.GetLocalizedText("Preset_" + preset.name));
        }

        InitializeDropdown(qualityPresetDropdown, presetOptions, (int)Manager.graphicsSettingsSO.Data.qualityPreset, OnQualityPresetChanged);
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Manager.graphicsSettingsSO.Data.Resolution.width &&
                resolutions[i].height == Manager.graphicsSettingsSO.Data.Resolution.height)
            {
                resolutionDropdown.SetValueWithoutNotify(i);
                break;
            }
        }
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = Manager.graphicsSettingsSO.Data.IsFullscreen;
        vsyncToggle.isOn = Manager.graphicsSettingsSO.Data.VSync;

        // Update Quality Preset Dropdown
        qualityPresetDropdown.SetValueWithoutNotify((int)Manager.graphicsSettingsSO.Data.qualityPreset);
        qualityPresetDropdown.RefreshShownValue();
    }

    // --- Event Handlers for UI changes ---

    public void OnResolutionChanged(int resolutionIndex)
    {
        Manager.graphicsSettingsSO.Data.Resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
        Manager.ScheduleApply();
    }

    public void OnFullscreenModeChanged(bool isFullscreen)
    {
        Manager.graphicsSettingsSO.Data.IsFullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
        Manager.ScheduleApply();
    }

    public void OnVSyncChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.VSync = isEnabled;
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        Manager.ScheduleApply();
    }

    public void OnQualityPresetChanged(int index)
    {
        Manager.graphicsSettingsSO.Data.qualityPreset = (SettingsData.QualityPreset)index;
        Manager.SetQualityPreset(index);
        Manager.ScheduleApply();
    }
}