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
        InitializeToggle(fullscreenToggle, Manager.Data.isFullscreen, OnFullscreenModeChanged);

        // Setup VSync Toggle
        InitializeToggle(vsyncToggle, Manager.Data.vsync, OnVSyncChanged);

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
        // Filter out resolutions with refresh rate of 0
        resolutions = Screen.resolutions
            .Where(r => r.refreshRate != 0)
            .GroupBy(r => new { r.width, r.height, r.refreshRate })
            .Select(g => g.First())
            .ToArray();

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            // Format the resolution string to include refresh rate
            string option = $"{resolutions[i].width} x {resolutions[i].height} @ {resolutions[i].refreshRate}Hz";
            options.Add(option);

            if (resolutions[i].width == Manager.Data.Resolution.width &&
                resolutions[i].height == Manager.Data.Resolution.height)
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

        InitializeDropdown(qualityPresetDropdown, presetOptions, (int)Manager.Data.qualityPreset, OnQualityPresetChanged);
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Manager.Data.Resolution.width &&
                resolutions[i].height == Manager.Data.Resolution.height)
            {
                resolutionDropdown.SetValueWithoutNotify(i);
                break;
            }
        }
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.SetIsOnWithoutNotify(Manager.Data.isFullscreen);
        vsyncToggle.SetIsOnWithoutNotify(Manager.Data.vsync);

        // Update Quality Preset Dropdown
        qualityPresetDropdown.SetValueWithoutNotify((int)Manager.Data.qualityPreset);
        qualityPresetDropdown.RefreshShownValue();
    }

    // --- Event Handlers for UI changes ---

    public void OnResolutionChanged(int resolutionIndex)
    {
        Manager.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Manager.Data.isFullscreen);
    }

    public void OnFullscreenModeChanged(bool isFullscreen)
    {
        Manager.SetFullscreen(isFullscreen);
    }

    public void OnVSyncChanged(bool isEnabled)
    {
        Manager.SetVSync(isEnabled);
    }

    public void OnQualityPresetChanged(int index)
    {
        Manager.SetQualityPreset(index);
    }
}