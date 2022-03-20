using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlacer : MonoBehaviour
{
    public GameObject placedShip;
    public List<Battleship> shipsToPlace = new List<Battleship>();
    public int currentShip, length, height;
    public bool vertical;

    private SpriteRenderer spriteRenderer;
    private Transform parent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject shipsParent = new GameObject();
        shipsParent.name = "ShipsParent";
        parent = shipsParent.transform;
    }

    /// <summary>
    /// Lets the Player place their ships. 
    /// </summary>
    /// <param name="battleships"></param>
    public void StartPlacingShips(Battleship[] battleships)
    {
        foreach (Battleship bs in battleships)
        {
            shipsToPlace.Add(bs);
        }
        currentShip = battleships.Length - 1;
        spriteRenderer.sprite = GameManager.instance.GetComponent<SpriteManager>().shipSprite[shipsToPlace[currentShip].size - 2];
        length = battleships[currentShip].size;
        height = 1;
        vertical = false;
    }

    /// <summary>
    /// Is called when a player has placed a ship. Also places a corrosponding sprite for visual confirmation. 
    /// </summary>
    public void ShipPlaced()
    {
        PlaceSprite();

        currentShip--;

        if (currentShip >= 0)
        {
            spriteRenderer.sprite = GameManager.instance.GetComponent<SpriteManager>().shipSprite[shipsToPlace[currentShip].size - 2];
            length = shipsToPlace[currentShip].size;
            height = 1;
        }
        else
        {
            if (GameManager.instance.InEditor)
            {
                TextHandler.UpdatePreviousPlayerPlacement(GameManager.instance.playerBoard);
            }
            
            GameManager.instance.placingShips = false;
            GameManager.instance.StartGame();
        }
    }

    /// <summary>
    /// Is called from ShipPlaced. Places the sprite. 
    /// </summary>
    private void PlaceSprite()
    {
        GameObject shipSprite = Instantiate(placedShip);
        shipSprite.transform.position = transform.position;
        shipSprite.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
        shipSprite.transform.SetParent(parent);
        shipSprite.name = shipsToPlace[currentShip].name;
        if (vertical)
        {
            shipSprite.transform.RotateAround(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(0, 0, 1), 90);
            RotateShip();
        }
    }

    /// <summary>
    /// Allows the player to undo a placed ship. Not yet implemented. 
    /// </summary>
    public void UndoPlacement()
    {
        currentShip++;
    }

    /// <summary>
    /// Moves the visual representation of the to-be-placed ship. 
    /// </summary>
    /// <param name="position"></param>
    public void MoveToField(Vector3 position)
    {
        if (position.x + length <= GameManager.instance.boardSize 
            && position.y + height <= GameManager.instance.boardSize)
        {
            transform.position = vertical ? new Vector3(position.x + 1, position.y, position.z) : position;
        }
    }

    /// <summary>
    /// Check whether the player wants to rotate their ship to be vertical or horisontal. 
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateShip();
        }
    }

    /// <summary>
    /// Rotate the current ship to be vertical or horizontal. 
    /// </summary>
    private void RotateShip()
    {
        vertical = !vertical;

        if (vertical)
        {
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 90);
        }
        else
        {
            transform.RotateAround(new Vector3(transform.position.x + 1, transform.position.y, 0), new Vector3(0, 0, 1), -90);
        }

        int temp = height;
        height = length;
        length = temp;
    }

    /// <summary>
    /// Checks whether the chosen field can actually contain the current ship, and if so, places it there. 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    public void PlaceShip(Board board, int posX, int posY)
    {
        Battleship ship = shipsToPlace[currentShip];
        ship.vertical = vertical;
        int height = vertical ? shipsToPlace[currentShip].size : 1;
        int length = vertical ? 1 : shipsToPlace[currentShip].size;

        ship.x = posX;
        ship.y = posY;

        if (posX + length > board.boardSize
            || posY + height > board.boardSize
            || !Utility.IsValidPlacement(posX, posY, length, height, board))
        {
            return;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Field field = board[posX + x, posY + y];
                field.shipPresent = true;
                field.fieldPartOfShip = ship;
                field.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 55, 0);
            }
        }

        ShipPlaced();
    }


}
