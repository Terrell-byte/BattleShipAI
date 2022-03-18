using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextHandler : MonoBehaviour
{

    [MenuItem("Tools/Write file")]
    public static void WriteString(int[,] array)
    {
        string path = "Assets/PreviousPlayerPlacement.txt";
        //Write some text to the test.txt file
        //StreamWriter writer = new StreamWriter(path, true);
        string newMap = "";
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                newMap += Mathf.Clamp(array[x, y],0,9);
            }
            if(y!= 9){
                newMap += "\n";
            }
        }

        File.WriteAllText(path, newMap);
        //writer.Close();
        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
    }

    [MenuItem("Tools/Read file")]
    public static int[,] ReadString()
    {
        string path = "Assets/PreviousPlayerPlacement.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        int[,] loadedMap = new int[10, 10];
        for (int y = 0; y < 10; y++)
        {
            string c = reader.ReadLine();
            for (int x = 0; x < 10; x++)
            {
                loadedMap[x,y] = int.Parse(c.Substring(x,1));
            }
        }

        reader.Close();
        return loadedMap;
    }

    public static void UpdatePreviousPlayerPlacement(Board board)
    {
        int[,] newMap = ReadString();

        for (int y = 0; y < board.boardSize; y++)
        {
            for (int x = 0; x < board.boardSize; x++)
            {
                if(board[x,y].fieldPartOfShip != null)
                {
                    newMap[x, y]++;
                }
            }
        }

        WriteString(newMap);
    }


}
