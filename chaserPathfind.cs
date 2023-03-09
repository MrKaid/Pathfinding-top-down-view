using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaserPathfind : MonoBehaviour
{

    [SerializeField] private Transform myTransform;
    [SerializeField] private Rigidbody2D rb;
    private float destinationDistance;
    public float moveSpeed;

    private int currentPathIndex;
    private List<Vector3> pathVectorList;


    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        //make sure the object stays on the right z position/doesnt disappear
        Vector3 pos = myTransform.position;
        pos.z = 0;
        myTransform.position = pos;

        HandleMovement();
    }

    //actually moves the player
    private void HandleMovement()
    {
        //if the list of steps the Character takes is not empty
        if (pathVectorList != null)
        {
            //sets the target position to the next step in pathVectorList
            Vector3 targetPosition = pathVectorList[currentPathIndex];

            //if the character is further than 1 float from the target position
            if (Vector3.Distance(myTransform.position, targetPosition) > .5f)
            {
                //normalize the difference in character and target positions to get a movement direction
                Vector3 moveDir = (targetPosition - myTransform.position).normalized;

                //move the character toward the target
                destinationDistance = Vector3.Distance(myTransform.position, targetPosition);
                myTransform.position = Vector3.MoveTowards(myTransform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                //increment to the next step of pathVectorList
                currentPathIndex++;

                //if we are at the last step/goal node then stop moving
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    //clears the pathVectorList
    private void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    //sets a variable named pathVectorList with a list of the steps we need to take to get to goal node
    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        pathVectorList = pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

}