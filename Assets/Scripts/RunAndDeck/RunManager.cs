using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RunManager : MonoBehaviour
{
    public CardPool cardPool;
    public CardManager cardManager;
    public List<Card> currentRunCards = new List<Card>();
    public WheelManager wheelManager; 
    public string bossFightSceneName = "BossFightScene"; // right now we only have one but later we fix this
    public CardDisplay[] cardDisplays;
    public Player player;
    public MidFightCardOfferUI midFightCardOfferUI; 
    public string postBossSceneName = "PostBossCardSelectionScene"; 
    private List<Card> offeredCards;
    private bool first = true; // TODO: right now startrun is also used just to go next boss, this will be removed once fixed
    public UIWheel uiWheel; // Reference to the UIWheel

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
            //cardPool.Initialize(); 

        if (first) // TODO: right now startrun is also used just to go next boss, this will be removed once fixed
        {
            cardPool.Initialize(); 
            first = false;
        }

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
        List<Card> initialCards = cardPool.GetInitialCards(); // Initial cards have a special rule  
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

        
        Debug.Log("Applied initial cards");

        foreach (Card card in currentRunCards)
        {
            Debug.Log("Card in current run: " + card.cardName);
        }
    }

    public void OfferCardSelection()
    {
        
        List<Card> cardsToOffer = cardPool.GetRandomCards(5);

       
        Debug.Log("Offered Cards:");
        foreach (Card card in cardsToOffer)
        {
            Debug.Log($"- {card.cardName} ({card.rarity})");
        }


    }

    public void OnCardSelected(Card selectedCard)
    {
        cardManager.ApplyCard(selectedCard);
        currentRunCards.Add(selectedCard);
        Debug.Log($"Selected Card: {selectedCard.cardName}");
    }

    public void StartFight()
    {
        EnableWheelAndPlayer(); // Enable when the fight starts. It is messy , but its fine for now
        MusicPlayer.instance.StartSection(MusicPlayer.MusicSection.fight);
        SceneManager.LoadScene(bossFightSceneName);
    }

    public void EndFight()
    {
        DisableWheelAndPlayer(); // Disable when fight ends. Messy but works for now
        SceneManager.LoadScene(postBossSceneName);
        MusicPlayer.instance.StartSection(MusicPlayer.MusicSection.ambience); //This will instead load a new music profile once we have more than 1 boss.
    }

    public void DisableWheelAndPlayer()
    {
        wheelManager.gameObject.SetActive(false); 

        player.gameObject.SetActive(false); 
    }

    public void EnableWheelAndPlayer()
    {
        wheelManager.gameObject.SetActive(true); 
        player.gameObject.SetActive(true); 
    }

    public void OfferMidFightCards()
    {
        List<Card> cardsToOffer = cardPool.GetRandomCards(5);
        MusicPlayer.instance.SetFilterIntensity(0.6f);
        midFightCardOfferUI.ShowCards(cardsToOffer);
    }

    public void OnMidFightStatCardSelected(Card selectedCard)
    {
        cardManager.ApplyCard(selectedCard);
        currentRunCards.Add(selectedCard);
        MusicPlayer.instance.SetFilterIntensity(0f);

        Time.timeScale = 1f;
        midFightCardOfferUI.gameObject.SetActive(false);
    }

    public void OnMidFightFieldCardSelected(Card selectedCard)
    {
        //applying card is handled in the ui wheel script
        currentRunCards.Add(selectedCard);
        MusicPlayer.instance.SetFilterIntensity(0f);

        Time.timeScale = 1f;
    }

    public void AddCardsToCurrentRun(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            cardManager.ApplyCard(card);
            currentRunCards.Add(card);
        }
    }

    public List<Card> GetOfferedCards()
    {
        if (offeredCards == null)
        {
            OfferPostFightCards();
        }
        return offeredCards;
    }

    public void OfferPostFightCards()
    {
        offeredCards = cardPool.GetRandomCards(5);
    }
}