using UnityEngine;
using System.Collections;

public class CameraSettings : MonoBehaviour {

    public Camera mainCamera;
	
	void Awake () {
        mainCamera.aspect = 4f / 3f;
	}
}
