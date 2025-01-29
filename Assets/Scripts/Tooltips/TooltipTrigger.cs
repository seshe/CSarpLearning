using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tooltipKey;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.ShowTooltip(LocalizationManager.Instance.GetLocalizedText(tooltipKey));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.HideTooltip();
    }
}
