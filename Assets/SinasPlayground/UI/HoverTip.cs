
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string topToShow;
    private float timeToWait = 0.0f;
    private void Start()
    {
        Debug.Log("HoverTip");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTipManager.OnMouseLoseFocus?.Invoke();
    }

    private void ShowMessage( )
    {
        HoverTipManager.OnMouseHover?.Invoke(topToShow, Input.mousePosition);
    }


    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        ShowMessage();
    }
    
}
