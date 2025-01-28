using TMPro;
using UnityEngine;

public class CardTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI badText;
    [SerializeField] private RectTransform rect;

    private float minHeight;

    public RectTransform Rect { get => rect; private set => rect = value; }

    private const float DescriptionTextOffset = 20;

    public void SetContent(Card card)
    {
        nameText.text = card.cardName;
        rarityText.text = card.rarity.ToString();
        typeText.text = card.cardType == CardType.Field ? "Field" : "Stat Upgrade";
        rarityText.color = StatsHelper.GetRarityColor(card.rarity);
        descriptionText.text = card.description;
        badText.text = card.downSideDescription;
        
        var descriptionTextBottom = descriptionText.rectTransform.anchoredPosition.y - descriptionText.rectTransform.sizeDelta.y * 0.5f;
        badText.rectTransform.anchoredPosition = new Vector2(badText.rectTransform.anchoredPosition.x,
            descriptionTextBottom - badText.rectTransform.sizeDelta.y - DescriptionTextOffset);
    
        // Todo: calc correct size
        Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, minHeight + descriptionText.preferredHeight + badText.preferredHeight + DescriptionTextOffset);
    }
    
    private void Start()
    {
        minHeight = rect.sizeDelta.y;
    }
}
