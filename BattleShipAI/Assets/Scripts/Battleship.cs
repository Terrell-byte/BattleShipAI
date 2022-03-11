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
        this.health = size;
        this.x = x;
        this.y = y;
        this.vertical = vertical;
    }

    public Battleship(int size, bool vertical)
    {
        this.size = size;
        this.health = size;
        this.vertical = vertical;
    }

    public void Hit()
    {
        
        health--;
        //Debug.Log("HIT" + health);
        if (health <= 0)
        {
            GameManager.instance.ShipSunk(this);
            GameObject sunkShip = new GameObject();
            sunkShip.transform.position = new Vector2(x, y);
            sunkShip.AddComponent<SpriteRenderer>().sprite = GameManager.instance.gameObject.GetComponent<SpriteManager>().shipSprite[size - 2];
            if(size == 3)
            {
                sunkShip.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            }
        }
    }
}
