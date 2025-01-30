using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MidFightCardOfferUI : MonoBehaviour
{
    public CardDisplay[] cardDisplays;
    public UpgradeDisplay[] upgradeDisplays;
    public Button confirmButton;
    public StatsDisplay statsDisplay;

    [SerializeField] private AudioClip openSfx;
    [SerializeField] private AudioClip selectSfx;
    [SerializeField] private AudioClip openWheelPlacementSfx;
    [SerializeField] private AudioClip closeSfx;
    
    private List<Card> offeredCards;
    private Card selectedCard;
    private int selectedIndex = -1;

    private AudioSource source;

    public Color highlightColor = new Color(1f, 1f, 1f, 0.5f);
    public UIWheel uiWheel;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // ShowCards
    public void ShowUpgrades(List<Card> cards)
    {
        animator.SetBool("isOpen", true);
        source.PlayOneShot(openSfx);
        offeredCards = cards;
        selectedCard = null;

        bool anyCardsActive = false; // Flag to check if any cards are displayed

        for (int i = 0; i < upgradeDisplays.Length; i++)
        {
            if (i < cards.Count)
            {
                upgradeDisplays[i].Display(cards[i]);
                upgradeDisplays[i].gameObject.SetActive(true);
                anyCardsActive = true; // Set flag to true if at least one card is active
            }
            else
            {
                upgradeDisplays[i].gameObject
                    .SetActive(false); // Hide unused (eg if we run out of cards in the pool i guess)
            }
        }

        // Enable confirm button if no cards are displayed or a card is selected
        confirmButton.interactable = !anyCardsActive || selectedCard != null;

        statsDisplay.UpdateStats();

        gameObject.SetActive(true);
    }


    // On Card clicked
    public void OnUpgradeClicked(int cardIndex)
    {
        SelectCard(cardIndex);
        source.PlayOneShot(selectSfx);
        
        // Enable the confirm button
        confirmButton.interactable = true;
    }


    public void OnConfirmButtonClicked()
    {
        if (selectedCard != null)
        {
            if (selectedCard.cardType == CardType.Field)
            {
                ConfirmFieldCard();
            }
            else
            {
                ConfirmStatUpgradeCard();
            }
        }
        else if (offeredCards.Count == 0)
        {
            // Handle the case where no cards are offered (empty list)
            Debug.Log("No cards to select, continuing the game.");
            Time.timeScale = 1f; // ok I hate having this here but like we should never get to this point anyway
            StartCoroutine(DeactivateAfterDelay());
        }
    }

    private void ConfirmStatUpgradeCard()
    {
        RunManager.Instance.OnMidFightStatCardSelected(selectedCard);
        Debug.Log("Selected card is not a Field card"); 
        if (selectedCard is StatUpgradeCard statUpgradeCard)
        {
            statsDisplay.AddStatsToListToShow(statUpgradeCard.statTypes);
        }

        // StartCoroutine(DeactivateAfterDelay());
        
        SelectCard(-1);


        Close();
    }

    private void ConfirmFieldCard()
    {
        Debug.Log("Selected card is a Field card");
        Field newField = ((FieldCard)selectedCard).field;
        if (RunManager.Instance.wheelManager.Segments.Count >= uiWheel.MaxSegments)
        {
            uiWheel.Initialize(RunManager.Instance.wheelManager, newField, selectedCard, UIWheelMode.Replace);
        }
        else
        {
            uiWheel.Initialize(RunManager.Instance.wheelManager, newField, selectedCard, UIWheelMode.Insert);
        }
        SelectCard(-1);
        source.PlayOneShot(openWheelPlacementSfx);

        // StartCoroutine(DeactivateAfterDelay());
    }

    private void SelectCard(int index)
    {
        // deselect previous selected card
        if (selectedIndex >= 0)
        { 
            upgradeDisplays[selectedIndex].Deselect();
            selectedCard = null;
            statsDisplay.RemoveTemporaryStats();
        }
        
        if (index >= 0)
        {
            upgradeDisplays[index].Select();
            selectedCard = index < offeredCards.Count ? offeredCards[index] : null;
            if (selectedCard is StatUpgradeCard statUpgradeCard)
            {
                statsDisplay.AddTemporaryStats(statUpgradeCard.statTypes, statUpgradeCard.statValues);
            }
        }
        selectedIndex = index;
    }


    
    public void Close()
    {
        SFXPool.instance.PlaySound(closeSfx, SFXPool.MixGroup.ui);
        gameObject.SetActive(false);
    }

    private IEnumerator DeactivateAfterDelay()
    {
        animator.SetBool("isOpen", false);
        SFXPool.instance.PlaySound(closeSfx, SFXPool.MixGroup.ui);
        const float delay = 0.8f;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);

        uiWheel.gameObject.SetActive(false);
    }

    private void Start()
    {
        // confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        // Hide the UI initially
        gameObject.SetActive(false);
    }
}