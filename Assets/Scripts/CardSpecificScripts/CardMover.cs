using UnityEngine;

/// <summary>
/// Moves the cards around based on mouse pointer
/// </summary>
public class CardMover : MonoBehaviour
{
    //The y offset of the card when moved
    [SerializeField] private float cardYOffset;
    //Speed at which we move the cards with
    private readonly float speed = 10f;
    //The target position we want to move the card to
    private Vector3 targetPos;
    //Bools to check if we should move our current object and if we have to use touch input
    private bool isMoving = false,useTouchInput=false;
    //The mouse button we want to check for inputs
    private const int mouseVal = 0;
    //The current game object that will be moved
    private GameObject currentGameObject = null;
    //The hit info of a raycasthit
    private RaycastHit hit;
    //The layer we want to check for cards
    [SerializeField] private LayerMask layerToCheck;

    //The positions of mouse and pointer touches
    private Vector3 touchPos, mousePos;

    private void Update()
    {
        DebugFlipAllCards();

        HandleCardMoving();
    }

    private void HandleCardMoving()
    {
        //Get the positions of both mouse and touch
        if (Input.touchCount > 0)
        {     
            touchPos = Input.GetTouch(0).position;
            useTouchInput = true;
        }
        else
        {
            mousePos = Input.mousePosition;
            useTouchInput = false;
        }

        //If mouse button is being held down
        if (Input.GetMouseButtonDown(mouseVal) || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            //Find an object where the mouse pointer is
            FindObject();
            //enable our moving
            isMoving = true;

            HandleDeckInteraction();
            HandleCardInteraction();
        }
        //If we release the mouse button
        else if (Input.GetMouseButtonUp(mouseVal) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            HandleCardLetGo();
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

        Ray ray = GetRayBasedOnInput();

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
        Ray ray = GetRayBasedOnInput();

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
                    CardObject cardHolder= createdCard.AddComponent<CardObject>();
                    cardHolder.cardData = card;
                    //Set our target game object to the card we drew. That way it it will be moved
                    currentGameObject = createdCard;
                }
                else
                {
                    currentGameObject = null;
                }
            }
        }
    }

    /// <summary>
    /// When interacting with a card, we want to take the card and prevent it from moving until we let it go.
    /// </summary>
    private void HandleCardInteraction()
    {
        //if already are holding an object
        if (currentGameObject != null)
        {
            //Check if we selected an object
            if (currentGameObject.GetComponent<CardObject>()!=null&& currentGameObject.TryGetComponent(out MoveGameObjectToPosition moveTo))
            {
                moveTo.moveToPosition = false;   
            }
        }
    }

    /// <summary>
    /// When letting go of a card, we want it to return to its old position unless we are hovering over an object that would accept the card
    /// </summary>
    private void HandleCardLetGo()
    {
        //if already are holding an object
        if (currentGameObject != null)
        {
            Ray ray = GetRayBasedOnInput();
            if (Physics.Raycast(ray, out hit,100, layerToCheck) && hit.transform.TryGetComponent(out DeckHolder deck)&&currentGameObject.TryGetComponent(out CardObject card))
            {
                deck.AddCard(card.cardData);
                Destroy(currentGameObject);
            }
            //Check if we selected an object
            else if (currentGameObject.GetComponent<CardObject>() != null && currentGameObject.TryGetComponent(out MoveGameObjectToPosition moveTo))
            {
                moveTo.moveToPosition = true;
            }
        }
    }

    /// <summary>
    /// Gets a ray based on what input device is currently being used
    /// </summary>
    private Ray GetRayBasedOnInput()
    {
        if (useTouchInput)
            return Camera.main.ScreenPointToRay(touchPos);
        else
            return Camera.main.ScreenPointToRay(mousePos);
    }

    /// <summary>
    /// Flips all cards - To be sure I matched the requirement of revealing cards, I have added thus debug for keyboard
    /// </summary>
    private void DebugFlipAllCards()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardObject[] cardObjects = FindObjectsOfType<CardObject>();

            foreach (CardObject card in cardObjects)
            {
                card.FlipCard();
            }
        }
    }
}
