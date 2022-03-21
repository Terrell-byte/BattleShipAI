using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The board consistent of n*n fields
/// </summary>
public class Board 
{
    private Field[,] board;
    public int boardSize;

    public Board(int boardSize)
    {
        board = new Field[boardSize, boardSize];
        this.boardSize = boardSize;
    }

    public Field[,] GetBoard() => board;

    public Field this[int x, int y]
    {
        get { return board[x, y]; }
        set { board[x, y] = value; }
    }
}
