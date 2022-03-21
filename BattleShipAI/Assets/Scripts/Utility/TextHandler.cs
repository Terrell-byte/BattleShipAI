using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Handles remembering previous players' placement of ships by writing and reading from a text file. 
/// </summary>
public class TextHandler : MonoBehaviour
{
    
    //[MenuItem("Tools/Write file")]
    public static void WriteString(int[,] array)
    {
        string path = "";
        if (GameManager.instance.InEditor)
        {
            path = "Assets/PreviousPlayerPlacement.txt";
        }
        else
        {
            return;
            path = Application.persistentDataPath + "/PreviousPlayerPlacement.txt";
        }
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        


        //string path = Application.streamingAssetsPath + "/PreviousPlayerPlacement.txt";
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

        writer.Write(newMap);

        //File.WriteAllText(path, newMap);
        //writer.Close();
        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);

        writer.Close();
    }

    //[MenuItem("Tools/Read file")]
    public static int[,] ReadString()
    {
        int[,] loadedMap = new int[10, 10];
        string path = "";

        if (GameManager.instance.InEditor)
        {
            path = "Assets/PreviousPlayerPlacement.txt";
        }
        else
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    loadedMap[x, y] = 0;
                }
            }
            return loadedMap;
            path = Application.persistentDataPath + "/PreviousPlayerPlacement.txt";
        }
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        
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
