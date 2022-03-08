using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field 
{
    public bool firedUpon, shipPresent;

    public Battleship fieldPartOfShip;

    public Field(bool firedUpon)
    {
        this.firedUpon = firedUpon;
    }
}
