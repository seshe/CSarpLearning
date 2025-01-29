using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextureAndDetailSettings : SettingsPanel
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown textureQualityDropdown;
    [SerializeField] private Slider lodBiasSlider;

    [Header("Tooltips")]
    [SerializeField] private TooltipTrigger textureQualityTooltipTrigger;
    [SerializeField] private TooltipTrigger lodBiasTooltipTrigger;

    public override void InitializeUI()
    {
        // Initialize Texture Quality Dropdown
        InitializeDropdown(textureQualityDropdown, new List<string>
        {
            LocalizationManager.Instance.GetLocalizedText("TextureQuality_Low"),
            LocalizationManager.Instance.GetLocalizedText("TextureQuality_Medium"),
            LocalizationManager.Instance.GetLocalizedText("TextureQuality_High"),
            LocalizationManager.Instance.GetLocalizedText("TextureQuality_Ultra")
        }, (int)Manager.Data.textureQuality, OnTextureQualityChanged);

        // Initialize LOD Bias Slider
        InitializeSlider(lodBiasSlider, 0.5f, 2f, Manager.Data.lodBias, OnLODBiasChanged);

        // Setup tooltips
        SetupTooltip(textureQualityTooltipTrigger, "TextureQuality_Tooltip");
        SetupTooltip(lodBiasTooltipTrigger, "LODBias_Tooltip");
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        textureQualityDropdown.SetValueWithoutNotify((int)Manager.Data.textureQuality);
        textureQualityDropdown.RefreshShownValue();
        lodBiasSlider.SetValueWithoutNotify(Manager.Data.lodBias);
    }

    // --- Event Handlers for UI changes ---

    public void OnTextureQualityChanged(int index)
    {
        Manager.SetTextureQuality(index);
    }

    public void OnLODBiasChanged(float value)
    {
        Manager.SetLODBias(value);
    }
}