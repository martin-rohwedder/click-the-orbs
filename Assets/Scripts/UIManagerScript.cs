using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

    public AudioClip clickSFX;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadScene(string sceneName)
    {
        audioSource.PlayOneShot(clickSFX);
        Application.LoadLevel(sceneName);
    }

    public void QuitGame()
    {
        audioSource.PlayOneShot(clickSFX);
        Application.Quit();
    }

    public void DisableBoolInAnimator(Animator anim)
    {
        audioSource.PlayOneShot(clickSFX);
        anim.SetBool("isCreditsPanelVisible", false);
    }

    public void EnableBoolInAnimator(Animator anim)
    {
        audioSource.PlayOneShot(clickSFX);
        anim.SetBool("isCreditsPanelVisible", true);
    }
}
