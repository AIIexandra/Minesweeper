using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    int endGame = 0;    //1 - проигрыш, 2 - выигрыш

    [SerializeField] int xCount = 9;
    [SerializeField] int yCount = 9;
    [SerializeField] int minesCount = 15;
    [SerializeField] GameObject chunkUp;
    [SerializeField] GameObject chunkDown;

    int[,] grid;
    GameObject[,] chunksUp;
    GameObject[,] chunksDown;

    int mineCorrectCount = 0;

    //Таймер
    [SerializeField] Text textMineRemCount;
    int mineRemCount;
    [SerializeField] Text textTime;
    float time;
    public static bool timeRun = true;

    //размеры панели
    [SerializeField] GameObject panelLoss;
    [SerializeField] GameObject panelWin;
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

    //[SerializeField] int vibroTime = 200;

    //координаты нажатия
    int iClick;
    int jClick;

    void Start()
    {
        //запуск таймера
        timeRun = true;
        textTime.text = Mathf.RoundToInt(time).ToString();

        //сложность уровня
        xCount = yCount = SceneSwitching.xCount;
        minesCount = SceneSwitching.minesCount;

        //UI количество мин
        mineRemCount = SceneSwitching.minesCount;
        textMineRemCount.text = mineRemCount.ToString();

        //Отключить панели выигрыша и проигрыша
        panelLoss.SetActive(false);
        panelWin.SetActive(false);

        //размеры панели
        topMargin = (Screen.height - Screen.width) * 0.5f + margin;
        leftMardin = margin;
        heightPanel = Screen.height - topMargin * 2;
        widthPanel = Screen.width - leftMardin * 2;

        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0.0f, heightPanel);
        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0.0f, heightPanel);
        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, widthPanel);
        rectTransformPanel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0.0f, widthPanel);

        rectTransformPanel.localPosition = new Vector2(0, 0);

        //создание поля
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

        //установка мин и цифр
        int m = minesCount;
        while (m > 0)
        {
            int i = Random.Range(0, xCount);
            int j = Random.Range(0, yCount);

            if (grid[i, j] >= 0)
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

        //генерация ячеек
        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                chunksDown[i, j] = Instantiate(chunkDown, Vector3.zero, Quaternion.identity, transform);
                RectTransform rt = chunksDown[i, j].GetComponent<RectTransform>();

                rt.anchorMin = new Vector2((i * 1.0f / xCount), j * 1.0f / yCount);
                rt.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;

                Text chunckText = rt.GetChild(0).GetComponent<Text>();
                Image mine = rt.GetChild(1).GetComponent<Image>();

                if (grid[i, j] == 0)   //пусто
                {
                    chunckText.text = "";
                    mine.enabled = false;
                }

                if (grid[i, j] < 0)  //мина
                {
                    chunckText.text = "";
                    mine.enabled = true;
                }

                if (grid[i, j] > 0)    //цифра
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

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Получить координаты клика
        iClick = (int)((pointerEventData.position.x / widthPanel * xCount));
        jClick = (int)((pointerEventData.position.y - topMargin) / heightPanel * yCount);

        //Активировать длительное нажатие
        pressing = true;
        totalClickTime = 0;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //если при отпускании не было деактивировано длительное нажатие, значит оно обычное
        if (pressing)
        {
            OpenChunk(iClick, jClick);
            if (grid[iClick, jClick] < 0)
            {
                endGame = 1;
                Finish();
            }
        }
    }

    void Update()
    {
        if (timeRun)
        {
            time += Time.deltaTime;
            textTime.text = Mathf.RoundToInt(time).ToString();
        }

        if (pressing)
        {
            totalClickTime += Time.deltaTime;

            if (totalClickTime >= clickTime && chunksUp[iClick, jClick].activeSelf)
            {
                pressing = false;   //деактивация длительного нажатия

                RectTransform rt = chunksUp[iClick, jClick].GetComponent<RectTransform>();
                Image flag = rt.GetChild(0).GetComponent<Image>();
                Image question = rt.GetChild(1).GetComponent<Image>();

                if (!flag.enabled && !question.enabled)    //Поставить флаг, уменьшить количество мин
                {
                    flag.enabled = true;
                    mineRemCount--;
                    textMineRemCount.text = mineRemCount.ToString();

                    //Handheld.Vibrate();

                    if (grid[iClick, jClick] < 0)
                    {
                        mineCorrectCount++;

                        if (mineCorrectCount == minesCount)
                        {
                            endGame = 2;
                            Finish();
                        }
                    }
                }
                 
                else if (flag.enabled)   //Убрать флаг, поставить вопрос, увелисить количество мин
                {
                    flag.enabled = false;
                    question.enabled = true;
                    mineRemCount++;
                    textMineRemCount.text = mineRemCount.ToString();

                    //Handheld.Vibrate();

                    if (grid[iClick, jClick] < 0)
                    {
                        mineCorrectCount--;
                    }
                }
                else   //Убрать вопрос
                {
                    flag.enabled = false;
                    question.enabled = false;

                    //Handheld.Vibrate();
                }
            }
        }
    }

    void Finish()
    {
        timeRun = false;

        if (endGame == 2)   //выигрыш
        {
            panelWin.SetActive(true);

            if (SceneSwitching.minesCount == 10)
            {
                PlayerPrefs.SetString("SimpleRecord", time.ToString("F2"));
            }
            else if (SceneSwitching.minesCount == 35)
            {
                PlayerPrefs.SetString("NormalRecord", time.ToString("F2"));
            }
            else if (SceneSwitching.minesCount == 50)
            {
                PlayerPrefs.SetString("HardRecord", time.ToString("F2"));
            }
        }

        else if (endGame == 1)  //проигрыш
        {
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    if (grid[i, j] < 0)
                        chunksUp[i, j].SetActive(false);
                }
            }

            panelLoss.SetActive(true);
            Debug.Log("Loss");
        }
    }
}
