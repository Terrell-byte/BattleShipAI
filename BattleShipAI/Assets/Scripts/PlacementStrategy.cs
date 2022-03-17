using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementStrategy : MonoBehaviour
{
    /*
     * Placement strategy
     */

    /// <summary>
    /// Places the ships at random. But following certain rules:
    /// 1) Do not place multiple ships on the same row unless they are vertically placed
    /// 2) Do not place ships next to one another
    /// 3) Avoid the middle
    /// Will sometimes break a rule to confuse the player. 
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="board"></param>
    /// <param name="boardOffset"></param>
    /// <param name="numberOfShips"></param>
    public void PlaceShipsStrategically(ShipPlacer sp, Board board, int boardOffset, int numberOfShips)
    {
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships, false);

        foreach (Battleship b in battleships)
        {
            bool hasPlacedShip = false;
            while (!hasPlacedShip)
            {
                //int tryToFollowRules = 0;
                int xPos = Random.Range(0, board.boardSize);
                int yPos = Random.Range(0, board.boardSize);
                bool vertical = Random.Range(0, 2) == 1 ? true : false;
                int height = vertical ? b.size : 1;
                int length = vertical ? 1 : b.size;

                // The middle is only off-limits, if the board is large enough. 
                if (board.boardSize > 8)
                {
                    // A ship should not be placed in the middle
                    if (IsInMiddle(xPos, yPos, height, length, board))
                    {
                        continue;
                    }
                }

                // Two ships may only share one row if at least one of them is vertical
                if (MultipleRowSpacesTaken(yPos, board))
                {
                    continue;
                }

                // Should not place ship right above or below another
                if ((yPos + 1 < board.boardSize && !Utility.IsValidPlacement(xPos, yPos + 1, b.size, 1, board))
                    || (yPos - 1 >= 0 && !Utility.IsValidPlacement(xPos, yPos - 1, b.size, 1, board)))
                {
                    continue;
                }

                if (Utility.IsValidPlacement(xPos, yPos, length, height, board))
                {
                    for (int j = 0; j < b.size; j++)
                    {
                        int x = vertical ? xPos : xPos + j;
                        int y = vertical ? yPos + j : yPos;
                        board[x, y].shipPresent = true;
                        board[x, y].fieldPartOfShip = b;
                    }
                    hasPlacedShip = true;
                }

                b.vertical = vertical;
                b.x = xPos + boardOffset;
                b.y = yPos;
            }

        }
    }

    /// <summary>
    /// Checks whether multiple spaces in a given row are taken.
    /// </summary>
    /// <param name="yPos"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private bool MultipleRowSpacesTaken(int yPos, Board board)
    {
        int spacesTaken = 0;
        for (int x = 0; x < board.boardSize; x++)
        {
            if (board[x, yPos].fieldPartOfShip != null)
            {
                spacesTaken++;
                if (spacesTaken > 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsInMiddle(int posX, int posY, int height, int length, Board board)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (posX + x < (board.boardSize / 2) + 2 && posX + x > (board.boardSize / 2) - 2
                    && posY + y < (board.boardSize / 2) + 2 && posY + y > (board.boardSize / 2) - 2)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Places the AIs ships in the corner for easy testing. 
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="board"></param>
    /// <param name="boardOffset"></param>
    /// <param name="numberOfShips"></param>
    public void PlaceShipsCorner(ShipPlacer sp, Board board, int boardOffset, int numberOfShips)
    {
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships, false);
        for (int y = 0; y < battleships.Length; y++)
        {
            battleships[y].x = battleships[y].x + boardOffset;
            for (int x = 0; x < battleships[y].size; x++)
            {
                board[x, y].shipPresent = true;
                board[x, y].fieldPartOfShip = battleships[y];
            }
        }
    }
}
