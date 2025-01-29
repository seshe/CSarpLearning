using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public abstract class SettingsPanel : MonoBehaviour
{
    protected GraphicsSettingsManager Manager;

    protected virtual void Awake()
    {
        Manager = FindObjectOfType<GraphicsSettingsManager>();
        if (Manager == null)
        {
            Debug.LogError("GraphicsSettingsManager not found in the scene!");
        }

        GraphicsSettingsManager.OnSettingsChanged += UpdateUI;
        LocalizationManager.Instance.OnLanguageChanged += UpdateUI;
    }

    protected virtual void OnDestroy()
    {
        GraphicsSettingsManager.OnSettingsChanged -= UpdateUI;
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateUI;
        }
    }

    public abstract void InitializeUI();
    protected abstract void UpdateUI();
    
    protected virtual void SetupTooltip(TooltipTrigger trigger, string tooltipKey)
    {
        if (trigger != null)
        {
            trigger.tooltipKey = tooltipKey;
        }
    }

    protected virtual void InitializeDropdown(TMP_Dropdown dropdown, List<string> options, int value, UnityEngine.Events.UnityAction<int> callback)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(value);
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(callback);
    }

    protected virtual void InitializeSlider(Slider slider, float minValue, float maxValue, float value, UnityEngine.Events.UnityAction<float> callback)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.SetValueWithoutNotify(value);
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(callback);
}

    protected virtual void InitializeToggle(Toggle toggle, bool value, UnityEngine.Events.UnityAction<bool> callback)
    {
        toggle.SetIsOnWithoutNotify(value);
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(callback);
    }
}