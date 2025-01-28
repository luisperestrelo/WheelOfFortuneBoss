using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UpgradeDisplayVisual : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image icon;
    [SerializeField] private List<GameObject> rarityFxObjects;


    public void SetValues(Card card, bool replaceIcon = true)
    {
        nameText.text = card.cardName;
        if(replaceIcon)
            icon.sprite = card.icon;
        SetRarity(card.rarity);
    }

    protected virtual void SetRarity(CardRarity rarity)
    {
        for (var i = 0; i < rarityFxObjects.Count; i++)
        {
            rarityFxObjects[i].gameObject.SetActive(i == (int)rarity);
        }
    }

    public abstract void Select();

    public abstract void Deselect();

}
