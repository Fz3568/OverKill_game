using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void MainMenu()
    {
        LoadScene(0);
       // Cursor.visible = true;
    }
    public void PlayGame()
    {
        LoadScene(1);
      //  Cursor.visible = false;
    }

    public void SettingsMenu()
    {
        LoadScene(2);
      //  Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

    Application.Quit();

#endif
    }

}
