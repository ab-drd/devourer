using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void ExitGameOnClick()
    {
        Debug.Log("Exited game");
        Application.Quit();
    }
}
