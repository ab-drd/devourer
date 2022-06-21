using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
    }

    public void StartGameOnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(1);
    }
}
