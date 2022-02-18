using UnityEngine;

public class CardObject : MonoBehaviour
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
        FlipCardObject();
        isRevealed = true;
        cardCollider.enabled = true;
    }

    /// <summary>
    /// Hide the card
    /// </summary>
    public void HideCard()
    {
        FlipCardObject();
        isRevealed = false;
        cardCollider.enabled = false;
    }

    /// <summary>
    /// Swap the card between the states of being revealed and being hidden
    /// </summary>
    public void FlipCard()
    {
        FlipCardObject();
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

    /// <summary>
    /// Flips the card to change its side
    /// </summary>
    private void FlipCardObject()
    {
        //Flip the card
        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x + 180, rot.y, rot.z);
        transform.rotation = Quaternion.Euler(rot);
    }
}
