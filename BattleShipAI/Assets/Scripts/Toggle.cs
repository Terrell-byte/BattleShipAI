using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public void ShowHeatmap()
    {
        GameManager.instance.showHeatMap = !GameManager.instance.showHeatMap;
        UI.UpdateHeatMapVisually(GameManager.instance.computer.heatmap.heatmap);
    }

    public void PlayerStarts()
    {
        GameManager.instance.playerTurn = !GameManager.instance.playerTurn;
    }

    public void IntelligentAI()
    {
        GameManager.instance.intelligentAI = !GameManager.instance.intelligentAI;
    }

    public void PredictPlayer()
    {
        GameManager.instance.usePreviousPlacement = !GameManager.instance.usePreviousPlacement;
    }
}
