using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PostBossCardSelectionUI : MonoBehaviour
{
    public CardDisplay[] cardDisplays; 
    public Button startFightButton;
    private List<Card> offeredCards;
    private List<int> selectedCards = new List<int>();
    public Color highlightColor = new Color(1f, 1f, 1f, 0.5f);

    private void Start()
    {
        StartCoroutine(InitializeAfterDelay());
    }

    private IEnumerator InitializeAfterDelay()
    {
        yield return new WaitForSeconds(0.01f); 

        // Ensure RunManager is initialized after the wait
        if (RunManager.Instance == null)
        {
            Debug.LogError("RunManager instance is not initialized after waiting.");
            yield break; // Stop the coroutine if RunManager is still null
        }

        startFightButton.onClick.AddListener(OnStartFightButtonClicked);
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
        // Check if the clicked card's index is already in the selectedCards list
        if (selectedCards.Contains(cardIndex))
        {
            // Deselect the card by removing its index
            selectedCards.Remove(cardIndex);
            cardDisplays[cardIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
        }
        else if (selectedCards.Count < 3)
        {
            // Select the card by adding its index
            selectedCards.Add(cardIndex);
            cardDisplays[cardIndex].transform.Find("BorderImage").GetComponent<Image>().color = highlightColor;
        }

        // Enable "Start Fight" button only if 3 cards are selected
        startFightButton.interactable = selectedCards.Count == 3;

        Debug.Log("Selected cards: " + selectedCards.Count);
    }

    public void OnStartFightButtonClicked()
    {
        if (selectedCards.Count == 3)
        {
            RunManager.Instance.EnableWheelAndPlayer(); // gotta re-enable it here so we can apply the cards

            // Convert selected indices to a list of Card objects
            List<Card> cardsToAdd = new List<Card>();
            foreach (int index in selectedCards)
            {
                cardsToAdd.Add(offeredCards[index]);
            }

            // Add the selected cards to the player's current run cards
            RunManager.Instance.AddCardsToCurrentRun(cardsToAdd);

            foreach (CardDisplay cardDisplay in cardDisplays)
            {
                cardDisplay.transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
            }

            RunManager.Instance.StartFight();
        }
    }
}
