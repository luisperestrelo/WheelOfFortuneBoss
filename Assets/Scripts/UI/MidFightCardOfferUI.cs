using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MidFightCardOfferUI : MonoBehaviour
{
    public CardDisplay[] cardDisplays;
    public Button confirmButton;
    private List<Card> offeredCards;
    private Card selectedCard;
    private int selectedIndex = -1;

    public Color highlightColor = new Color(1f, 1f, 1f, 0.5f); // Set in inspector
    public void ShowCards(List<Card> cards)
    {
        

        offeredCards = cards;
        selectedCard = null;

        for (int i = 0; i < cardDisplays.Length; i++)
        {
            if (i < cards.Count)
            {
                cardDisplays[i].DisplayCard(cards[i]);
                cardDisplays[i].gameObject.SetActive(true);


            }
            else
            {
                cardDisplays[i].gameObject.SetActive(false); // Hide unused (eg if we run out of cards in the pool i guess)
            }
        }

        confirmButton.interactable = false; // player can only confirm when he selects a card


        gameObject.SetActive(true); // sets itself active 
    }

    public void OnCardClicked(int cardIndex)
    {
        // Reset the previously selected card's border (if any)
        if (selectedIndex != -1)
        {
            cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
        }

        selectedIndex = cardIndex;
        selectedCard = offeredCards[cardIndex];

        // Highlight the selected card
        cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = highlightColor;

        // Enable the confirm button
        confirmButton.interactable = true;
    }

    public void OnConfirmButtonClicked()
    {
        if (selectedCard != null)
        {
            RunManager.Instance.OnMidFightCardSelected(selectedCard);

            // Hide the UI
            gameObject.SetActive(false);

            // Reset the selected card's border
            if (selectedIndex != -1)
            {
                cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
                selectedIndex = -1;
            }
        }
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        // Hide the UI initially
        gameObject.SetActive(false);
    }
}