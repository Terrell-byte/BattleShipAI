using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlacer : MonoBehaviour
{
    public int currentShip;
    private bool vertical;
    private SpriteRenderer spriteRenderer;
    private int length, height;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartPlacingShips()
    {
        spriteRenderer.sprite = GameManager.instance.GetComponent<SpriteManager>().shipSprite[currentShip];
        length = currentShip+2;
        height = 1;
        vertical = false;
    }

    public void PlaceShip(Battleship shipToBePlaced)
    {
        currentShip++;
    }

    public void MoveToField(Vector3 position)
    {
        if((position.x + length <= GameManager.instance.boardSize && position.y + height <= GameManager.instance.boardSize))
        {
            transform.position = position;
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vertical = !vertical;

            if (vertical)
            {
                transform.RotateAround(new Vector3(0,0,0),new Vector3(0,0,1),90);
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
            else
            {
                transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1), -90);
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }

            int temp = height;
            height = length;
            length = temp;
        }
    }


}
