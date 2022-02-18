using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Holds the data of the original deck
/// </summary>
public class DeckHolder : MonoBehaviour
{
    [Tooltip("The deck to load")]
    [SerializeField] private DeckOfCards setDeck;
    [SerializeField] private float deckDisplayCardSet=0.01f,topCardYValue=0;

    [SerializeField] private List<CardData> deck = new List<CardData>();

    private void Start()
    {
        LoadDeck();
        LoadDeckDisplay();
    }

    private void LoadDeck()
    {
        deck.AddRange(setDeck.deckData.ToList());
        ExtensionMethods.Shuffle(deck);
    }

    /// <summary>
    /// Loads the cards to show the deck size, does not represent actual cards but rather just their set back graphic
    /// </summary>
    private void LoadDeckDisplay()
    {
        //Remove all previous children
        foreach(Transform child in gameObject.transform) {
            Destroy(child.gameObject);
        }
        topCardYValue = 0;

        Vector3 pos = transform.position;
        for (int d = 0; d < deck.Count; d++)
        {
            Instantiate(setDeck.deckBackDisplay, new Vector3(pos.x,pos.y+topCardYValue, pos.z),Quaternion.identity,transform);
            topCardYValue += deckDisplayCardSet;
        }
    }
    
    /// <summary>
    /// Takes the top card from the deck
    /// </summary>
    public CardData DrawCard()
    {
        if (deck.Count > 0)
        {
            //Get the top card in the list (queue was considered but due to us needing to shuffle the list, a queue is not suitable)
            int index = deck.Count - 1;
            CardData cardData = deck[index];
            //Remove the top card in the list
            deck.RemoveAt(index);
            //reload cards displayed in the list
            LoadDeckDisplay();
            //Return the card
            return cardData;
        }
        else
            //If no cards in deck, dont return any drawn cards
            return null;
    }
    
    /// <summary>
    /// Adds card to deck and shuffles
    /// </summary>
    public void AddCard(CardData card)
    {
        deck.Add(card);
        ExtensionMethods.Shuffle(deck);
        LoadDeckDisplay();
    }

    /// <summary>
    /// Gets the rotation value that should be used for the cards
    /// </summary>
    public Vector3 GetCardRotationValue()
    {
        return setDeck.cardDefaultRotation;
    }
}
