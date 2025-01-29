using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightingSettings : SettingsPanel
{
    [Header("UI Elements")]
    [SerializeField] private Toggle ssaoToggle;
    [SerializeField] private TMP_Dropdown ssaoQualityDropdown;
    [SerializeField] private Toggle ssgiToggle;
    [SerializeField] private TMP_Dropdown ssgiQualityDropdown;
    [SerializeField] private Toggle volumetricLightingToggle;
    [SerializeField] private TMP_Dropdown volumetricLightingQualityDropdown;

    [Header("Tooltips")]
    [SerializeField] private TooltipTrigger ssaoTooltipTrigger;
    [SerializeField] private TooltipTrigger ssaoQualityTooltipTrigger;
    [SerializeField] private TooltipTrigger ssgiTooltipTrigger;
    [SerializeField] private TooltipTrigger ssgiQualityTooltipTrigger;
    [SerializeField] private TooltipTrigger volumetricLightingTooltipTrigger;
    [SerializeField] private TooltipTrigger volumetricLightingQualityTooltipTrigger;

    public override void InitializeUI()
    {
        // Initialize SSAO Toggle
        InitializeToggle(ssaoToggle, Manager.graphicsSettingsSO.Data.SSAOEnabled, OnSSAOEnabledChanged);

        // Initialize SSAO Quality Dropdown
        InitializeDropdown(ssaoQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("SSAOQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("SSAOQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("SSAOQuality_High")
        }, Manager.graphicsSettingsSO.Data.SSAOQuality, OnSSAOQualityChanged);

        // Initialize SSGI Toggle
        InitializeToggle(ssgiToggle, Manager.graphicsSettingsSO.Data.SSGIEnabled, OnSSGIEnabledChanged);

        // Initialize SSGI Quality Dropdown
        InitializeDropdown(ssgiQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("SSGIQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("SSGIQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("SSGIQuality_High")
        }, Manager.graphicsSettingsSO.Data.SSGIQuality, OnSSGIQualityChanged);

        // Initialize Volumetric Lighting Toggle
        InitializeToggle(volumetricLightingToggle, Manager.graphicsSettingsSO.Data.VolumetricLightingEnabled, OnVolumetricLightingEnabledChanged);

        // Initialize Volumetric Lighting Quality Dropdown
        InitializeDropdown(volumetricLightingQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("VolumetricLightingQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("VolumetricLightingQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("VolumetricLightingQuality_High")
        }, Manager.graphicsSettingsSO.Data.VolumetricLightingQuality, OnVolumetricLightingQualityChanged);

        // Setup tooltips
        SetupTooltip(ssaoTooltipTrigger, "SSAO_Tooltip");
        SetupTooltip(ssaoQualityTooltipTrigger, "SSAOQuality_Tooltip");
        SetupTooltip(ssgiTooltipTrigger, "SSGI_Tooltip");
        SetupTooltip(ssgiQualityTooltipTrigger, "SSGIQuality_Tooltip");
        SetupTooltip(volumetricLightingTooltipTrigger, "VolumetricLighting_Tooltip");
        SetupTooltip(volumetricLightingQualityTooltipTrigger, "VolumetricLightingQuality_Tooltip");
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        ssaoToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.SSAOEnabled);
        ssaoQualityDropdown.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.SSAOQuality);
        ssaoQualityDropdown.RefreshShownValue();
        ssgiToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.SSGIEnabled);
        ssgiQualityDropdown.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.SSGIQuality);
        ssgiQualityDropdown.RefreshShownValue();
        volumetricLightingToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.VolumetricLightingEnabled);
        volumetricLightingQualityDropdown.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.VolumetricLightingQuality);
        volumetricLightingQualityDropdown.RefreshShownValue();
    }

    // --- Event Handlers for UI changes ---

    public void OnSSAOEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.SSAOEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnSSAOQualityChanged(int index)
    {
        Manager.graphicsSettingsSO.Data.SSAOQuality = index;
        Manager.ScheduleApply();
    }

    public void OnSSGIEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.SSGIEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnSSGIQualityChanged(int index)
    {
        Manager.graphicsSettingsSO.Data.SSGIQuality = index;
        Manager.ScheduleApply();
    }

    public void OnVolumetricLightingEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.VolumetricLightingEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnVolumetricLightingQualityChanged(int index)
    {
        Manager.graphicsSettingsSO.Data.VolumetricLightingQuality = index;
        Manager.ScheduleApply();
    }
}