using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [TextArea(15,15)]
    public string heatMapStr;

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public ShipPlacer shipPlacer;
    public Text victoryText; 

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

    public bool intelligentAI;

    public Camera camera;

    private void Awake()
    {
        instance = this;
    }

    public void PlayerIsReady()
    {
        GameObject.Find("Playerstart").SetActive(false);
        GameObject.Find("BoardSize").SetActive(false);


        if (boardSize > 10)
        {
            SetCamera();
        }

        boardOffset = boardSize + 2;
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships, true);

        playerBoard = Utility.GeneratePlayerBoard(new Board(boardSize));
        computerBoard = Utility.GenerateComputerBoard(new Board(boardSize), boardOffset);

        LetPlayerPlaceShips(battleships);
        computer.PlaceShipsRandom(shipPlacer, computerBoard, boardOffset, numberOfShips);

        computerShipsRemaining = numberOfShips;
        playerShipsRemaining = numberOfShips;

        foreach (Battleship bs in battleships)
        {
            computer.playerShipsRemaining.Add(bs);
        }
        computer.UpdateHeatMap();
    }

    private void SetCamera()
    {
        float scale = ((float)boardSize / 10);
        camera.orthographicSize = camera.orthographicSize*scale;
        camera.transform.position = new Vector3(camera.transform.position.x * scale, camera.transform.position.y * scale,-1);
    }



    /// <summary>
    /// Starts the game after the ships have been placed. 
    /// </summary>
    public void StartGame()
    {
        Debug.Log(""+ ((boardSize / 2) + 1) + " " + ((boardSize / 2) - 1));

        gameStarted = true;
        shipPlacer.gameObject.SetActive(false);
        if (!playerTurn)
        {
            StartComputerTurn();
        }
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
        if (!playerTurn)
        {
            playerShipsRemaining--;
            Debug.Log("Player's "+battleship.name + " is sunk");
        }
        else
        {
            computerShipsRemaining--;
            Debug.Log("Computer's " + battleship.name + " is sunk");
        }
        
        if(playerShipsRemaining <= 0)
        {
            Debug.Log("Computer has won");
            victoryText.text = "Defeat";
            victoryText.gameObject.SetActive(true);
            gameStarted = false;
        }

        if(computerShipsRemaining <= 0)
        {
            victoryText.text = "Victory";
            victoryText.gameObject.SetActive(true);
            Debug.Log("Player has won");
            gameStarted = false;
        }
    }

    
}
