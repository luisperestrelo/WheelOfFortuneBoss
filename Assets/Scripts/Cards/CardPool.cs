using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardPool : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public List<Card> availableCards = new List<Card>();

    [Header("Rarity Settings")]
    public int commonCopies = 4;
    public int rareCopies = 2;
    public int epicCopies = 1;

    [Header("Initial Card Selection")]
    public List<Card> attackCards = new List<Card>();
    public List<Card> chargeCards = new List<Card>();
    public List<Card> statUpgradeCards = new List<Card>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        allCards.Clear();


        List<Card> loadedCards = Resources.LoadAll<Card>("Cards").ToList();

        foreach (Card card in loadedCards)
        {
            int copiesToAdd = 0;
            switch (card.rarity)
            {
                case CardRarity.Common:
                    copiesToAdd = commonCopies;
                    break;
                case CardRarity.Rare:
                    copiesToAdd = rareCopies;
                    break;
                case CardRarity.Epic:
                    copiesToAdd = epicCopies;
                    break;
            }

            for (int i = 0; i < copiesToAdd; i++)
            {
                allCards.Add(card); // Add the card multiple times based on rarity
            }
        }

        availableCards = new List<Card>(allCards);
    }

    public List<Card> GetRandomCards(int numCards)
    {
        // Shuffle the availableCards list (todo: maybe a fisher yates shuffle for shits and giggles)
        List<Card> shuffledCards = availableCards.OrderBy(x => Random.value).ToList();

        // Take the first numCards from the shuffled list
        List<Card> selectedCards = shuffledCards.Take(numCards).ToList();

        // Remove the selected cards from the availableCards list
        // need to rework this a bit
        foreach (Card card in selectedCards)
        {
            availableCards.Remove(card);
        }

        return selectedCards;
    }

    public List<Card> GetRandomWithoutRemovingFromPool(int numCards)
    {
        // Shuffle the availableCards list (todo: maybe a fisher yates shuffle for shits and giggles)
        List<Card> shuffledCards = availableCards.OrderBy(x => Random.value).ToList();

        // Take the first numCards from the shuffled list
        List<Card> selectedCards = shuffledCards.Take(numCards).ToList();

        // todo: need to rework this a bit, not being used.

        return selectedCards;
    }

    //this would be for mid-run adding cards stuff if we do that. EG fireball upgrades specific to fireball get added after we unlock a fireball

    public void AddCardToPool(Card card)
    {
        switch (card.rarity)
        {
            case CardRarity.Common:
                for (int i = 0; i < commonCopies; i++)
                {
                    allCards.Add(card);
                    availableCards.Add(card);
                }
                break;
            case CardRarity.Rare:
                for (int i = 0; i < rareCopies; i++)
                {
                    allCards.Add(card);
                    availableCards.Add(card);
                }
                break;
            case CardRarity.Epic:
                for (int i = 0; i < epicCopies; i++)
                {
                    allCards.Add(card);
                    availableCards.Add(card);
                }
                break;
        }

    }
    // Remove all instances of the card from the pool
    public void RemoveCardFromPool(Card card)
    {
        allCards.RemoveAll(c => c == card);
        availableCards.RemoveAll(c => c == card);
    }

    //initial cards have custom rules.
    //current implementation: 1 attack card, 1 charge card, 1 stat upgrade card
    //we could remove the stat upgrade card if we want to, but I think it's nice to show the player that they can upgrade their stats
    //Not all cards are available for initial cards, they are cherry-picked and are "simple" cards
    public List<Card> GetInitialCards()
    {
        List<Card> initialCards = new List<Card>();

        if (attackCards.Count > 0)
        {
            Card attackCard = GetRandomCardFromList(attackCards, true);
            initialCards.Add(attackCard);
        }

        if (chargeCards.Count > 0)
        {
            Card chargeCard = GetRandomCardFromList(chargeCards, true);
            initialCards.Add(chargeCard);
        }

        if (statUpgradeCards.Count > 0)
        {
            Card statUpgradeCard = GetRandomCardFromList(statUpgradeCards, true);
            initialCards.Add(statUpgradeCard);
        }

        return initialCards;
    }

   
    private Card GetRandomCardFromList(List<Card> cardList, bool removeFromAvailable = false)
    {
        Card selectedCard = cardList.OrderBy(x => Random.value).First();

        if (removeFromAvailable)
        {
            availableCards.Remove(selectedCard);
        }

        return selectedCard;
    }
}