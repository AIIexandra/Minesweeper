using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAgain : MonoBehaviour
{

    public void Again()
    {
        SceneManager.LoadScene(0);
    }
}
