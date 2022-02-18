using System.Collections;
using UnityEngine;
public class SolitaireGamemode : BaseGamemode
{
    [SerializeField] private CardPlacementLocations gameboard, cardFinishedSections, deckPositions;
    [SerializeField] private float yOffset = 0.0002f,zOffset=0.02f,cardMoveSpeed=0.5f;

    private int cardLayer=6;
    private void Start()
    {
        //For solitair the main section requires 7 points
        if (gameboard.cardLocations.Count != 7)
            Debug.LogError("There are not enough gameboard locations, requires 7 total to play game");
        //Requires 4 points
        if (cardFinishedSections.cardLocations.Count != 4)
            Debug.LogError("There are not enough finished locations, requires 4 total to play game");
        //requires 1, this is where we will be drawing cards for player to add to the solitair board
        if (deckPositions.cardLocations.Count != 1)
            Debug.LogError("There are not enough deckPosition locations, requires 1 total to play game");
        
        StartCoroutine(StartGame());
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    private IEnumerator StartGame()
    {
        float currentYOffset = 0;
        float currentZOffset = 0;
        //We use this value to limit cards placed as solitair does not have equal fields
        int cardVal=-1;

        //Wait before starting so that we can load the deck
        yield return new WaitForSeconds(0.01f);
        //Start creation cards
        //Every new row
        for (int d = 0; d < 7; d++)
        {
            //Every card in a column
            for (int i = 0; i < 7; i++)
            {
                //Draw a card
                CardData card = gameDeck.DrawCard();

                //If we drew a card
                if (card!=null)
                {
                    //This makes it so that cards will be in a stacking order and each following column will have more cards in it
                    if (i > cardVal)
                    {
                        //Setup game object
                        Vector3 pos = gameboard.cardLocations[i].position;
                        pos = new Vector3(pos.x, pos.y + currentYOffset, pos.z + currentZOffset);
                        GameObject createdCard = Instantiate(card.cardModel, gameDeck.transform.position, Quaternion.Euler(gameDeck.GetCardRotationValue()));
                        createdCard.layer = cardLayer;

                        //Add it as a child of the transform of its gameboard
                        createdCard.transform.parent = gameboard.cardLocations[i];

                        //Initialise card holder
                        CardObject cardHolder = createdCard.AddComponent<CardObject>();
                        cardHolder.cardData = card;

                        //Setup the MoveGameObjectToPosition script
                        MoveGameObjectToPosition createdCardMoveToObject = createdCard.AddComponent<MoveGameObjectToPosition>();
                        createdCardMoveToObject.SetSpeed(cardMoveSpeed);
                        createdCardMoveToObject.SetTargetDestination(pos);

                        //add a box collider
                        BoxCollider cardCollider = createdCard.AddComponent<BoxCollider>();
                        //we disable card colliders so that they cant be moved
                        cardCollider.enabled = false;

                        currentYOffset += yOffset;
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }
            cardVal++;
            currentZOffset -= zOffset;
        }

        //Add card holders to each position, this will then add all the cards to their respective lists
        foreach (Transform transform in gameboard.cardLocations)
        {
            SolitaireCardHolder solitaireCardHolder = transform.gameObject.AddComponent<SolitaireCardHolder>();
            solitaireCardHolder.yOffset = yOffset;
            solitaireCardHolder.zOffset = zOffset;
        }
    }
}
