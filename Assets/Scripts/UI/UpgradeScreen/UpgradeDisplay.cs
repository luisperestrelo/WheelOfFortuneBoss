using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image selectionImage;
    [SerializeField] private FieldCardDisplay fieldCardDisplay;
    [SerializeField] private StatUpgradeCardDisplay statUpgradeDisplay;
    [SerializeField] private AudioSource menuSource;
    [SerializeField] private AudioClip hoverSfx;

    [SerializeField] private CardTooltip tooltip;

    private Card card;

    public void Display(Card card)
    {
        this.card = card;
        if (card.cardType == CardType.Field)
            DisplayActive(card as FieldCard);
        else
            DisplayPassive(card);
    }

    public void Select()
    {
        if (card.cardType == CardType.Field)
            fieldCardDisplay.Select();
        else
            statUpgradeDisplay.Select();
    }

    public void Deselect()
    {
        if (card.cardType == CardType.Field)
            fieldCardDisplay.Deselect();
        else
            statUpgradeDisplay.Deselect();
    }

    void Start()
    {
        statUpgradeDisplay.gameObject.SetActive(false);
        fieldCardDisplay.gameObject.SetActive(false);
    }

    private void DisplayPassive(Card card)
    {
        statUpgradeDisplay.gameObject.SetActive(true);
        fieldCardDisplay.gameObject.SetActive(false);
        statUpgradeDisplay.SetValues(card);
    }

    private void DisplayActive(FieldCard card)
    {
        fieldCardDisplay.gameObject.SetActive(true);
        statUpgradeDisplay.gameObject.SetActive(false);
        fieldCardDisplay.SetValues(card);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip(card, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip(Card card, Vector2 position)
    {
        menuSource.PlayOneShot(hoverSfx, 0.7f);
        var xOffset = position.x < Screen.width / 2 ? tooltip.Rect.sizeDelta.x : -tooltip.Rect.sizeDelta.x;
        tooltip.transform.position = new Vector2(position.x + xOffset, position.y);

        tooltip.SetContent(card);
        tooltip.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }
}