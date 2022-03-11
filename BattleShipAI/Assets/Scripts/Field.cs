using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool firedUpon, shipPresent;

    public Battleship fieldPartOfShip;

    public Field(bool firedUpon)
    {
        this.firedUpon = firedUpon;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.playerTurn && !firedUpon)
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
