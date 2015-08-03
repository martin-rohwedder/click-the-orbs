using UnityEngine;
using System.Collections;
/**
 * This script will play a fade in and out intro with the Rohwedder Games Logo
 * 
 * @author Martin Rohwedder
 * @version 1.0
 */
public class LogoIntro : MonoBehaviour
{
    public string nextSceneName;

    private AudioSource[] allAudioSources;

    // Use this for initialization
    void Awake()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        StartCoroutine(FadeInAndOut(GetComponent<SpriteRenderer>()));
    }

    // Updated every frame
    void Update()
    {
        if (Application.isMobilePlatform)
        {
            //Skip the logo intro on screen touch
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //Load the Main Scene
                    StopAllAudio();
                    StartCoroutine(LoadMainScene());
                }
            }
        }
        else
        {
            //Skip the logo intro on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                //Load the Main Scene
                StopAllAudio();
                StartCoroutine(LoadMainScene());
            }
        }
    }

    // Fade the sprite in and out
    private IEnumerator FadeInAndOut(SpriteRenderer sprite)
    {
        float duration = 3f; //3 seconds
        float currentTime = 0f;

        // Alpha values: Fade In : from 0 to 1
        float oldAlpha = 0.0f;
        float finalAlpha = 1.0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(oldAlpha, finalAlpha, currentTime / duration);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(4.0f);

        currentTime = 0f;

        // Alpha values: Fade Out : from 1 to 0
        oldAlpha = 1.0f;
        finalAlpha = 0.0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(oldAlpha, finalAlpha, currentTime / duration);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

            currentTime += Time.deltaTime;
            yield return null;
        }

        //Load the Main Scene
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(LoadMainScene());

        yield break;
    }

    void StopAllAudio()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }

    IEnumerator LoadMainScene()
    {
        float fadeTime = GameObject.Find("_ScreenFader").GetComponent<ScreenFader>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime + 1.0f);
        Application.LoadLevel(nextSceneName);
    }
}