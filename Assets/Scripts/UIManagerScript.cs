using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartNewGame()
    {
        Application.LoadLevel("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
