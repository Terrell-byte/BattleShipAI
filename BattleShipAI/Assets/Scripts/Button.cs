using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.instance.RestartGame();
    }

    public void PlayerIsReadyToStart()
    {
        GameManager.instance.PlayerIsReady();
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit(); //ignored in editor
    }
}
