using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    public static int xCount = 9;
    public static int minesCount = 10;

    public void ToSimpleGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
        xCount = 9;
        minesCount = 10;
    }

    public void ToNormalGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
        xCount = 15;
        minesCount = 35;
    }

    public void ToHardGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
        xCount = 15;
        minesCount = 50;
    }

    public void ToSelectLevel()
    {
        SceneManager.LoadSceneAsync("SelectLevelScene");
    }

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public void ToRules()
    {
        SceneManager.LoadSceneAsync("RulesScene");
    }

    public void ToRecords()
    {
        SceneManager.LoadSceneAsync("RecordsScene");
    }
}
