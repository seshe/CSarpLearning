using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform tooltipBackground;

    public static Tooltip Instance { get; private set; }

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        canvasGroup = GetComponent<CanvasGroup>();
        HideTooltip();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipBackground.sizeDelta = new Vector2(tooltipText.preferredWidth + 16, tooltipText.preferredHeight + 16); // + padding
        canvasGroup.alpha = 1;
    }

    public void HideTooltip()
    {
        canvasGroup.alpha = 0;
    }
}
