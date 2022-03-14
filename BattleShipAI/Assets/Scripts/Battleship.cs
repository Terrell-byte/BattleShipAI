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
        name = setName();
    }

    private string setName()
    {
        switch(size)
        {
            case 2: return "Destroyer";
            case 3: return "Submarine";
            case 4: return "Battleship";
            case 5: return "Carrier";
            default: return "Unknown ship";
        }
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
            Debug.Log(x);
            GameManager.instance.ShipSunk(this);
            GameObject sunkShip = new GameObject();
            sunkShip.name = "Sunken "+name;
            sunkShip.transform.SetParent(GameObject.Find("ShipsParent").transform);
            sunkShip.AddComponent<SpriteRenderer>().sprite = GameManager.instance.gameObject.GetComponent<SpriteManager>().shipSprite[size - 2];
            sunkShip.transform.position = new Vector2(x, y);
            if (size == 3)
            {
                sunkShip.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            }

            if (vertical)
            {
                sunkShip.transform.RotateAround(new Vector3(x, y, 0), new Vector3(0, 0, 1), 90);
                sunkShip.transform.position = new Vector2(x+1, y);
            }
            Debug.Log(x);

        }
    }
}
