using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CurrentCardsMenu : MonoBehaviour
{

    [SerializeField] private UpgradeDisplay[] cardDisplays;

    // (Optional) A close button to hide this menu.
    [SerializeField] private Button closeButton;

    /*
        [SerializeField] private Image selectionImage;
    [SerializeField] private FieldCardDisplay fieldCardDisplay;
    [SerializeField] private StatUpgradeCardDisplay statUpgradeDisplay;
    [SerializeField] private AudioSource menuSource;
    [SerializeField] private AudioClip hoverSfx;

    [SerializeField] private CardTooltip tooltip;*/



    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);
    }

    private void Start()
    {
        //PopulateCards();
        gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        PopulateCards();
    }


    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }


    public void PopulateCards()
    {


        List<Card> runCards = RunManager.Instance.currentRunCards;

        for (int i = 0; i < cardDisplays.Length; i++)
        {
            if (i < runCards.Count)
            {
                // Activate the display and set its card info.
                cardDisplays[i].gameObject.SetActive(true);
                cardDisplays[i].Display(runCards[i]);

                // Ensure that the display is non-interactive.
                Button button = cardDisplays[i].GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = false;
                }
            }
            else
            {
                // Deactivate any extra card display objects.
                cardDisplays[i].gameObject.SetActive(false);
            }
        }
    }
}