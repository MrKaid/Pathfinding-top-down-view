using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaserFollowBehavior : MonoBehaviour
{


    [SerializeField] private chaserPathfind characterPathfinding;
    public pathfinding pathfinding;

    public LayerMask layer;

    private void Start()
    {
        //initializes grid and nodes to 18 rows by 15 columns
        //creates our pathfinding object
        pathfinding = new pathfinding(17, 33, layer);

        FindObjectOfType<SpawnTiles>().Init(17, 33);
    }

    private void Update()
    {
        //if mouse left button is clicked:
            //Target Position
            //Get the mouse position and find out where in t    he grid it was clicked
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);

        //record the players position as a Vector3
        Vector3 CharPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        //pass Target Position to playerPathfind class to set the target position
        characterPathfinding.SetTargetPosition(CharPos);

        //if mouse right button is clicked:
        if (Input.GetMouseButtonDown(1))
        {
            //Impassable Position
            //Get the mouse position and find out where in the grid it was clicked
            Vector3 mouseSWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int s, out int t);
            //Debug.Log(x + " " + y);

            ////set the position/node on the Grid as an Impassable position
            //pathfinding.GetNode(x, y).SetIsWalkable(false);
            Debug.Log(x + " " + y + " " + pathfinding.GetNode(x, y).isWalkable);


        }
    }

}