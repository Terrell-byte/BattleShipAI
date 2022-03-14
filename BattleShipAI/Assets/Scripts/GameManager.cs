using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TextArea(15,15)]
    public string boardStr;
    public ShipPlacer shipPlacer;


    public int boardSize, boardOffset;
    public int numberOfShips = 5;
    public bool playerTurn; // If false -> computer turn

    public bool gameStarted;

    public Board playerBoard, computerBoard;

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

        playerBoard = Utility.GeneratePlayerBoard(new Board(boardSize));
        computerBoard = Utility.GenerateComputerBoard(new Board(boardSize),boardOffset);

        LetPlayerPlaceShips(battleships);
        Computer.PlaceShips(shipPlacer,computerBoard,boardOffset, numberOfShips);
    }

    public void StartGame()
    {
        gameStarted = true;
        shipPlacer.gameObject.SetActive(false);
    }

    private void LetPlayerPlaceShips(Battleship[] battleships)
    {
        placingShips = true;
        shipPlacer.StartPlacingShips(battleships);
    }

    public void ShipSunk(Battleship battleship)
    {
        Debug.Log("Ship is sunk");
    }

    
}
