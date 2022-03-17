using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public int[,] heatMap;
    private List<Field> hitFields = new List<Field>();
    public List<Battleship> playerShipsRemaining = new List<Battleship>();
    private int[,] neighbours = { {1, 0 }, {0,1 },{-1,0},{ 0,-1 } };

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

    public void PlaceShipsRandom(ShipPlacer sp, Board board, int boardOffset, int numberOfShips)
    {
        Battleship[] battleships = new Battleship[numberOfShips];
        battleships = Utility.GenerateBattleships(battleships,false);

        foreach (Battleship b in battleships)
        {
            bool hasPlacedShip = false;
            while(!hasPlacedShip)
            {
                //int tryToFollowRules = 0;
                int xPos = Random.Range(0, board.GetBoard().GetLength(0));
                int yPos = Random.Range(0, board.GetBoard().GetLength(1));
                bool vertical = Random.Range(0, 2) == 1 ? true : false;

                if(board.GetBoard().GetLength(0) > 8) //The middle is only off-limits, if the board is large enough. 
                {
                    if (IsInMiddle(xPos, yPos, vertical, b.size, board)) // Should not be placed in the middle
                    {
                        continue;
                    }
                }
                
                if (HasMultipleRowSpacesTaken(yPos,board)) //Only a vertical ship may be in a row also
                {
                    continue;
                }

                if ((yPos+1 < board.GetBoard().GetLength(1) && !Utility.IsValidPlacement(xPos, yPos+1, b.size, 1, board))
                    || (yPos - 1 >= 0 && !Utility.IsValidPlacement(xPos, yPos - 1, b.size, 1, board))) //Should not place ship right above or below another
                {
                    continue;
                }


                //int height = vertical ? b.size : 1;
                //int length = vertical ? 1 : b.size;

                if (vertical)
                {
                    if (Utility.IsValidPlacement(xPos, yPos, 1, b.size, board))
                    {
                        for (int j = 0; j < b.size; j++)
                        {
                            board.GetBoard()[xPos, yPos + j].shipPresent = true;
                            board.GetBoard()[xPos, yPos + j].fieldPartOfShip = b;
                        }
                        hasPlacedShip = true;
                    }
                }
                else
                {
                    if (Utility.IsValidPlacement(xPos, yPos, b.size, 1, board))
                    {
                        for (int j = 0; j < b.size; j++)
                        {
                            board.GetBoard()[xPos + j, yPos].shipPresent = true;
                            board.GetBoard()[xPos + j, yPos].fieldPartOfShip = b;
                        }
                        hasPlacedShip = true;
                    }
                }

                b.vertical = vertical;
                b.x = xPos + boardOffset;
                b.y = yPos;
            }

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="yPos"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private bool HasMultipleRowSpacesTaken(int yPos, Board board)
    {
        int spacesTaken = 0;
        for (int x = 0; x < board.boardSize; x++)
        {
            if (board.GetBoard()[x, yPos].fieldPartOfShip != null)
            {
                spacesTaken++;
                if(spacesTaken > 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsInMiddle(int posX, int posY, bool vertical, int size, Board board)
    {
        int height = vertical ? size : 1;
        int length = vertical ? 1 : size;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (posX+x < (board.boardSize / 2) + 2 && posX+x > (board.boardSize / 2) - 2
                            && posY+y < (board.boardSize / 2) + 2 && posY+y > (board.boardSize / 2) - 2)
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
        battleships = Utility.GenerateBattleships(battleships,false);
        for (int y = 0; y < battleships.Length; y++)
        {
            battleships[y].x = battleships[y].x + boardOffset;
            for (int x = 0; x < battleships[y].size; x++)
            {
                board.GetBoard()[x, y].shipPresent = true;
                board.GetBoard()[x, y].fieldPartOfShip = battleships[y];
            }
        }
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
        int highestHeatMapValue = GetHighestHeatMapValue(out x,out y);

        
        //on higher difficulty it activates the intelligent AI
        if (GameManager.instance.intelligentAI)
        {
            if (!board.GetBoard()[x, y].firedUpon)
            {
                board.GetBoard()[x, y].FieldHit();
            }
            else //if the heatmap is bugged, the computer will just slowly work through unshot fields
            {
                x = 0;
                y = 0;
                while (x < board.GetBoard().GetLength(0) && y < board.GetBoard().GetLength(1)
                    && board.GetBoard()[x, y].firedUpon)
                {
                    x++;
                    y++;
                }
                board.GetBoard()[x, y].FieldHit();
            }

        }
        else //Shoots randomly if on low difficulty
        {
            bool hasShot = false;
            int hasTried = 0;
            while (!hasShot && hasTried < 1000)
            {
                int xCor = Random.Range(0, board.GetBoard().GetLength(0));
                int yCor = Random.Range(0, board.GetBoard().GetLength(1));
                if (!board.GetBoard()[x, y].firedUpon)
                {
                    board.GetBoard()[xCor, yCor].FieldHit();
                    hasShot = true;
                }
                hasTried++;
            }
        }

        UpdateHeatMap();

        GameManager.instance.playerTurn = true; //end computer turn
    }

    /// <summary>
    /// A simple return of the highest value in the heatmap as well as the coordinates of that field. 
    /// </summary>
    /// <param name="indexX"></param>
    /// <param name="indexY"></param>
    /// <returns></returns>
    private int GetHighestHeatMapValue(out int indexX, out int indexY)
    {
        indexX = 0;
        indexY = 0;
        int highestValue = 0;
        for (int y = 0; y < heatMap.GetLength(0); y++)
        {
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                if(heatMap[x,y] > highestValue)
                {
                    highestValue = heatMap[x, y];
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
        heatMap = new int[GameManager.instance.boardSize, GameManager.instance.boardSize]; //reset heatmap

        for (int x = 0; x < board.GetBoard().GetLength(0); x++)
        {
            for (int y = 0; y < board.GetBoard().GetLength(0); y++)
            {
                TryToPlaceAllShips(x, y, board, true); //true means horisontal
                TryToPlaceAllShips(x, y, board, false); //false means vertical
                SetPriorityOfShotField(x,y,board);
            }
        }

        GameManager.instance.heatMapStr = Utility.PrintHeatMap(heatMap);

        UI.UpdateHeatMapVisually(heatMap);
    }

    /// <summary>
    /// If a ship has been hit, the AI prioritises shooting around the hit space.
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="board"></param>
    private void SetPriorityOfShotField(int posX, int posY, Board board)
    {
        if(!(board.GetBoard()[posX, posY].firedUpon
            && board.GetBoard()[posX,posY].fieldPartOfShip != null
            && board.GetBoard()[posX, posY].fieldPartOfShip.health > 0))
        {
            return; //Only prioritize around this field, if it is hit and has a ship
        }

        bool hitNeighbour = false;
        for (int i = 0; i < 4; i++)
        { 
            if(posX + neighbours[i, 0] < board.GetBoard().GetLength(0) && posY + neighbours[i, 1] < board.GetBoard().GetLength(0)
                && posX + neighbours[i, 0] >= 0 && posY + neighbours[i, 1] >= 0)
            {
                if (board.GetBoard()[posX + neighbours[i, 0], posY + neighbours[i, 1]].firedUpon &&
                    board.GetBoard()[posX + neighbours[i, 0], posY + neighbours[i, 1]].fieldPartOfShip != null
                    && board.GetBoard()[posX + neighbours[i, 0], posY + neighbours[i, 1]].fieldPartOfShip.health>0)
                {
                    hitNeighbour = true;
                }
            }
        }

        if (!hitNeighbour) //no hit neighbour, so prioritize all four surrounding fields
        {
            for (int i = 0; i < 4; i++)
            {
                if (posX + neighbours[i, 0] < board.GetBoard().GetLength(0) && posY + neighbours[i, 1] < board.GetBoard().GetLength(0)
                    && posX + neighbours[i, 0] >= 0 && posY + neighbours[i, 1] >= 0)
                {
                    if (!board.GetBoard()[posX + neighbours[i, 0], posY + neighbours[i, 1]].firedUpon)
                    {
                        heatMap[posX + neighbours[i, 0], posY + neighbours[i, 1]] += 100;
                        //Debug.Log("no hit heighbours: "+ posX + neighbours[i, 0]+","+ posY + neighbours[i, 1]);
                    }
                }
            }
        }
        else //a neighbour is hit, so prioritise the other direction
        {
            for (int i = 0; i < 4; i++)
            {
                if (posX + neighbours[i, 0] < board.GetBoard().GetLength(0) && posY + neighbours[i, 1] < board.GetBoard().GetLength(0)
                    && posX + neighbours[i, 0] >= 0 && posY + neighbours[i, 1] >= 0)
                {
                    if (board.GetBoard()[posX + neighbours[i, 0], posY + neighbours[i, 1]].firedUpon)
                    {
                        if(posX + (-1) * neighbours[i, 0] >= 0 && posY + (-1) * neighbours[i, 1] >= 0
                            && posX + (-1) * neighbours[i, 0] < board.GetBoard().GetLength(0) && posY + (-1) * neighbours[i, 1] < board.GetBoard().GetLength(0)
                            && !board.GetBoard()[posX + (-1) * neighbours[i, 0], posY + (-1) * neighbours[i, 1]].firedUpon)
                        {
                            heatMap[posX + (-1) * neighbours[i, 0], posY + (-1) * neighbours[i, 1]] += 100;
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
    private void TryToPlaceAllShips(int posX, int posY, Board board, bool vertical)
    {
        foreach (Battleship batship in playerShipsRemaining)
        {
            int length;
            int height;
            if (vertical)
            {
                height = batship.size;
                length = 1;
            }
            else
            {
                height = 1;
                length = batship.size;
            }

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
    private void IncrementHeatMap(int posX, int posY, int length, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                heatMap[x + posX, y + posY]++;
            }
        }
    }






}
