using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TextArea(15,15)]
    public string boardStr;
    public ShipPlacer shipPlacer;


    public int boardSize;
    public int numberOfShips = 5;
    public bool playerTurn; // If false -> computer turn

    public bool gameStarted;

    public Board board;

    public static GameManager instance;
    public GameObject fieldPrefab;
    public bool placingShips;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships);

        //board = Utility.GenerateBoard(new Board(boardSize));
        board = Utility.GenerateGameBoard(new Board(boardSize));

        LetPlayerPlaceShips();

        boardStr = Utility.PrintBoard(board.GetBoard());
    }

    private void LetPlayerPlaceShips()
    {
        placingShips = true;
        shipPlacer.StartPlacingShips();
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
