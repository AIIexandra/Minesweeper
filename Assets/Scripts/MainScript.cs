using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainScript : MonoBehaviour
{ 
    [SerializeField] int xCount = 9;
    [SerializeField] int yCount = 9;
    [SerializeField] int mineCount = 15;
    [SerializeField] GameObject chunckUp;
    //[SerializeField] GameObject panel;

    int[,] grid;

    GameObject[,] gameObjects;

    void Start()
    {
        grid = new int[xCount, yCount];
        gameObjects = new GameObject[xCount, yCount];

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                grid[i, j] = 0;
            }
        }

        int m = mineCount;
        while (m > 0)
        {
            int i = Random.Range(0, xCount);
            int j = Random.Range(0, yCount);

            if(grid[i, j] >= 0)
            {
                grid[i, j] = -10;

                if (i > 0) grid[i - 1, j]++;
                if (j > 0) grid[i, j - 1]++;
                if (i < xCount - 1) grid[i + 1, j]++;
                if (j < yCount - 1) grid[i, j + 1]++;
                if (i > 0 && j > 0) grid[i - 1, j - 1]++;
                if (i > 0 && j < yCount - 1) grid[i - 1, j + 1]++;
                if (i < xCount - 1 && j > 0) grid[i + 1, j - 1]++;
                if (i < xCount - 1 && j < yCount - 1) grid[i + 1, j + 1]++;

                m--;
            }
        }

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                gameObjects[i, j] = Instantiate(chunckUp, Vector3.zero, Quaternion.identity, transform);
                RectTransform rt = gameObjects[i, j].GetComponent<RectTransform>();

                rt.anchorMin = new Vector2(i * 1.0f / xCount, j * 1.0f / yCount);
                rt.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;

                Text chunckText = rt.GetChild(0).GetComponent<Text>();
                if (grid[i, j] == 0) chunckText.text = "";
                if (grid[i, j] < 0) chunckText.text = "*";
                if (grid[i, j] > 0) chunckText.text = grid[i, j].ToString();
                gameObjects[i, j].SetActive(false);
            }
        }
    }

    public void OnPointerDown(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        int i = (int)(pointerEventData.position.x / Screen.width * xCount);
        int j = (int)(pointerEventData.position.y / Screen.height * yCount);
        OpenChunk(i, j);
    }

    void OpenChunk(int i, int j)
    {
        if (gameObjects[i, j].activeSelf) return;
        gameObjects[i, j].SetActive(true);

        if (grid[i, j] == 0)
        {
            if (i > 0) OpenChunk(i - 1, j);
            if (j > 0) OpenChunk(i, j - 1);
            if (i < xCount - 1) OpenChunk(i + 1, j);
            if (j < yCount - 1) OpenChunk(i, j + 1);
            if (i > 0 && j > 0) OpenChunk(i - 1, j - 1);
            if (i > 0 && j < yCount - 1) OpenChunk(i - 1, j + 1);
            if (i < xCount - 1 && j > 0) OpenChunk(i + 1, j - 1);
            if (i < xCount - 1 && j < yCount - 1) OpenChunk(i + 1, j + 1);
        }
    }
}
