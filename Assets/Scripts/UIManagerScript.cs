using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

    public void LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
