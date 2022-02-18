using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for card holders, function changes for different game modes and different parts of a game mode
/// </summary>
public class CardHolder : MonoBehaviour
{
    //Cards that would be held here
    [SerializeField]protected List<CardObject> cardObjects=new List<CardObject>();

    private void Start()
    {
        InitialiseCardHolder();
    }

    protected virtual void InitialiseCardHolder()
    {
    }

    public virtual void AddCardToHolder(CardObject card)
    {
        if (card!=null)
            cardObjects.Add(card);
    }

    public virtual void RemoveCardFromHolder(CardObject card)
    {
        if (cardObjects.Contains(card))
            cardObjects.Remove(card);
        else
            Debug.LogWarning("Card to be removed from the holder was not found.");
    }
}
