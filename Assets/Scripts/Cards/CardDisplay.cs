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
        descriptionText.text = card.description;
        typeText.text = "Card Type: " + card.cardType.ToString();

        rarityText.text = card.rarity.ToString();
        if (card.rarity == CardRarity.Common)
        {
            rarityText.color = Color.white;
        }
        else if (card.rarity == CardRarity.Rare)
        {
            rarityText.color = Color.blue;
        }
        else if (card.rarity == CardRarity.Epic)
        {
            rarityText.color = Color.magenta;
        }

    }
}