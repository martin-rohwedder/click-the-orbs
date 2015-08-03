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
        StartCoroutine(DoFadeOutScene(sceneName));
    }

    public void QuitGame()
    {
        audioSource.PlayOneShot(clickSFX);
        StartCoroutine(DoFadeOutScene("quit"));
    }

    IEnumerator DoFadeOutScene(string sceneName)
    {
        float fadeTime = GameObject.Find("_ScreenFader").GetComponent<ScreenFader>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime + 1.0f);

        if (sceneName.Equals("quit"))
        {
            Application.Quit();
        }
        else
        {
            Application.LoadLevel(sceneName);
        }
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
