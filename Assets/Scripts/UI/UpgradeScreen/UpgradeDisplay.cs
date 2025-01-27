using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDisplay : MonoBehaviour
{
    
    [SerializeField] private Image selectionImage;
    [SerializeField] private UpgradeDisplayVisual activeUpgrade;
    [SerializeField] private UpgradeDisplayVisual passiveUpgrade;
    // Start is called before the first frame update
    void Start()
    {
        passiveUpgrade.gameObject.SetActive(false);
        activeUpgrade.gameObject.SetActive(false);
    }

    public void Display(Card card)
    {
        if (card.cardType == CardType.Field)
            DisplayActive(card);
        else
            DisplayPassive(card);
    }

    public void Select()
    {
        selectionImage.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        selectionImage.gameObject.SetActive(false);
    }


    private void DisplayPassive(Card card)
    {
        passiveUpgrade.gameObject.SetActive(true);
        activeUpgrade.gameObject.SetActive(false);
        passiveUpgrade.SetValues(card, false);

    }

    private void DisplayActive(Card card)
    {
        activeUpgrade.gameObject.SetActive(true);
        passiveUpgrade.gameObject.SetActive(false);
        activeUpgrade.SetValues(card);
    }
}
