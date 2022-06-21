using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenu : MonoBehaviour
{
    public void ExitOnClick()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
