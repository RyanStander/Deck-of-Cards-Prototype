using UnityEngine;

public class SolitaireCardHolder : CardHolder
{
    public float yOffset= 0.0002f, zOffset=0.01f;
    private float currentYOffset=0, currentZOffset=0;

    /// <summary>
    /// Adds a card to the bottom of the list, the bottom card will be revealed
    /// </summary> 
    public override void AddCardToHolder(CardObject card)
    {
        base.AddCardToHolder(card);

        if (card.TryGetComponent(out MoveGameObjectToPosition moveGameObject))
        {
            //Create the position we want to set the object to
            Vector3 pos = transform.position;
            pos = new Vector3(pos.x, pos.y + currentYOffset, pos.z + currentZOffset);

            //update the current offsets
            currentYOffset += yOffset;
            currentZOffset -= zOffset;

            //Set the target destination of the object
            moveGameObject.SetTargetDestination(pos);

            //Get the current count of cards
            int cardCount = cardObjects.Count;

            //if there is only 1 card, we dont have anything to hide
            if (cardCount > 1)
                cardObjects[cardCount - 2].HideCard();

            //Reveal the top card
            cardObjects[cardCount - 1].RevealCard();

        }
    }

    public override void RemoveCardFromHolder(CardObject card)
    {
        base.RemoveCardFromHolder(card);
    }

    /// <summary>
    /// Finds all cards that are children and adds them to the card holder
    /// </summary>
    protected override void InitialiseCardHolder()
    {
        base.InitialiseCardHolder();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out CardObject cardObject))
            {
                AddCardToHolder(cardObject);
            }
        }
    }
}
