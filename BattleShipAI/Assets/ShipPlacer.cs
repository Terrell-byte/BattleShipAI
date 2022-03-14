using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlacer : MonoBehaviour
{
    public int currentShip;
    public GameObject placedShip;
    public bool vertical;
    private SpriteRenderer spriteRenderer;
    public int length, height;
    public List<Battleship> shipsToPlace = new List<Battleship>();
    private Transform parent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject shipsParent = new GameObject();
        shipsParent.name = "ShipsParent";
        parent = shipsParent.transform;
    }

    public void StartPlacingShips(Battleship[] battleships)
    {
        foreach(Battleship bs in battleships)
        {
            shipsToPlace.Add(bs);
        }
        currentShip = battleships.Length - 1;
        spriteRenderer.sprite = GameManager.instance.GetComponent<SpriteManager>().shipSprite[shipsToPlace[currentShip].size-2];
        length = battleships[currentShip].size;
        height = 1;
        vertical = false;
    }

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
            GameManager.instance.placingShips = false;
            GameManager.instance.StartGame();
        }
    }

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

    public void UndoPlacement()
    {
        currentShip++;
    }

    public void MoveToField(Vector3 position)
    {
        if((position.x + length <= GameManager.instance.boardSize && position.y + height <= GameManager.instance.boardSize))
        {
            transform.position = position;

            if (vertical)
            {
                transform.position = new Vector2(transform.position.x+1, transform.position.y);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateShip();
        }
    }

    private void RotateShip()
    {
        vertical = !vertical;

        if (vertical)
        {
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 90);
        }
        else
        {
            transform.RotateAround(new Vector3(transform.position.x+1, transform.position.y, 0), new Vector3(0, 0, 1), -90);
        }

        int temp = height;
        height = length;
        length = temp;
    }

    public void PlaceShip(Board board, int posX, int posY)
    {
        Battleship ship = shipsToPlace[currentShip];
        ship.vertical = vertical;
        int height = 0;
        int length = 0;
        if (vertical)
        {
            length = 1;
            height = shipsToPlace[currentShip].size;
        }
        else
        {
            length = shipsToPlace[currentShip].size;
            height = 1;
        }

        ship.x = posX;
        ship.y = posY;

        if (!(posX + length <= GameManager.instance.boardSize && posY + height <= GameManager.instance.boardSize))
        {
            return;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                board.GetBoard()[posX + x, posY + y].shipPresent = true;
                board.GetBoard()[posX + x, posY + y].fieldPartOfShip = ship;
                board.GetBoard()[posX + x, posY + y].gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 55, 0);
            }
        }

        ShipPlaced();
    }


}
