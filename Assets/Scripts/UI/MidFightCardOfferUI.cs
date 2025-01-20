using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MidFightCardOfferUI : MonoBehaviour
{
    public CardDisplay[] cardDisplays;
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

    public void ShowCards(List<Card> cards)
    {
        animator.SetBool("isOpen", true);

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
        if (selectedCard != null)
        {
            if (selectedCard.cardType == CardType.Field)
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
                    cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
                    selectedIndex = -1;
                }
                StartCoroutine(DeactivateAfterDelay());
            }
            else
            {
                RunManager.Instance.OnMidFightStatCardSelected(selectedCard);
                Debug.Log("Selected card is not a Field card");

                StartCoroutine(DeactivateAfterDelay());

                if (selectedIndex != -1)
                {
                    cardDisplays[selectedIndex].transform.Find("BorderImage").GetComponent<Image>().color = Color.clear;
                    selectedIndex = -1;
                }
            }
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        animator.SetBool("isOpen", false);
        const float delay = 0.8f;
        yield return new WaitForSeconds(delay);
        //gameObject.SetActive(false);
        uiWheel.gameObject.SetActive(false);
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        // Hide the UI initially
        gameObject.SetActive(false);
    }
}