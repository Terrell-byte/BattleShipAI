using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool firedUpon, shipPresent;

    public Battleship fieldPartOfShip;
    public int x, y;
    public bool enemyField; //player or computer's side

    public Field(int x, int y, bool firedUpon)
    {
        this.firedUpon = firedUpon;
        this.x = x;
        this.y = y;
    }

    private void OnMouseDown()
    {
        if (enemyField && GameManager.instance.gameStarted && GameManager.instance.playerTurn && !firedUpon)
        {
            firedUpon = true;
            GameManager.instance.playerTurn = false;
            if (shipPresent)
            {
                fieldPartOfShip.Hit();
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
        } else if (!enemyField && GameManager.instance.placingShips)
        {
            GameManager.instance.shipPlacer.MoveToField(transform.position);
            GameManager.instance.shipPlacer.PlaceShip(GameManager.instance.playerBoard,x,y);
        }



        //for testing
        GameManager.instance.playerTurn = true;
    }

    private void OnMouseOver()
    {
        if (GameManager.instance.placingShips)
        {
            GameManager.instance.shipPlacer.MoveToField(transform.position);
        }
    }

}
