using UnityEngine;
using TMPro;

public class ShadowSettings : SettingsPanel
{
    [Header("UI Elements")]
    [SerializeField] private Slider shadowDistanceSlider;
    [SerializeField] private TMP_Dropdown shadowQualityDropdown;
    [SerializeField] private TMP_Dropdown shadowResolutionDropdown;
    [SerializeField] private TMP_Dropdown shadowCascadesDropdown;

    [Header("Tooltips")]
    [SerializeField] private TooltipTrigger shadowDistanceTooltipTrigger;
    [SerializeField] private TooltipTrigger shadowQualityTooltipTrigger;
    [SerializeField] private TooltipTrigger shadowResolutionTooltipTrigger;
    [SerializeField] private TooltipTrigger shadowCascadesTooltipTrigger;

    public override void InitializeUI()
    {
        // Initialize Shadow Distance Slider
        InitializeSlider(shadowDistanceSlider, 50f, 1000f, Manager.Data.shadowDistance, OnShadowDistanceChanged);

        // Initialize Shadow Quality Dropdown
        InitializeDropdown(shadowQualityDropdown, new System.Collections.Generic.List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("ShadowQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("ShadowQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("ShadowQuality_High")
        }, (int)Manager.Data.shadowQuality, OnShadowQualityChanged);

        // Initialize Shadow Resolution Dropdown
        InitializeDropdown(shadowResolutionDropdown, new System.Collections.Generic.List<string> { "512", "1024", "2048", "4096" },
            GetResolutionIndex(Manager.Data.shadowResolution), OnShadowResolutionChanged);

        // Initialize Shadow Cascades Dropdown
        InitializeDropdown(shadowCascadesDropdown, new System.Collections.Generic.List<string> { "1", "2", "4" },
            GetCascadeIndex(Manager.Data.shadowCascades), OnShadowCascadesChanged);

        // Setup tooltips
        SetupTooltip(shadowDistanceTooltipTrigger, "ShadowDistance_Tooltip");
        SetupTooltip(shadowQualityTooltipTrigger, "ShadowQuality_Tooltip");
        SetupTooltip(shadowResolutionTooltipTrigger, "ShadowResolution_Tooltip");
        SetupTooltip(shadowCascadesTooltipTrigger, "ShadowCascades_Tooltip");
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        shadowDistanceSlider.SetValueWithoutNotify(Manager.Data.shadowDistance);
        shadowQualityDropdown.SetValueWithoutNotify((int)Manager.Data.shadowQuality);
        shadowQualityDropdown.RefreshShownValue();
        shadowResolutionDropdown.SetValueWithoutNotify(GetResolutionIndex(Manager.Data.shadowResolution));
        shadowResolutionDropdown.RefreshShownValue();
        shadowCascadesDropdown.SetValueWithoutNotify(GetCascadeIndex(Manager.Data.shadowCascades));
        shadowCascadesDropdown.RefreshShownValue();
    }

    // --- Event Handlers for UI changes ---

    public void OnShadowDistanceChanged(float value)
    {
        Manager.SetShadowDistance(value);
    }

    public void OnShadowQualityChanged(int value)
    {
        Manager.SetShadowQuality(value);
    }

    public void OnShadowResolutionChanged(int index)
    {
        Manager.SetShadowResolution(GetResolutionFromIndex(index));
    }

    public void OnShadowCascadesChanged(int index)
    {
        Manager.SetShadowCascades(GetCascadeCountFromIndex(index));
    }

    // Helper functions to convert between dropdown index and resolution/cascade values
    private int GetResolutionIndex(int resolution)
    {
        switch (resolution)
        {
            case 512: return 0;
            case 1024: return 1;
            case 2048: return 2;
            case 4096: return 3;
            default: return 2; // Default to 2048
        }
    }

    private int GetResolutionFromIndex(int index)
    {
        switch (index)
        {
            case 0: return 512;
            case 1: return 1024;
            case 2: return 2048;
            case 3: return 4096;
            default: return 2048;
        }
    }

    private int GetCascadeIndex(int cascadeCount)
    {
        switch (cascadeCount)
        {
            case 1: return 0;
            case 2: return 1;
            case 4: return 2;
            default: return 2; // Default to 4
        }
    }

    private int GetCascadeCountFromIndex(int index)
    {
        switch (index)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 4;
            default: return 4;
        }
    }
}