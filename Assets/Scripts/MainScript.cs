using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{ 
    [SerializeField] int xCount = 9;
    [SerializeField] int yCount = 9;
    [SerializeField] int mineCount = 15;
    [SerializeField] GameObject chunkUp;
    [SerializeField] GameObject chunkDown;
    
    int[,] grid;
    GameObject[,] chunksUp;
    GameObject[,] chunksDown;

    //размеры панели
    [SerializeField] RectTransform rectTransformPanel;
    [SerializeField] int margin = 10;
    float widthPanel;
    float heightPanel;
    float topMargin;
    float leftMardin;

    //обработка длительного нажатия
    bool pressing = false;
    float clickTime = 0.3f;
    float totalClickTime = 0;
    //координаты нажатия
    int iClick;
    int jClick;

    void Start()
    {
        //размеры панели
        //rectTransformPanel = panel.GetComponent<RectTransform>();

        topMargin = (Screen.height - Screen.width) * 0.5f + margin;
        leftMardin = margin;
        heightPanel = Screen.height - topMargin * 2;
        widthPanel = Screen.width - leftMardin * 2;

        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0.0f, heightPanel);
        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0.0f, heightPanel);
        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, widthPanel);
        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0.0f, widthPanel);

        rectTransformPanel.localPosition = new Vector2(0, 0);

        //генерация ячеек
        grid = new int[xCount, yCount];
        chunksDown = new GameObject[xCount, yCount];
        chunksUp = new GameObject[xCount, yCount];

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
                chunksDown[i, j] = Instantiate(chunkDown, Vector3.zero, Quaternion.identity, transform);
                RectTransform rt = chunksDown[i, j].GetComponent<RectTransform>();

                rt.anchorMin = new Vector2(i * 1.0f / xCount, j * 1.0f / yCount);
                rt.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;

                Text chunckText = rt.GetChild(0).GetComponent<Text>();
                Image mine = rt.GetChild(1).GetComponent<Image>();

                if (grid[i, j] == 0) 
                { 
                    chunckText.text = "";
                    mine.enabled = false;
                }

                if (grid[i, j] < 0)
                {
                    chunckText.text = "";
                    mine.enabled = true;
                }

                if (grid[i, j] > 0)
                {
                    chunckText.text = grid[i, j].ToString();
                    mine.enabled = false;
                }

                chunksUp[i, j] = Instantiate(chunkUp, Vector3.zero, Quaternion.identity, transform);
                rt = chunksUp[i, j].GetComponent<RectTransform>();

                rt.anchorMin = new Vector2(i * 1.0f / xCount, j * 1.0f / yCount);
                rt.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;

                Image flag = rt.GetChild(0).GetComponent<Image>();
                Image question = rt.GetChild(1).GetComponent<Image>();
                flag.enabled = false;
                question.enabled = false;
            }
        }
    }

    void OpenChunk(int i, int j)
    {
        if (chunksUp[i, j].activeSelf)
        {
            chunksUp[i, j].SetActive(false);

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

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        int i = (int)((pointerEventData.position.x / widthPanel * xCount));
        int j = (int)((pointerEventData.position.y - topMargin) / heightPanel * yCount);

        if (pressing)
        {           
            OpenChunk(i, j);
            Debug.Log("[" + i + ", " + j + "]");
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        iClick = (int)((pointerEventData.position.x / widthPanel * xCount));
        jClick = (int)((pointerEventData.position.y - topMargin) / heightPanel * yCount);

        pressing = true;
        totalClickTime = 0;
    }

    void Update()
    {
        if (pressing)
        {
            totalClickTime += Time.deltaTime;

            if (totalClickTime >= clickTime)
            {
                pressing = false;

                RectTransform rt = chunksUp[iClick, jClick].GetComponent<RectTransform>();
                Image flag = rt.GetChild(0).GetComponent<Image>();
                Image question = rt.GetChild(1).GetComponent<Image>();

                if (!flag.enabled && !question.enabled)
                {
                    flag.enabled = true;
                }

                else if (flag.enabled)
                {
                    flag.enabled = false;
                    question.enabled = true;
                }
                else
                {
                    flag.enabled = false;
                    question.enabled = false;
                }
            }
        }
    }
}
