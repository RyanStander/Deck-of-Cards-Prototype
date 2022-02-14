using UnityEngine;

/// <summary>
/// Moves the cards around based on mouse pointer
/// </summary>
public class CardMover : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false;
    private const int MOUSE = 0;
    [SerializeField]private GameObject currentGameObject = null;
    private Camera mainCamera;
    private RaycastHit hit;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(MOUSE))
        {
            FindObject();
            isMoving = true;
            HandleDeckInteraction();
        }
        else if (Input.GetMouseButtonUp(MOUSE))
        {
            isMoving = false;
            currentGameObject = null;
        }

        if (isMoving)
        {
            if (currentGameObject != null)
            {
                SetTarggetPosition();
                MoveObject();
            }
        }
    }
    private void SetTarggetPosition()
    {
        Plane plane = new Plane(Vector3.up, currentGameObject.transform.position);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float point))
            targetPos = ray.GetPoint(point);
    }
    private void MoveObject()
    {
        currentGameObject.transform.position = Vector3.MoveTowards(currentGameObject.transform.position, targetPos, speed * Time.deltaTime);
    }

    private void FindObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            currentGameObject = hit.transform.gameObject;
        }
    }

    /// <summary>
    /// When interacting with a deck, we want to obtain a card from the list and add it to the players hand until they let go.
    /// Alternatively if the player is holding a card and lets go on a deck, they will add it to it
    /// </summary>
    private void HandleDeckInteraction()
    {
        //if already are holding a card
        if (currentGameObject!=null)
        {
            if (currentGameObject.TryGetComponent(out DeckHolder deck))
            {
                CardData card = deck.DrawCard();

                GameObject createdCard = Instantiate(card.cardModel, hit.transform.position, Quaternion.Euler(deck.GetCardRotationValue()));
                //Add a box collider to the card so that we can select it again
                createdCard.AddComponent<BoxCollider>();
                currentGameObject = createdCard;
                isMoving = true;
            }
        }
        //else if we arent holding anything
        else
        {

        }
    }
}
