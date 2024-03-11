using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance { get; private set; }
    [SerializeField] private RectTransform canvasRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundReactTransform;
    private RectTransform rectTransform;
    private TooltipTimer tooltipTimer;
    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundReactTransform = transform.Find("background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        HandleFallowMouse();

        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if( tooltipTimer.timer <= 0) {
            Hide();
            }
        }
    }

    private void HandleFallowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundReactTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundReactTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundReactTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundReactTransform.rect.height;
        }


        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        backgroundReactTransform.sizeDelta = textSize + padding;
    }
    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);
        HandleFallowMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public class TooltipTimer
    {
        public float timer;
    }
}
