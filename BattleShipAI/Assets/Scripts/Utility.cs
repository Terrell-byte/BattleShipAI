using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
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


    public static Battleship[] GenerateBattleships(Battleship[] battleships)
    {
        int[] sizeArray = { 2, 3, 4, 4, 5 };
        for (int i = 0; i < 5; i++)
        {
            battleships[i] = new Battleship(0, i, sizeArray[i], false);
        }

        return battleships;
    }

}
