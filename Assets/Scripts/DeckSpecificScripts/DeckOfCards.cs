using UnityEngine;

[CreateAssetMenu(menuName ="Decks/Skins")]
public class DeckOfCards : ScriptableObject
{
    public GameObject deckBackDisplay;
    public Vector3 cardDefaultRotation;
    public CardData[] deckData = new CardData[52];

    //Perform check to automate card naming
    private void OnValidate()
    {
#if UNITY_EDITOR
        foreach (CardData cardData in deckData)
        {
            if (cardData.cardName == "" && cardData.cardModel != null)
            {
                //Assign card's name
                cardData.cardName = cardData.cardModel.name;

                cardData.cardSuit = ExtensionMethods.DetermineCardSuit(cardData.cardModel.name);

                cardData.cardNumber = ExtensionMethods.DetermineCardNumber(cardData.cardModel.name);
            }
        }

#endif
    }
}

[System.Serializable]
//Holds every card
public class CardData
{
    //The name of the card
    public string cardName;
    //The model used for the card
    public GameObject cardModel;
    //The suit of the card
    public CardSuit cardSuit;
    //The number of the card, a jack would be 11 for example
    public int cardNumber;
}


