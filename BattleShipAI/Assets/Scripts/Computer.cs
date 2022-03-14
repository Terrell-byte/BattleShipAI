using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public static void PlaceShips(ShipPlacer sp,Board board, int boardOffset, int numberOfShips)
    {
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships);

        for (int y = 0; y < battleships.Length; y++)
        {
            battleships[y].x = battleships[y].x + boardOffset;
            for (int x = 0; x < battleships[y].size; x++)
            {
                board.GetBoard()[x, y].shipPresent = true;
                board.GetBoard()[x, y].fieldPartOfShip = battleships[y];
            }
        }
    }
}
