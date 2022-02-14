using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    //The information of the card
    public CardData cardData;
    private bool isRevealed = false;
    private Collider cardCollider;

    private void Start()
    {
        //Get the collider of the card
        cardCollider= GetComponent<Collider>();
    }

    /// <summary>
    /// Reveal the card
    /// </summary>
    public void RevealCard()
    {
        isRevealed = true;
        cardCollider.enabled = true;
    }

    /// <summary>
    /// Swap the card between the states of being revealed and being hidden
    /// </summary>
    public void FlipCard()
    {
        isRevealed = !isRevealed;
        cardCollider.enabled = !cardCollider.enabled;
    }

    /// <summary>
    /// Returns whether the card is revealed
    /// </summary>
    public bool IsRevealed()
    {
        return isRevealed;
    }
}
