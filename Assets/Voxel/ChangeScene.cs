using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Loadscene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void GoToMainMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
