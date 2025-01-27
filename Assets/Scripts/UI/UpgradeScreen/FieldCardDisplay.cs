using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCardDisplay : UpgradeDisplayVisual
{
    [SerializeField] protected List<RotateImage> rarityFxObjects;
    // Start is called before the first frame update
    protected override void SetRarity(CardRarity rarity)
    {
        for (var i = 0; i < rarityFxObjects.Count; i++)
        {
            rarityFxObjects[i].gameObject.SetActive(i == (int)rarity);
        }
    }

    public override void Select()
    {
  
            rarityFxObjects.ForEach(x => x.Play());   
 
    }
    
    public override void Deselect()
    {

            rarityFxObjects.ForEach(x => x.Stop());   

    }
}
