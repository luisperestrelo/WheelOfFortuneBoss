using System;
using TMPro;
using UnityEngine;

public class HoverTipManager : MonoBehaviour
{
    
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    // Start is called before the first frame update


    private void OnEnable()
    {
        OnMouseHover += Show;
        OnMouseLoseFocus += Hide;
    }


    private void OnDisable()
    {
        OnMouseHover -= Show;
        OnMouseLoseFocus -= Hide;
    }
    
    
    void Start()
    {
        Hide();

    }
    
    private void Show(string text, Vector2 mousePosition)
    {
        tipText.text = text;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight > 200 ? 200 : tipText.preferredHeight);
        
        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePosition.x + tipWindow.sizeDelta.x * 2, mousePosition.y);
    }

    private void Hide()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
    
 

}
