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
        }, (int)Manager.graphicsSettingsSO.Data.textureQuality, OnTextureQualityChanged);

        // Initialize LOD Bias Slider
        InitializeSlider(lodBiasSlider, 0.5f, 2f, Manager.graphicsSettingsSO.Data.LODBias, OnLODBiasChanged);

        // Setup tooltips
        SetupTooltip(textureQualityTooltipTrigger, "TextureQuality_Tooltip");
        SetupTooltip(lodBiasTooltipTrigger, "LODBias_Tooltip");
    }

    protected override void UpdateUI()
    {
        // Update UI elements with current settings
        textureQualityDropdown.SetValueWithoutNotify((int)Manager.graphicsSettingsSO.Data.textureQuality);
        textureQualityDropdown.RefreshShownValue();
        lodBiasSlider.SetValueWithoutNotify(Manager.graphicsSettingsSO.Data.LODBias);
    }

    // --- Event Handlers for UI changes ---

    public void OnTextureQualityChanged(int index)
    {
        if (!System.Enum.IsDefined(typeof(SettingsData.TextureQuality), index))
        {
            Debug.LogError("Invalid texture quality index: " + index);
            return;
        }

        Manager.SetTextureQuality(index);
    }

    public void OnLODBiasChanged(float value)
    {
        Manager.graphicsSettingsSO.Data.LODBias = value;
        Manager.ScheduleApply();
    }
}