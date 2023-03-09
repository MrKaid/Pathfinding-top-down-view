using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node
{
    //initializes our stuff
    private myGrid<node> grid;
    public int row, col, f, g, h;

    public bool isWalkable;
    public node parent;

    //constructor for our node Grid object
    public node(myGrid<node> grid, int x, int y)
    {
        this.grid = grid;
        this.row = x;
        this.col = y;
        isWalkable = true;
    }

    public void setF()
    {
        f = g + h;
    }

    public void setG(int value)
    {
        g = value;
    }
    public void setH(int value)
    {
        h = value;
    }

    //sets whether that node is passable(is the location of a barrier) or not
    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

}