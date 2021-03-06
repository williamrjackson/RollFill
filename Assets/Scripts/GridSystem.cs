﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public Element UnitCube;
    [SerializeField]
    int initRowCount;
    
    [SerializeField]
    int initColumnCount;
    [SerializeField]
    GameObject editorContainer; 
    [SerializeField]
    private bool bypassPrefsLevel;
    


    GridElementLevel loadedLevel;

    public static GridSystem Instance;
    public enum Direction {Up, Down, Left, Right};

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    void Start()
    {
        int levelIndex = (bypassPrefsLevel) ? 1 : PlayerPrefs.GetInt("Level");
        if (initRowCount != 0 && initColumnCount != 0)
        {
            GridElementLevel level = new GridElementLevel();
            level.columns = initColumnCount;
            level.rows = initRowCount;
            level.elements = new List<Element>();
            for (int i = 0; i < level.rows * level.columns; i++)
            {
                Element newElement = Instantiate(UnitCube);
                level.elements.Add(newElement);
            }
            SetGrid(level);
        }
        else if (SerializeJson.LevelExists(levelIndex))
        {
            LoadLevel(levelIndex);
            if (editorContainer != null)
                editorContainer.SetActive(false);
        }
    }

    public void LoadLevel(int number)
    {
        if (loadedLevel != null)
            loadedLevel.Clear();
        
        SetGrid(SerializeJson.DeserializeLevelfile(number));
    }

    public void CheckForWin()
    {
        if (GameManager.Instance.IsComplete(loadedLevel))
        {
            GameManager.Instance.DisplaySuccess();
            Wrj.Utils.Delay(2f, () => WinLevel());
        }
    }
    private void WinLevel()
    {
        Debug.Log("WIN!");
        loadedLevel.Clear();
        SetGrid(SerializeJson.NextLevel());
    }

    public List<Element> GetUnitsFromPosition (int position, Direction dir)
    {
        List<Element> returnList = new List<Element>();
        if (dir == Direction.Right)
        {
            int edge = (loadedLevel.columns * (position / loadedLevel.columns)) + loadedLevel.columns;
            int index = position;
            while (index < edge && !loadedLevel.elements[index].isWall)
            {
                returnList.Add(loadedLevel.elements[index]);
                index++;
            }
        }
        else if (dir == Direction.Left)
        {
            int edge = (loadedLevel.columns * (position / loadedLevel.columns)) - 1;
            int index = position;
            while (index > edge && !loadedLevel.elements[index].isWall)
            {
                returnList.Add(loadedLevel.elements[index]);
                index--;
            }
        }
        else if (dir == Direction.Up)
        {
            int index = position;
            while (index >= 0 && !loadedLevel.elements[index].isWall)
            {
                returnList.Add(loadedLevel.elements[index]);
                index -= loadedLevel.columns;
            }
        }
        else if (dir == Direction.Down)
        {
            int index = position;
            while (index < loadedLevel.elements.Count && !loadedLevel.elements[index].isWall)
            {
                returnList.Add(loadedLevel.elements[index]);
                index += loadedLevel.columns;
            }
        }
        return returnList;
    }

    public void ResetGrid()
    {
        SetGrid(loadedLevel);
    }

    public void SetGrid(GridElementLevel level)
    {
        if (level == null)
            return;

        Player.Instance.transform.parent = transform;
        if (level.columns * level.rows != level.elements.Count)
        {
            Debug.LogError("Corrupted Level");
            return;
        }
        loadedLevel = level;
        for (int i = 0; i < level.rows; i++)
        {
            for (int j = 0; j < level.columns; j++)
            {
                int elementIndex = (i * level.columns) + j;
                Element element = level.elements[elementIndex];
                element.index = elementIndex;
                element.transform.parent = transform;
                element.transform.localPosition = new Vector3(j, 0, -i);

                if (element.isWall)
                {
                    element.transform.localPosition = element.transform.localPosition.With(y:.125f);
                    element.transform.Scale(element.transform.localScale.With(y:1.25f), 0f);
                    element.transform.Color(Color.gray, 0f);
                }
                else
                {
                    element.transform.localPosition = element.transform.localPosition.With(y:0f);
                    element.transform.Scale(element.transform.localScale.With(y:1f), 0f);
                    element.transform.Color(Color.white, 0f);
                }

                if (element.isInvisible)
                {
                    element.transform.Alpha(0f, 0f);
                    element.isWall = true;
                }
                else
                {
                    element.transform.Alpha(1f, 0f);
                }

                if (element.isStartPosition)
                {
                    element.SetCollected();
                    Player.Instance.SetCurrentPos(element.index);
                    Player.Instance.transform.localPosition = element.transform.localPosition.With(y:1f);
                }
            }
        }
        transform.position = new Vector3(-(level.columns * .5f) + .5f, transform.position.y, (level.rows / 2) -.5f);
        CameraMan.Instance.MoveCameraToFit(Mathf.Max(loadedLevel.columns, loadedLevel.rows));
        GameManager.Instance.DisplayCurrentLevel();
    }

    public void SaveCurrentLevel()
    {
        SerializeJson.SerializeLevelToFile(loadedLevel);
    }

    public class GridElementLevel
    {
        public int columns;
        public int rows;
        public List<Element> elements;

        public void Clear()
        {
            columns = 0;
            rows = 0;
            foreach (Element element in elements)
            {
                Destroy(element.gameObject);
            }
            elements.Clear();
        }
    }
}
