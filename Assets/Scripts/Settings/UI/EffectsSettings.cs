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
        InitializeToggle(bloomToggle, Manager.Data.bloomEnabled, OnBloomEnabledChanged);

        // Initialize Bloom Intensity Slider
        InitializeSlider(bloomIntensitySlider, 0f, 1f, Manager.Data.bloomIntensity, OnBloomIntensityChanged);

        // Initialize Motion Blur Toggle
        InitializeToggle(motionBlurToggle, Manager.Data.motionBlurEnabled, OnMotionBlurEnabledChanged);

        // Initialize Motion Blur Intensity Slider
        InitializeSlider(motionBlurIntensitySlider, 0f, 1f, Manager.Data.motionBlurIntensity, OnMotionBlurIntensityChanged);

        // Initialize Depth of Field Toggle
        InitializeToggle(depthOfFieldToggle, Manager.Data.depthOfFieldEnabled, OnDepthOfFieldEnabledChanged);

        // Initialize Depth of Field Intensity Slider
        InitializeSlider(depthOfFieldIntensitySlider, 0f, 10f, Manager.Data.depthOfFieldFocusDistance, OnDepthOfFieldIntensityChanged);

        // Initialize Vignette Toggle
        InitializeToggle(vignetteToggle, Manager.Data.vignetteEnabled, OnVignetteEnabledChanged);

        // Initialize Vignette Intensity Slider
        InitializeSlider(vignetteIntensitySlider, 0f, 1f, Manager.Data.vignetteIntensity, OnVignetteIntensityChanged);

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
        bloomToggle.SetIsOnWithoutNotify(Manager.Data.bloomEnabled);
        bloomIntensitySlider.SetValueWithoutNotify(Manager.Data.bloomIntensity);
        motionBlurToggle.SetIsOnWithoutNotify(Manager.Data.motionBlurEnabled);
        motionBlurIntensitySlider.SetValueWithoutNotify(Manager.Data.motionBlurIntensity);
        depthOfFieldToggle.SetIsOnWithoutNotify(Manager.Data.depthOfFieldEnabled);
        depthOfFieldIntensitySlider.SetValueWithoutNotify(Manager.Data.depthOfFieldFocusDistance);
        vignetteToggle.SetIsOnWithoutNotify(Manager.Data.vignetteEnabled);
        vignetteIntensitySlider.SetValueWithoutNotify(Manager.Data.vignetteIntensity);
    }

    // --- Event Handlers for UI changes ---

    public void OnBloomEnabledChanged(bool isEnabled)
    {
        Manager.SetBloomEnabled(isEnabled);
    }

    public void OnBloomIntensityChanged(float value)
    {
        Manager.SetBloomIntensity(value);
    }

    public void OnMotionBlurEnabledChanged(bool isEnabled)
    {
        Manager.SetMotionBlurEnabled(isEnabled);
    }

    public void OnMotionBlurIntensityChanged(float value)
    {
        Manager.SetMotionBlurIntensity(value);
    }

    public void OnDepthOfFieldEnabledChanged(bool isEnabled)
    {
        Manager.SetDepthOfFieldEnabled(isEnabled);
    }

    public void OnDepthOfFieldIntensityChanged(float value)
    {
        Manager.SetDepthOfFieldFocusDistance(value);
    }

    public void OnVignetteEnabledChanged(bool isEnabled)
    {
        Manager.SetVignetteEnabled(isEnabled);
    }

    public void OnVignetteIntensityChanged(float value)
    {
        Manager.SetVignetteIntensity(value);
    }
}