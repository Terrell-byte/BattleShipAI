using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    /// <summary>
    /// Prints an upside-down representation of the board. Not currently used. 
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public static string PrintBoard(Field[,] board)
    {
        string boardStr = "";
        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                if (board[x,y].firedUpon)
                {
                    boardStr += "X";
                }
                else if(board[x,y].fieldPartOfShip != null)
                {
                    boardStr += "S";
                }
                else
                {
                    boardStr += "_";
                }
            }
            
            boardStr += "\n";
        }
        return boardStr;
    }

    /// <summary>
    /// Prints a text-representation of the heatmap. 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static string PrintHeatMap(int[,] map)
    {
        string[] arrayStr = new string[GameManager.instance.boardSize];
        string boardStr = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                boardStr += map[x, y] + " ";
            }
            arrayStr[y] = boardStr;
            boardStr = "";
            //boardStr += "\n";
        }

        for (int i = arrayStr.Length-1; i >= 0; i--)
        {
            boardStr += arrayStr[i];
            boardStr += "\n";
        }

        return boardStr;
    }

    /// <summary>
    /// Creates a board for the Player to place ships. Both visually and backend. 
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public static Board GeneratePlayerBoard(Board board)
    {
        GameObject boardParent = new GameObject();
        boardParent.name = "PlayerBoardParent";
        for (int y = 0; y < board.GetBoard().GetLength(1); y++)
        {
            for (int x = 0; x < board.GetBoard().GetLength(0); x++)
            {
                GameObject fieldOb = Instantiate(GameManager.instance.fieldPrefab, new Vector2(x, y), Quaternion.identity);
                fieldOb.AddComponent<Field>();
                fieldOb.transform.SetParent(boardParent.transform);
                Field field = fieldOb.GetComponent<Field>();
                field.x = x;
                field.y = y;
                field.enemyField = false;

                board.GetBoard()[x, y] = field;
            }
        }
        return board;
    }

    /// <summary>
    /// Creates a board for the AI to place ships. Both visually and backend.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static Board GenerateComputerBoard(Board board, int offset)
    {
        GameObject boardParent = new GameObject();
        boardParent.name = "PlayerComputerParent";
        for (int y = 0; y < board.GetBoard().GetLength(1); y++)
        {
            for (int x = 0; x < board.GetBoard().GetLength(0); x++)
            {
                GameObject fieldOb = Instantiate(GameManager.instance.fieldPrefab, new Vector2(x+offset, y), Quaternion.identity);
                fieldOb.AddComponent<Field>();
                fieldOb.transform.SetParent(boardParent.transform);
                Field field = fieldOb.GetComponent<Field>();
                field.x = x;
                field.y = y;
                field.enemyField = true;

                board.GetBoard()[x, y] = field;
            }
        }
        return board;
    }

    /// <summary>
    /// Generates five Battleship objects.
    /// </summary>
    /// <param name="battleships"></param>
    /// <returns></returns>
    public static Battleship[] GenerateBattleships(Battleship[] battleships, bool playerIsOwner)
    {
        int[] sizeArray = { 2, 3, 4, 4, 5 };
        for (int i = 0; i < 5; i++)
        {
            battleships[i] = new Battleship(0, i, sizeArray[i], false, playerIsOwner);
        }

        return battleships;
    }

    /// <summary>
    /// Checks whether a ship can be placed in the given field. 
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="length"></param>
    /// <param name="height"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public static bool IsValidPlacement(int posX, int posY, int length, int height, Board board)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if(x+posX > board.GetBoard().GetLength(0) || y+posY > board.GetBoard().GetLength(0) || board.GetBoard()[posX + x, posY + y].fieldPartOfShip != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Checks whether a whole ship can be placed in a spot or if it is blocked by an already fired upon field. 
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="length"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static bool IsValidHeatMapPlacement(int posX, int posY, int length, int height)
    {
        Board board = GameManager.instance.playerBoard;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (x + posX >= board.GetBoard().GetLength(0) || y + posY >= board.GetBoard().GetLength(0) || board.GetBoard()[posX + x, posY + y].firedUpon)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
