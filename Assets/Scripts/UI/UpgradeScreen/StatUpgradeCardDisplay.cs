using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgradeCardDisplay : UpgradeDisplayVisual
{
    [SerializeField] private GameObject selectContainer;
    [SerializeField] private List<RotateImage> selectObjects;
    
    public void SetValues(Card card)
    {
        nameText.text = card.cardName;
        icon.sprite = card.icon;
        SetRarity(card.rarity);
    }

    public override void Select()
    {
        selectContainer.gameObject.SetActive(true);
        selectObjects.ForEach(x => x.Play());
    }
    
    public override void Deselect()
    {
        selectContainer.gameObject.SetActive(false);
        selectObjects.ForEach(x => x.Stop());

    }
}
