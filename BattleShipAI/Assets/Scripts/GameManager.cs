using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TextArea(15,15)]
    public string heatMapStr;
    public ShipPlacer shipPlacer;

    private int playerShipsRemaining, computerShipsRemaining;

    public int boardSize, boardOffset;
    public int numberOfShips = 5;
    public bool playerTurn; // If false -> computer turn
    public Computer computer;

    public bool gameStarted, showHeatMap;

    public Board playerBoard, computerBoard;

    public static GameManager instance;
    public GameObject fieldPrefab;
    public bool placingShips;
    public GameObject heatMapCanvas;
    public GameObject heatmapText;

    public int Difficulty;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        boardOffset = boardSize + 2;
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships,true);

        playerBoard = Utility.GeneratePlayerBoard(new Board(boardSize));
        computerBoard = Utility.GenerateComputerBoard(new Board(boardSize),boardOffset);

        LetPlayerPlaceShips(battleships);
        computer.PlaceShipsCorner(shipPlacer,computerBoard,boardOffset, numberOfShips);

        computerShipsRemaining = numberOfShips;
        playerShipsRemaining = numberOfShips;

        foreach(Battleship bs in battleships)
        {
            computer.playerShipsRemaining.Add(bs);
        }
        computer.UpdateHeatMap();
    }

    /// <summary>
    /// Starts the game after the ships have been placed. 
    /// </summary>
    public void StartGame()
    {
        gameStarted = true;
        shipPlacer.gameObject.SetActive(false);
    }

    /// <summary>
    /// Is called every time the Player has taken a shot. 
    /// </summary>
    public void StartComputerTurn()
    {
        computer.StartTurn(playerBoard);
    }

    /// <summary>
    /// Is called when the Player should start placing their ships. 
    /// </summary>
    /// <param name="battleships"></param>
    private void LetPlayerPlaceShips(Battleship[] battleships)
    {
        placingShips = true;
        shipPlacer.StartPlacingShips(battleships);
    }

    /// <summary>
    /// Is called whenever a Battleship is sunk. 
    /// </summary>
    /// <param name="battleship"></param>
    public void ShipSunk(Battleship battleship)
    {
        Debug.Log(battleship.name + " is sunk");

        if (!playerTurn)
        {
            playerShipsRemaining--;
            //computer.playerShipsRemaining.Remove(battleship);
        }
        else
        {
            computerShipsRemaining--;
        }
        
        if(playerShipsRemaining <= 0)
        {
            Debug.Log("Computer has won");
            gameStarted = false;
        }

        if(computerShipsRemaining <= 0)
        {
            Debug.Log("Player has won");
            gameStarted = false;
        }
    }

    
}
