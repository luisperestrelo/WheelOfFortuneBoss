using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsDisplayItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public RectTransform Rect;
    [SerializeField] public TextMeshProUGUI Text;

    private StatType statType;


    public void Initialize(StatType type, string text, float maxWidth, Vector2 position, Color color)
    {
        Initialize(type, text, maxWidth, position);
        Text.color = color;
    }
    public void Initialize(StatType type, string text, float maxWidth, Vector2 position)
    {
        statType = type;
        Text.text = text;
        Rect.sizeDelta = new Vector2(maxWidth - 16, Rect.sizeDelta.y);
        Rect.anchoredPosition = position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StatsDisplay.OnMouseHover?.Invoke(statType, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StatsDisplay.OnMouseLoseFocus?.Invoke();
    }
}