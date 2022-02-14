using UnityEngine;

/// <summary>
/// Moves the cards around based on mouse pointer
/// </summary>
public class CardMover : MonoBehaviour
{
    [SerializeField] private float cardYOffset;
    private float speed = 10f;
    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false;
    private const int MOUSE = 0;
    [SerializeField] private GameObject currentGameObject = null;
    private Camera mainCamera;
    private RaycastHit hit;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleCardMoving();
    }

    private void HandleCardMoving()
    {
        //If mouse button is being held down
        if (Input.GetMouseButtonDown(MOUSE))
        {
            //Find an object where the mouse pointer is
            FindObject();
            //enable our moving
            isMoving = true;
            //Check if we are interacting with a deck
            HandleDeckInteraction();
        }
        //If we release the mouse button
        else if (Input.GetMouseButtonUp(MOUSE))
        {
            isMoving = false;
            currentGameObject = null;
        }

        //if we are holding down the mouse butto
        if (isMoving)
        {
            if (currentGameObject != null)
            {
                SetTargetPosition();
                MoveObject();
            }
        }
    }


    /// <summary>
    /// Set the position we want to move our target to
    /// </summary>
    private void SetTargetPosition()
    {
        Plane plane = new Plane(Vector3.up, currentGameObject.transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float point))
            targetPos = ray.GetPoint(point);
    }

    /// <summary>
    /// Move our target object to where the mouse is
    /// </summary>
    private void MoveObject()
    {
        currentGameObject.transform.position = Vector3.MoveTowards(currentGameObject.transform.position, targetPos, speed * Time.deltaTime);
    }

    /// <summary>
    /// Raycast from screen to world and find an object to set our target to
    /// </summary>
    private void FindObject()
    {
        //Get a raycast from the screen point of mouse position to the game world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //If it hits an object
        if (Physics.Raycast(ray, out hit))
        {
            //Set the object as our target
            currentGameObject = hit.transform.gameObject;
        }
    }

    /// <summary>
    /// When interacting with a deck, we want to obtain a card from the list and add it to the players hand until they let go.
    /// Alternatively if the player is holding a card and lets go on a deck, they will add it to it
    /// </summary>
    private void HandleDeckInteraction()
    {
        //if already are holding an object
        if (currentGameObject != null)
        {
            //Check if we selected an object
            if (currentGameObject.TryGetComponent(out DeckHolder deck))
            {
                //Draw a card
                CardData card = deck.DrawCard();

                //If we actually drew a card
                if (card != null)
                {
                    Vector3 cardPosition =new Vector3(hit.transform.position.x, cardYOffset, hit.transform.position.z);
                    //Create the drawn card
                    GameObject createdCard = Instantiate(card.cardModel, cardPosition, Quaternion.Euler(deck.GetCardRotationValue()));
                    //Add a box collider to the card so that we can select it again
                    createdCard.AddComponent<BoxCollider>();
                    //Initialise card holder
                    CardHolder cardHolder= createdCard.AddComponent<CardHolder>();
                    cardHolder.cardData = card;
                    //Set our target game object to the card we drew. That way it it will be moved
                    currentGameObject = createdCard;
                }
            }
        }
    }
}
