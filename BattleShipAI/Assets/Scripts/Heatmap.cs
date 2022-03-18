using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatmap : MonoBehaviour
{
    public int[,] heatmap;
    private int[,] neighbours = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
    public List<Battleship> playerShipsRemaining = new List<Battleship>();


    /// <summary>
    /// A simple return of the highest value in the heatmap as well as the coordinates of that field. 
    /// </summary>
    /// <param name="indexX"></param>
    /// <param name="indexY"></param>
    /// <returns></returns>
    public int GetHighestHeatMapValue(out int indexX, out int indexY)
    {
        indexX = 0;
        indexY = 0;
        int highestValue = 0;
        for (int y = 0; y < heatmap.GetLength(0); y++)
        {
            for (int x = 0; x < heatmap.GetLength(0); x++)
            {
                if (heatmap[x, y] > highestValue)
                {
                    highestValue = heatmap[x, y];
                    indexX = x;
                    indexY = y;
                }
            }
        }
        return highestValue;
    }

    /// <summary>
    /// Updates the whole HeatMap depending on where the Player have most likely placed ships. 
    /// </summary>
    public void UpdateHeatMap()
    {
        Board board = GameManager.instance.playerBoard;
        heatmap = new int[GameManager.instance.boardSize, GameManager.instance.boardSize]; //reset heatmap

        for (int x = 0; x < board.boardSize; x++)
        {
            for (int y = 0; y < board.boardSize; y++)
            {
                TryToPlaceAllShips(x, y, board, true); //true means horisontal
                TryToPlaceAllShips(x, y, board, false); //false means vertical
                SetPriorityOfShotField(x, y, board);
            }
        }

        GameManager.instance.heatMapStr = Utility.PrintHeatMap(heatmap);

        UI.UpdateHeatMapVisually(heatmap);
    }

    /// <summary>
    /// If a ship has been hit, the AI prioritises shooting around the hit space.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="board"></param>
    public void SetPriorityOfShotField(int posX, int posY, Board board)
    {
        if (!(board[posX, posY].firedUpon
            && board[posX, posY].fieldPartOfShip != null
            && board[posX, posY].fieldPartOfShip.health > 0))
        {
            return; //Only prioritize around this field, if it is hit and has a ship
        }

        bool hitNeighbour = false;
        for (int i = 0; i < 4; i++)
        {
            int x = posX + neighbours[i, 0];
            int y = posY + neighbours[i, 1];

            if (Utility.IsValidCoordinate(x, y, board))
            {
                Field currentField = board[x, y];

                if (currentField.firedUpon
                    && currentField.fieldPartOfShip != null
                    && currentField.fieldPartOfShip.health > 0)
                {
                    hitNeighbour = true;
                }
            }
        }

        if (!hitNeighbour) //no hit neighbour, so prioritize all four surrounding fields
        {
            for (int i = 0; i < 4; i++)
            {
                int x = posX + neighbours[i, 0];
                int y = posY + neighbours[i, 1];

                if (Utility.IsValidCoordinate(x, y, board))
                {
                    if (!board[x, y].firedUpon)
                    {
                        heatmap[x, y] += 100;
                        //Debug.Log("no hit heighbours: "+ posX + neighbours[i, 0]+","+ posY + neighbours[i, 1]);
                    }
                }
            }
        }
        else //a neighbour is hit, so prioritise the other direction
        {
            for (int i = 0; i < 4; i++)
            {
                int x = posX + neighbours[i, 0];
                int y = posY + neighbours[i, 1];

                if (Utility.IsValidCoordinate(x, y, board))
                {
                    if (board[x, y].firedUpon)
                    {
                        int oppositeX = posX + (-1) * neighbours[i, 0];
                        int oppositeY = posY + (-1) * neighbours[i, 1];

                        if (Utility.IsValidCoordinate(oppositeX, oppositeY, board)
                            && !board[oppositeX, oppositeY].firedUpon)
                        {
                            heatmap[oppositeX, oppositeY] += 100;
                            //Debug.Log("prioritise one");
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Simulates placing ships on every possible field of the Player's board to populate a heatmap
    /// so as to find the place where a Player has most likely placed a ship.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="board"></param>
    /// <param name="vertical"></param>
    public void TryToPlaceAllShips(int posX, int posY, Board board, bool vertical)
    {
        foreach (Battleship batship in playerShipsRemaining)
        {
            int length = vertical ? 1 : batship.size;
            int height = vertical ? batship.size : 1;

            if (Utility.IsValidHeatMapPlacement(posX, posY, length, height))
            {
                IncrementHeatMap(posX, posY, length, height);
            }
        }
    }

    /// <summary>
    /// If a ship can be placed in the given spot, all heatmap fields (that the ship would occupy) should then be incremented. 
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="length"></param>
    /// <param name="height"></param>
    public void IncrementHeatMap(int posX, int posY, int length, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                heatmap[x + posX, y + posY]++;
            }
        }
    }
}
