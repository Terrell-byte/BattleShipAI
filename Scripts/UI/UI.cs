using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    /// <summary>
    /// Creates or updates the visual representation of the heatmap
    /// </summary>
    /// <param name="heatmap"></param>
    public static void UpdateHeatMapVisually(int[,] heatmap)
    {
        foreach (Transform child in GameManager.instance.heatMapCanvas.transform)
        {
            Destroy(child.gameObject);
        }

        if (GameManager.instance.showHeatMap)
        {
            for (int y = 0; y < heatmap.GetLength(1); y++)
            {
                for (int x = 0; x < heatmap.GetLength(0); x++)
                {
                    GameObject newText = Instantiate(GameManager.instance.heatmapText, GameManager.instance.heatMapCanvas.transform);
                    newText.GetComponent<Text>().text = heatmap[x, y].ToString();
                }
            }
        }
    }
}
