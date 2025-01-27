using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeDisplayVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image icon;
    [SerializeField] private List<GameObject> rarityFxObjects;

    public void SetValues(Card card, bool replaceIcon = false)
    {
        nameText.text = card.cardName;
        if(replaceIcon)
            icon.sprite = card.icon;
        SetRarity(card.rarity);
    }

    private void SetRarity(CardRarity rarity)
    {
        for (var i = 0; i < rarityFxObjects.Count; i++)
        {
            rarityFxObjects[i].SetActive(i == (int)rarity);
        }
    }
}
