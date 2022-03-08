using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board 
{
    public Field[,] board;
    public int boardSize;

    public Board(int boardSize)
    {
        board = new Field[boardSize, boardSize];
        this.boardSize = boardSize;
    }

    public Field[,] GetBoard()
    {
        return board;
    }
}
