using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SerializeJson : MonoBehaviour
{
    public static int lastLoadedLevel = 0;

    // write the current editor view to level 0
    public static void SerializeEditToFile()
    {
        // Bail if there's no edit view in this scene
        EditorView editor = FindObjectOfType<EditorView>();
        if (editor == null) return;

        string path = Application.dataPath + "/Levels/Level0.json";
        Debug.Log(path);

        LevelData data = new LevelData();

        // convert the editor cells into the json serializable LevelData
        data.columns = editor.effectiveXDimensions;
        data.rows = editor.effectiveYDimensions;
        data.unitCubes = new UnitCube[editor.buttonList.Count];
        for (int i = 0; i < data.unitCubes.Length; i++)
        {
            if (editor.buttonList[i].State == EditButtonState.Invisible)
            {
                data.unitCubes[i] = new UnitCube(true, true);
            }
            else if (editor.buttonList[i].State == EditButtonState.Wall)
            {
                data.unitCubes[i] = new UnitCube(true, false);
            }
            else if (editor.buttonList[i].State == EditButtonState.Floor)
            {
                data.unitCubes[i] = new UnitCube(false, false);
            }

            if (editor.buttonList[i].State == EditButtonState.Start)
            {
                data.startPosition = i;
            }
        }

        string jsonString = JsonUtility.ToJson(data);

        // Write the file
        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
    }

    // Return level 0 content as a LevelData object.
    public static LevelData EditLevel
    {
        get
        {
            string filePath = Application.dataPath + "/Levels/Level0.json";
            if (File.Exists(filePath))
            {
                Debug.Log("Loading " + filePath);
                return JsonUtility.FromJson<LevelData>(File.ReadAllText(filePath));
            }
            return null;
        }
    }

    // Check the existence of a specific level by index
    public static bool LevelExists(int levelIndex)
    {
        string path = Application.dataPath + "/Levels/Level";
        return File.Exists(path + levelIndex.ToString() + ".json");
    }

    // turn the json into an actual level
    public static GridSystem.GridElementLevel DeserializeLevelfile(int levelIndex)
    {
        GridSystem.GridElementLevel loadedLevel = new GridSystem.GridElementLevel();
        string filePath = Application.dataPath + "/Levels/Level" + levelIndex.ToString() + ".json";
        if (File.Exists(filePath))
        {
            Debug.Log("Loading " + filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(filePath));
            loadedLevel.columns = levelData.columns;
            loadedLevel.rows = levelData.rows;
            loadedLevel.elements = new List<Element>();
            for (int i = 0; i < levelData.unitCubes.Length; i++)
            {
                Element element = Instantiate(GridSystem.Instance.UnitCube);
                element.isWall = levelData.unitCubes[i].isWall;
                element.isInvisible = levelData.unitCubes[i].isInvisible;
                element.index = i;
                loadedLevel.elements.Add(element);
            }
            loadedLevel.elements[levelData.startPosition].isStartPosition = true;
            lastLoadedLevel = levelIndex;
        }
        return loadedLevel;
    }

    // Return the next level if it exists
    public static GridSystem.GridElementLevel NextLevel()
    {
        int nextLevelIndex = lastLoadedLevel + 1;
        // Write the level index to prefs, to persist it
        if (LevelExists(nextLevelIndex))
        {
            PlayerPrefs.SetInt("Level", nextLevelIndex);
            return DeserializeLevelfile(nextLevelIndex);
        }
        return null;
    }
}

[System.Serializable]
public class LevelData
{
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
