using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldCardDisplay : UpgradeDisplayVisual
{
    [SerializeField] private List<RotateImage> selectObjects;
    // Start is called before the first frame update


    public void SetValues(FieldCard card)
    {
        nameText.text = card.cardName;
        icon.sprite = card.icon;
        icon.color = card.field.Color;
        SetRarity(card.rarity);
    }

    public override void Select()
    {
  
        selectObjects.ForEach(x => x.Play());   
 
    }
    
    public override void Deselect()
    {

        selectObjects.ForEach(x => x.Stop());   

    }
}