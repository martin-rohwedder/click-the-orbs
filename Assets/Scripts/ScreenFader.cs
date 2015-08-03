using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{

    public Texture2D fadeOutTexture;
    public bool fadeInOnLevelLoaded = true;
    public float fadeTimeMultiplier = 0.5f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
    private float fadeSpeed = 0.8f;

    void Awake()
    {
        if (fadeInOnLevelLoaded)
        {
            alpha = 1.0f;
        }
        else
        {
            alpha = 0.0f;
        }
    }

    void OnGUI()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime * fadeTimeMultiplier;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        yield return new WaitForSeconds(1.0f);
        yield break;
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }

}
