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

    int[,] grid;

    void Start()
    {
        grid = new int[xCount, yCount];

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
                GameObject obj = Instantiate(chunckUp, Vector3.zero, Quaternion.identity, transform);
                RectTransform rt = obj.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(i * 1.0f / xCount, j * 1.0f / yCount);
                rt.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;

                Text chunckText = rt.GetChild(0).GetComponent<Text>();
                if (grid[i, j] == 0) chunckText.text = "";
                if (grid[i, j] < 0) chunckText.text = "*";
                if (grid[i, j] > 0) chunckText.text = grid[i, j].ToString();
            }
        }
    }

    void Update()
    {
        
    }
}
