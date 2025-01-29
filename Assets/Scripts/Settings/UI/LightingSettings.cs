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
        InitializeToggle(ssaoToggle, Manager.Data.ssaoEnabled, OnSSAOEnabledChanged);

        // Initialize SSAO Quality Dropdown
        InitializeDropdown(ssaoQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("SSAOQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("SSAOQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("SSAOQuality_High")
        }, Manager.Data.ssaoQuality, OnSSAOQualityChanged);

        // Initialize SSGI Toggle
        InitializeToggle(ssgiToggle, Manager.Data.ssgiEnabled, OnSSGIEnabledChanged);

        // Initialize SSGI Quality Dropdown
        InitializeDropdown(ssgiQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("SSGIQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("SSGIQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("SSGIQuality_High")
        }, Manager.Data.ssgiQuality, OnSSGIQualityChanged);

        // Initialize Volumetric Lighting Toggle
        InitializeToggle(volumetricLightingToggle, Manager.Data.volumetricLightingEnabled, OnVolumetricLightingEnabledChanged);

        // Initialize Volumetric Lighting Quality Dropdown
        InitializeDropdown(volumetricLightingQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("VolumetricLightingQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("VolumetricLightingQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("VolumetricLightingQuality_High")
        }, Manager.Data.volumetricLightingQuality, OnVolumetricLightingQualityChanged);

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
        ssaoToggle.SetIsOnWithoutNotify(Manager.Data.ssaoEnabled);
        ssaoQualityDropdown.SetValueWithoutNotify(Manager.Data.ssaoQuality);
        ssaoQualityDropdown.RefreshShownValue();
        ssgiToggle.SetIsOnWithoutNotify(Manager.Data.ssgiEnabled);
        ssgiQualityDropdown.SetValueWithoutNotify(Manager.Data.ssgiQuality);
        ssgiQualityDropdown.RefreshShownValue();
        volumetricLightingToggle.SetIsOnWithoutNotify(Manager.Data.volumetricLightingEnabled);
        volumetricLightingQualityDropdown.SetValueWithoutNotify(Manager.Data.volumetricLightingQuality);
        volumetricLightingQualityDropdown.RefreshShownValue();
    }

    // --- Event Handlers for UI changes ---

    public void OnSSAOEnabledChanged(bool isEnabled)
    {
        Manager.SetSSAOEnabled(isEnabled);
    }

    public void OnSSAOQualityChanged(int index)
    {
        Manager.SetSSAOQuality(index);
    }

    public void OnSSGIEnabledChanged(bool isEnabled)
    {
        Manager.SetSSGIEnabled(isEnabled);
    }

    public void OnSSGIQualityChanged(int index)
    {
        Manager.SetSSGIQuality(index);
    }

    public void OnVolumetricLightingEnabledChanged(bool isEnabled)
    {
        Manager.SetVolumetricLightingEnabled(isEnabled);
    }

    public void OnVolumetricLightingQualityChanged(int index)
    {
        Manager.SetVolumetricLightingQuality(index);
    }
}