using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TextArea(15,15)]
    public string boardStr;


    public int boardSize;
    public int numberOfShips = 5;

    

    private Board board;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships);

        board = Utility.GenerateBoard(new Board(boardSize));


        for (int i = 0; i < numberOfShips; i++)
        {
            PlaceShip(board, battleships[i]);
        }


        boardStr = Utility.PrintBoard(board.board);
    }

    public void ShipSunk(Battleship battleship)
    {
        Debug.Log("Ship is sunk");
    }

    public void PlaceShip(Board board, Battleship battleship)
    {
        for(int i = 0; i < battleship.size; i++)
        {
            if (!battleship.vertical)
            {
                board.GetBoard()[battleship.x+i, battleship.y].shipPresent = true;
                board.GetBoard()[battleship.x + i, battleship.y].fieldPartOfShip = battleship;
            }
            else
            {
                board.GetBoard()[battleship.x, battleship.y+i].shipPresent = true;
                board.GetBoard()[battleship.x, battleship.y+i].fieldPartOfShip = battleship;
            }
        }
    }
}
