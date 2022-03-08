using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleship 
{
    public int size, health, x, y;
    public string name;
    public bool vertical;


    public Battleship(int x, int y, int size, bool vertical)
    {
        this.size = size;
        this.x = x;
        this.y = y;
        this.vertical = vertical;
    }

    public void Hit()
    {
        health--;
        if(health <= 0)
        {
            GameManager.instance.ShipSunk(this);
        }
    }
}
