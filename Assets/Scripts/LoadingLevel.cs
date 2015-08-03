using UnityEngine;
using System.Collections;

public class LoadingLevel : MonoBehaviour {

    public GameObject loadingScreenObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.isLoadingLevel)
        {
            loadingScreenObject.SetActive(true);
        }
        else
        {
            loadingScreenObject.SetActive(false);
        }
	}
}
