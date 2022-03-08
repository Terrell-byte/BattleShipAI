using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility 
{
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

    public static Battleship[] GenerateBattleships(Battleship[] battleships)
    {
        int[] sizeArray = { 2, 3, 4, 4, 5 };
        for (int i = 0; i < 5; i++)
        {
            battleships[i] = new Battleship(0, i, sizeArray[i], false);
        }

        return battleships;
    }

    public static Board GenerateBoard(Board board)
    {
        
        for (int y = 0; y < board.GetBoard().GetLength(1); y++)
        {
            for (int x = 0; x < board.GetBoard().GetLength(0); x++)
            {

                board.GetBoard()[x, y] = new Field(false);

            }
        }
        return board;
    }

    public static Board GenerateRandomBoard(Board board)
    {

        for (int y = 0; y < board.GetBoard().GetLength(1); y++)
        {
            for (int x = 0; x < board.GetBoard().GetLength(0); x++)
            {
                board.GetBoard()[x, y] = new Field(false);
            }
        }

        
        

        return board;
    }

}
