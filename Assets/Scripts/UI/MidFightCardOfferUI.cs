using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MidFightCardOfferUI : MonoBehaviour
{
    public CardDisplay[] cardDisplays;
    public UpgradeDisplay[] upgradeDisplays;
    public Button confirmButton;
    private List<Card> offeredCards;
    private Card selectedCard;
    private int selectedIndex = -1;
    
    

    public Color highlightColor = new Color(1f, 1f, 1f, 0.5f);
    public UIWheel uiWheel;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowUpgrades(List<Card> cards)
    {
        animator.SetBool("isOpen", true);

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
                upgradeDisplays[i].gameObject.SetActive(false); // Hide unused (eg if we run out of cards in the pool i guess)
            }
        }

        // Enable confirm button if no cards are displayed or a card is selected
        confirmButton.interactable = !anyCardsActive || selectedCard != null;

        gameObject.SetActive(true);
    }

    
    public void OnUpgradeClicked(int cardIndex)
    {
        // Reset the previously selected card's border (if any)
        if (selectedIndex != -1)
        {
            // upgradeDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
            upgradeDisplays[selectedIndex].Deselect();
        }

        selectedIndex = cardIndex;
        selectedCard = offeredCards[cardIndex];

        // Highlight the selected card
        // cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = highlightColor;
        upgradeDisplays[selectedIndex].Select();

        // Enable the confirm button
        confirmButton.interactable = true;
    }
    
    public void ShowCards(List<Card> cards)
    {
        animator.SetBool("isOpen", true);

        offeredCards = cards;
        selectedCard = null;

        bool anyCardsActive = false; // Flag to check if any cards are displayed

        for (int i = 0; i < cardDisplays.Length; i++)
        {
            if (i < cards.Count)
            {
                cardDisplays[i].DisplayCard(cards[i]);
                cardDisplays[i].gameObject.SetActive(true);
                anyCardsActive = true; // Set flag to true if at least one card is active
            }
            else
            {
                cardDisplays[i].gameObject.SetActive(false); // Hide unused (eg if we run out of cards in the pool i guess)
            }
        }

        // Enable confirm button if no cards are displayed or a card is selected
        confirmButton.interactable = !anyCardsActive || selectedCard != null;

        gameObject.SetActive(true);
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
        Debug.Log("OnConfirmButtonClicked");
        if (selectedCard != null)
        {
            if (selectedCard.cardType == CardType.Field)
            {
                ConfirmFieldCard();
            }
            else
            {
                ConfirmPassiveCard();
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

    private void ConfirmPassiveCard()
    {
        RunManager.Instance.OnMidFightStatCardSelected(selectedCard);
        Debug.Log("Selected card is not a Field card");

        // StartCoroutine(DeactivateAfterDelay());
        if (selectedIndex != -1)
        {
            // cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
            upgradeDisplays[selectedIndex].Deselect();
            selectedIndex = -1;
        }
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
        if (selectedIndex != -1)
        {
            // cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
            upgradeDisplays[selectedIndex].Deselect();
            selectedIndex = -1;
        }
        
        // StartCoroutine(DeactivateAfterDelay());
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DeactivateAfterDelay()
    {
        animator.SetBool("isOpen", false);
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