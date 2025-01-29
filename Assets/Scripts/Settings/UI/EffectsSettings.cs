using UnityEngine;
using UnityEngine.UI;

public class EffectsSettings : SettingsPanel
{
    [Header("UI Elements")]
    [SerializeField] private Toggle bloomToggle;
    [SerializeField] private Slider bloomIntensitySlider;
    [SerializeField] private Toggle motionBlurToggle;
    [SerializeField] private Slider motionBlurIntensitySlider;
    [SerializeField] private Toggle depthOfFieldToggle;
    [SerializeField] private Slider depthOfFieldIntensitySlider;
    [SerializeField] private Toggle vignetteToggle;
    [SerializeField] private Slider vignetteIntensitySlider;

    [Header("Tooltips")]
    [SerializeField] private TooltipTrigger bloomTooltipTrigger;
    [SerializeField] private TooltipTrigger bloomIntensityTooltipTrigger;
    [SerializeField] private TooltipTrigger motionBlurTooltipTrigger;
    [SerializeField] private TooltipTrigger motionBlurIntensityTooltipTrigger;
    [SerializeField] private TooltipTrigger depthOfFieldTooltipTrigger;
    [SerializeField] private TooltipTrigger depthOfFieldIntensityTooltipTrigger;
    [SerializeField] private TooltipTrigger vignetteTooltipTrigger;
    [SerializeField] private TooltipTrigger vignetteIntensityTooltipTrigger;

    public override void InitializeUI()
    {
        // Initialize Bloom Toggle
        InitializeToggle(bloomToggle, Manager.graphicsSettingsSO.Data.BloomEnabled, OnBloomEnabledChanged);

        // Initialize Bloom Intensity Slider
        InitializeSlider(bloomIntensitySlider, 0f, 1f, Manager.graphicsSettingsSO.Data.BloomIntensity, OnBloomIntensityChanged);

        // Initialize Motion Blur Toggle
        InitializeToggle(motionBlurToggle, Manager.graphicsSettingsSO.Data.MotionBlurEnabled, OnMotionBlurEnabledChanged);

        // Initialize Motion Blur Intensity Slider
        InitializeSlider(motionBlurIntensitySlider, 0f, 1f, Manager.graphicsSettingsSO.Data.MotionBlurIntensity, OnMotionBlurIntensityChanged);

        // Initialize Depth of Field Toggle
        InitializeToggle(depthOfFieldToggle, Manager.graphicsSettingsSO.Data.DepthOfFieldEnabled, OnDepthOfFieldEnabledChanged);

        // Initialize Depth of Field Intensity Slider
        InitializeSlider(depthOfFieldIntensitySlider, 0f, 10f, Manager.graphicsSettingsSO.Data.DepthOfFieldFocusDistance, OnDepthOfFieldIntensityChanged);

        // Initialize Vignette Toggle
        InitializeToggle(vignetteToggle, Manager.graphicsSettingsSO.Data.VignetteEnabled, OnVignetteEnabledChanged);

        // Initialize Vignette Intensity Slider
        InitializeSlider(vignetteIntensitySlider, 0f, 1f, Manager.graphicsSettingsSO.Data.VignetteIntensity, OnVignetteIntensityChanged);

        // Setup tooltips
        SetupTooltip(bloomTooltipTrigger, "Bloom_Tooltip");
        SetupTooltip(bloomIntensityTooltipTrigger, "BloomIntensity_Tooltip");
        SetupTooltip(motionBlurTooltipTrigger, "MotionBlur_Tooltip");
        SetupTooltip(motionBlurIntensityTooltipTrigger, "MotionBlurIntensity_Tooltip");
        SetupTooltip(depthOfFieldTooltipTrigger, "DepthOfField_Tooltip");
        SetupTooltip(depthOfFieldIntensityTooltipTrigger, "DepthOfFieldIntensity_Tooltip");
        SetupTooltip(vignetteTooltipTrigger, "Vignette_Tooltip");
        SetupTooltip(vignetteIntensityTooltipTrigger, "VignetteIntensity_Tooltip");
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        bloomToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.BloomEnabled);
        bloomIntensitySlider.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.BloomIntensity);
        motionBlurToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.MotionBlurEnabled);
        motionBlurIntensitySlider.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.MotionBlurIntensity);
        depthOfFieldToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.DepthOfFieldEnabled);
        depthOfFieldIntensitySlider.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.DepthOfFieldFocusDistance);
        vignetteToggle.SetIsOnWithoutNotify(Manager.graphicsSettingsSO.Data.VignetteEnabled);
        vignetteIntensitySlider.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.VignetteIntensity);
    }

    // --- Event Handlers for UI changes ---

    public void OnBloomEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.BloomEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnBloomIntensityChanged(float value)
    {
        Manager.graphicsSettingsSO.Data.BloomIntensity = value;
        Manager.ScheduleApply();
    }

    public void OnMotionBlurEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.MotionBlurEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnMotionBlurIntensityChanged(float value)
    {
        Manager.graphicsSettingsSO.Data.MotionBlurIntensity = value;
        Manager.ScheduleApply();
    }

    public void OnDepthOfFieldEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.DepthOfFieldEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnDepthOfFieldIntensityChanged(float value)
    {
        Manager.graphicsSettingsSO.Data.DepthOfFieldFocusDistance = value;
        Manager.ScheduleApply();
    }

    public void OnVignetteEnabledChanged(bool isEnabled)
    {
        Manager.graphicsSettingsSO.Data.VignetteEnabled = isEnabled;
        Manager.ScheduleApply();
    }

    public void OnVignetteIntensityChanged(float value)
    {
        Manager.graphicsSettingsSO.Data.VignetteIntensity = value;
        Manager.ScheduleApply();
    }
}