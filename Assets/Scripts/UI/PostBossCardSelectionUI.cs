using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PostBossCardSelectionUI : MonoBehaviour
{
    public CardDisplay[] cardDisplays; 
    public Button startFightButton;
    private List<Card> offeredCards;
    private List<Card> selectedCards = new List<Card>();
    public Color highlightColor = new Color(1f, 1f, 1f, 0.5f);

    private void Start()
    {
        
        startFightButton.onClick.AddListener(OnStartFightButtonClicked);

        // Button becomes interactable when player selects 3 cards
        startFightButton.interactable = false;

        offeredCards = RunManager.Instance.GetOfferedCards();

        ShowCards(offeredCards);
    }

    public void ShowCards(List<Card> cards)
    {
        for (int i = 0; i < cardDisplays.Length; i++)
        {
            if (i < cards.Count)
            {
                cardDisplays[i].DisplayCard(cards[i]);
                cardDisplays[i].gameObject.SetActive(true);

               
            }
            else
            {
                cardDisplays[i].gameObject.SetActive(false);
            }
        }
    }

//TODO: Buggy
    public void OnCardClicked(int cardIndex)
    {
        Card clickedCard = offeredCards[cardIndex];

        if (selectedCards.Contains(clickedCard))
        {
            // Deselect the card
            selectedCards.Remove(clickedCard);
            cardDisplays[cardIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
        }
        else if (selectedCards.Count < 3)
        {
            // Select the card
            selectedCards.Add(clickedCard);
            cardDisplays[cardIndex].transform.Find("BorderImage").GetComponent<Image>().color = highlightColor;
        }

        // Enable "Start Fight" button only if 3 cards are selected
        startFightButton.interactable = selectedCards.Count == 3;
    }

    public void OnStartFightButtonClicked()
    {
        if (selectedCards.Count == 3)
        {
            RunManager.Instance.EnableWheelAndPlayer(); // gotta re-enable it here so we can apply the cards
            // Add the selected cards to the player's current run cards
            RunManager.Instance.AddCardsToCurrentRun(selectedCards);

            foreach (CardDisplay cardDisplay in cardDisplays)
            {
                cardDisplay.transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
            }

            RunManager.Instance.StartFight();
        }
    }
}
