using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI badText;
    [SerializeField] private RectTransform rect;

    private RectTransform textRect;

    private float minHeight;

    public RectTransform Rect { get => rect; private set => rect = value; }

    public void SetContent(Card card)
    {
        nameText.text = card.cardName;
        rarityText.text = card.rarity.ToString();
        rarityText.color = GetRarityColor(card.rarity);
        badText.text = card.downSideDescription;
        descriptionText.text = card.description;

        Rect.sizeDelta = new Vector2(134, minHeight + descriptionText.preferredHeight + badText.preferredHeight + 16);
    }

    private Color GetRarityColor(CardRarity rarity)
    {

        return rarity switch
        {
            CardRarity.Common => Color.white,
            CardRarity.Rare => Color.blue,
            CardRarity.Epic => Color.magenta,
            _ => Color.white
        };

    }
    



    private void Awake()
    {
        minHeight = rect.sizeDelta.y;
        textRect = descriptionText.GetComponent<RectTransform>();
    }
}
