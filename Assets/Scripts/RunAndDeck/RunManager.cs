using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RunManager : MonoBehaviour
{
    public CardPool cardPool;
    public CardManager cardManager;
    public List<Card> currentRunCards = new List<Card>();
    public WheelManager wheelManager; // Assign in Inspector
    public string bossFightSceneName = "BossFightScene"; // Set the name of your boss fight scene
    public CardDisplay[] cardDisplays;
    public Player player;

    public static RunManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartRun()
    {
        // Reset player stats, clear current run cards, etc.
        currentRunCards.Clear();
        cardPool.Initialize(); // Or re-initialize if you want a fresh pool each run

        // Initial card selection
        OfferInitialCards();
    }

    void Start()
    {
        StartRun();
        DisableWheelAndPlayer();
    }

    void Update() //testing
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OfferCardSelection();
        }
    }

    public void OfferInitialCards()
    {
        List<Card> initialCards = cardPool.GetInitialCards(); // Using the custom rule method
        // Display these cards in the UI
        for (int i = 0; i < initialCards.Count; i++)
        {
            cardDisplays[i].DisplayCard(initialCards[i]);
        }

        // Apply all cards immediately
        foreach (Card card in initialCards)
        {
            cardManager.ApplyCard(card);
            currentRunCards.Add(card);
        }

        // Close the card selection UI (if it was used)
        Debug.Log("Applied initial cards (UI not implemented yet)");

        foreach (Card card in currentRunCards)
        {
            Debug.Log("Card in current run: " + card.cardName);
        }
    }

    public void OfferCardSelection()
    {
        // Trigger the card selection UI (not implemented yet)
        List<Card> cardsToOffer = cardPool.GetRandomCards(5);

        // For now, just log the offered cards to the console
        Debug.Log("Offered Cards:");
        foreach (Card card in cardsToOffer)
        {
            Debug.Log($"- {card.cardName} ({card.rarity})");
        }

        // In the actual implementation, you would display the cards in the UI
        // and call OnCardSelected when a card is clicked
    }

    // This will be called when the player selects a card in the UI (UI not implemented yet)
    public void OnCardSelected(Card selectedCard)
    {
        cardManager.ApplyCard(selectedCard);
        currentRunCards.Add(selectedCard);
        // Close the card selection UI
        Debug.Log($"Selected Card: {selectedCard.cardName}");
    }

    public void StartFight()
    {

        EnableWheelAndPlayer();
        SceneManager.LoadScene(bossFightSceneName);

    }


    public void EndFight()
    {
        SceneManager.LoadScene("PostBossCardSelection");
    }

    public void DisableWheelAndPlayer()
    {
        // Disable Wheel components
        wheelManager.gameObject.SetActive(false); // Or disable specific components

        // Disable Player components
        player.gameObject.SetActive(false); // Or disable specific components
    }

    public void EnableWheelAndPlayer()
    {
        wheelManager.gameObject.SetActive(true); // Or disable specific components
        player.gameObject.SetActive(true); // Or disable specific components
    }

}