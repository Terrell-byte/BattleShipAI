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

    /// <summary>
    /// When the Player tries to place a ship or shoot the AI's board. 
    /// </summary>
    private void OnMouseDown()
    {
        if (!enemyField && GameManager.instance.placingShips)
        {
            GameManager.instance.shipPlacer.MoveToField(transform.position);
            GameManager.instance.shipPlacer.PlaceShip(GameManager.instance.playerBoard, x, y);
        }
        else
        {
            if(GameManager.instance.gameStarted && GameManager.instance.playerTurn && !firedUpon && enemyField && GameManager.instance.gameStarted)
            {
                FieldHit();
                GameManager.instance.playerTurn = false;
                GameManager.instance.StartComputerTurn();
            }
        }
        

    }

    /// <summary>
    /// When a field is chosen for attack
    /// </summary>
    public void FieldHit()
    {
        firedUpon = true;
        if (shipPresent)
        {
            fieldPartOfShip.Hit();
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
    }

    /// <summary>
    /// If the Player is placing their ships, the visual representation should be moved to the current field's position. 
    /// </summary>
    private void OnMouseOver()
    {
        if (GameManager.instance.placingShips)
        {
            GameManager.instance.shipPlacer.MoveToField(transform.position);
        }
    }

}
