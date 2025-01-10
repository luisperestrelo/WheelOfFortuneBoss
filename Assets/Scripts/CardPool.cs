using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardPool : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    [Header("Rarity Settings")]
    public int commonCopies = 4;
    public int rareCopies = 2;
    public int epicCopies = 1;

    public void Initialize()
    {
        allCards.Clear(); // Clear the list before populating

        // Load all Card ScriptableObjects from the "Cards" folder
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
    }

    public List<Card> GetRandomCards(int numCards)
    {
        // Shuffle the allCards list (you can use a Fisher-Yates shuffle for better randomness)
        List<Card> shuffledCards = allCards.OrderBy(x => Random.value).ToList();

        // Return the first numCards from the shuffled list
        return shuffledCards.Take(numCards).ToList();
    }

    public void AddCardToPool(Card card)
    {
        // Add the card to the pool based on its rarity
        switch (card.rarity)
        {
            case CardRarity.Common:
                for (int i = 0; i < commonCopies; i++)
                {
                    allCards.Add(card);
                }
                break;
            case CardRarity.Rare:
                for (int i = 0; i < rareCopies; i++)
                {
                    allCards.Add(card);
                }
                break;
            case CardRarity.Epic:
                for (int i = 0; i < epicCopies; i++)
                {
                    allCards.Add(card);
                }
                break;
        }
    }

    public void RemoveCardFromPool(Card card)
    {
        // Remove all instances of the card from the pool
        allCards.RemoveAll(c => c == card);
    }

    public List<Card> GetInitialCards()
    {
        List<Card> initialCards = new List<Card>();

        // Ensure at least 3 FieldCards
        List<FieldCard> fieldCards = allCards.OfType<FieldCard>().OrderBy(x => Random.value).Take(3).ToList();
        initialCards.AddRange(fieldCards);

        // Add 2 other random cards
        List<Card> otherCards = allCards.Except(fieldCards).OrderBy(x => Random.value).Take(2).ToList();
        
        initialCards.AddRange(otherCards);

        return initialCards;
    }
} 