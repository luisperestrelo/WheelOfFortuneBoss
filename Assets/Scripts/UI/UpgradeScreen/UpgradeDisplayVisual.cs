using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UpgradeDisplayVisual : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected Image icon;
    [SerializeField] private List<GameObject> rarityFxObjects;
    

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
