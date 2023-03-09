using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public GameObject tile;

    public chaserFollowBehavior chaser;

    public void Init(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject temp = Instantiate(tile, chaser.pathfinding.GetPositionOfNode(x, y), Quaternion.identity);

                temp.transform.localScale = new Vector3(.724f, .724f, .724f);

                if (chaser.pathfinding.GetNode(x, y).isWalkable)
                    temp.GetComponent<SpriteRenderer>().color = Color.white;
                else
                    temp.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
}
