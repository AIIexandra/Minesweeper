using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    public void ToGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
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

    public void ToSituations()
    {
        SceneManager.LoadSceneAsync("SituationsScene");
    }
}
