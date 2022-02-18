using UnityEngine;

public class MoveGameObjectToPosition : MonoBehaviour
{
    //Speed that it moves to its target
    private float moveToSpeed=1;
    //The target position
    private Vector3 targetPosition;
    //Whether it should be moving
    public bool moveToPosition=false;

    /// <summary>
    /// Sets a target position of the gameobject and moves it to there. Once it reaches the destination it will stop.
    /// </summary>
    public void SetTargetDestination(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        moveToPosition = true;
    }

    /// <summary>
    /// Set the move to speed of the object
    /// </summary>
    public void SetSpeed(float moveToSpeed)
    {
        this.moveToSpeed = moveToSpeed;
    }

    private void FixedUpdate()
    {
        //Move towards target if we have moveToPosition true
        if (moveToPosition)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveToSpeed * Time.deltaTime);
    }
}
