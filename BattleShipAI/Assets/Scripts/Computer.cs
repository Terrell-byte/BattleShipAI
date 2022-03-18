using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public Heatmap heatmap = new Heatmap();
    private PlacementStrategy placementStrategy = new PlacementStrategy();
    //public List<Battleship> playerShipsRemaining = new List<Battleship>();


    //private List<Field> hitFields = new List<Field>();

    private void Awake()
    {
        heatmap = new Heatmap();
        heatmap.InitPreviousPlayerPlacement();
    }

    /*
     * Placement strategy
     */

    public void PlaceComputerShips(ShipPlacer shipPlacer, Board computerBoard, int boardOffset, int numberOfShips)
    {
        placementStrategy.PlaceShipsStrategically(shipPlacer, computerBoard, boardOffset, numberOfShips);
    }

    /*
     * Firing strategy
     */

    /// <summary>
    /// How the AI starts each turn. 
    /// </summary>
    /// <param name="board"></param>
    public void StartTurn(Board board)
    {
        int x, y = 0;
        int highestHeatMapValue = heatmap.GetHighestHeatMapValue(out x, out y);


        //on higher difficulty it activates the intelligent AI
        if (GameManager.instance.intelligentAI)
        {
            if (!board[x, y].firedUpon)
            {
                board[x, y].FieldHit();
            }
            else //if the heatmap is bugged, the computer will just slowly work through unshot fields. This does not happen.
            {
                x = 0;
                y = 0;
                while (x < board.boardSize 
                    && y < board.boardSize
                    && board[x, y].firedUpon)
                {
                    x++;
                    y++;
                }
                board[x, y].FieldHit();
            }
        }
        else //Shoots randomly if on low difficulty
        {
            bool hasShot = false;
            int hasTried = 0;
            while (!hasShot && hasTried < 1000)
            {
                int xCor = Random.Range(0, board.boardSize);
                int yCor = Random.Range(0, board.boardSize);
                if (!board[x, y].firedUpon)
                {
                    board[xCor, yCor].FieldHit();
                    hasShot = true;
                }
                hasTried++;
            }
        }

        heatmap.UpdateHeatMap();

        GameManager.instance.playerTurn = true; //end computer turn
    }

    
}
