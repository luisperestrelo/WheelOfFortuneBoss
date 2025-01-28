using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI sizeText;

    private void Start()
    {
        iconImage = transform.Find("Icon").GetComponent<Image>();
        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        rarityText = transform.Find("Rarity").GetComponent<TextMeshProUGUI>();
        typeText = transform.Find("Type").GetComponent<TextMeshProUGUI>();  
    }

    public void DisplayCard(Card card)
    {
        iconImage.sprite = card.icon;
        nameText.text = card.cardName;
        
        if (card.isKissCurse)
        {
            descriptionText.text = card.description + " <color=red>" + card.downSideDescription + "</color>";
        }
        else
        {
            descriptionText.text = card.description;
        }

        typeText.text = card.cardType.ToString();

        rarityText.text = card.rarity.ToString();
        rarityText.color = StatsHelper.GetRarityColor(card.rarity);
    }
}