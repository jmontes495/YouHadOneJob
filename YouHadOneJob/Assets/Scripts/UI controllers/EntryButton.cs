using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryButton : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("MainScreen");
    }
}
