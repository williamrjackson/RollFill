using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFile : MonoBehaviour
{

    // Update is called once per frame
    public static void SaveLevelToFile(GridSystem.GridElementLevel level)
    {
        string path = Application.dataPath + "/Levels/Level";

        int fileNameCounter = 1;
        while (File.Exists(path + fileNameCounter.ToString() + ".json"))
        {
            fileNameCounter++;
        }
        path = path + fileNameCounter.ToString() + ".json";
        Debug.Log(path);
        
        LevelData data = new LevelData();

        data.columns = level.columns;
        data.rows = level.rows;
        data.UnitCubeCount = level.elements.Count;
        data.unitCubes = new UnitCube[data.UnitCubeCount];
        for (int i = 0; i < data.unitCubes.Length; i++)
        {
            data.unitCubes[i] = new UnitCube(level.elements[i].isWall, level.elements[i].isInvisible);
            if (level.elements[i].isStartPosition)
            {
                data.startPosition = level.elements[i].index;
            }
        }

        string jsonString = JsonUtility.ToJson (data);

        using (StreamWriter streamWriter = File.CreateText (path))
        {
            streamWriter.Write (jsonString);
        }
    }
}

[System.Serializable]
public class LevelData
{
    public int UnitCubeCount;
    public int rows;
    public int columns;
    public int startPosition; 
    public UnitCube[] unitCubes;
}
[System.Serializable]
public class UnitCube
{
    public bool isInvisible = false;
    public bool isWall = false;
    public UnitCube(bool wall, bool invisible)
    {
        isWall = wall;
        isInvisible = invisible;
    }
}
