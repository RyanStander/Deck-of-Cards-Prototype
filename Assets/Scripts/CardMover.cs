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

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentGameObject = hit.transform.gameObject;
        }
    }
}
