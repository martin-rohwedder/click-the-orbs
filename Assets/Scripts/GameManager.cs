using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //States for the actual game
    private enum States {
        START,
        PLAYER_TOUCH,
        LIGHT_ORBS,
        ROUND_SUCCES,
        GAME_OVER
    };

    public List<GameObject> orbs = new List<GameObject>();
    public Text popupText;
    public Text scoreText;
    public Text highscoreText;
    public GameObject GameOverPanel;
    public float orbLightTime = 1.4f;
    public AudioClip positivePlingSFX;
    public AudioClip negativePlingSFX;
    public AudioClip tickSFX;

    private AudioSource audioSource;
    private List<int> colorList = new List<int>();                                                              // 1==Blue, 2==Green, 3==Red, 4==Purple, 5==Yellow
    private States currentState = States.START;
    private string[] succesTextList = { "Well Done!", "Good Job!", "Awesome job!", "Fantastic!", "Incredible!" };
    private int playerTouchIndex = 0;
    private int currentOrbColor = 0;
    private bool isDoingStart = false;
    private bool isDoingOrbs = false;
    private bool isDoingPlayerTouch = false;
    private bool isDoingRoundSucces = false;
    private bool isDoingGameOver = false;
    private int currentScore = 0;
    private int highestScore = 0;

	// Use this for initialization
	void Start () {
        GameOverPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        popupText.text = "";

        int bestScore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = "Highscore: " + bestScore;

        // Init color list with two random colors
        for (int i = 0; i < 2; i++)
        {
            colorList.Add(Random.Range(1, 6));
        }
	}

	// Update is called once per frame
	void Update () {
        if (currentState == States.START) {
            if (!isDoingStart)
            {
                StartCoroutine(DoStartPhase());
            }
        }
        else if (currentState == States.LIGHT_ORBS) {
            if (!isDoingOrbs)
            {
                StartCoroutine(LightOrbs());
            }
        }
        else if (currentState == States.PLAYER_TOUCH) {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isDoingPlayerTouch)
                {
                    DoPlayerTouch();
                }
            }
            else if (Input.touchCount == 1) {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (!isDoingPlayerTouch)
                    {
                        DoPlayerTouch();
                    }
                }
            }
        }
        else if (currentState == States.ROUND_SUCCES)
        {
            if (!isDoingRoundSucces)
            {
                StartCoroutine(DoRoundSucces());
            }
        }
        else if (currentState == States.GAME_OVER)
        {
            if (!isDoingGameOver)
            {
                StartCoroutine(DoGameOver());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        Debug.Log("Current State == " + currentState.ToString());
	}

    public void StartNewGame()
    {
        audioSource.PlayOneShot(tickSFX);
        GameOverPanel.SetActive(false);
        isDoingGameOver = false;
        isDoingPlayerTouch = false;
        popupText.text = "";
        playerTouchIndex = 0;
        currentOrbColor = 0;
        currentScore = 0;
        SetScoreText(0);

        colorList.Clear();

        // Init color list with two random colors
        for (int i = 0; i < 2; i++)
        {
            colorList.Add(Random.Range(1, 6));
        }

        currentState = States.START;
    }

    public void GoToMainMenuScene()
    {
        audioSource.PlayOneShot(tickSFX);
        StartCoroutine(LoadMainScene("MainMenuScene"));
    }

    IEnumerator LoadMainScene(string sceneName)
    {
        float fadeTime = GameObject.Find("_ScreenFader").GetComponent<ScreenFader>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime + 1.0f);
        Application.LoadLevel(sceneName);
    }

    void SetScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator DoStartPhase()
    {
        isDoingStart = true;
        yield return new WaitForSeconds(1.0f);
        popupText.text = "Get Ready!";
        yield return new WaitForSeconds(2.0f);
        audioSource.PlayOneShot(tickSFX);
        popupText.text = "3";
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(tickSFX);
        popupText.text = "2";
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(tickSFX);
        popupText.text = "1";
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(tickSFX);
        popupText.text = "Go!";
        yield return new WaitForSeconds(1.0f);

        isDoingStart = false;
        currentState = States.LIGHT_ORBS;

        yield return null;
    }

    IEnumerator DoGameOver()
    {
        isDoingGameOver = true;
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(negativePlingSFX);
        popupText.text = "Game Over!";
        GameOverPanel.SetActive(true);

        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            highscoreText.text = "Highscore: " + highestScore;

            SaveScoreToDevice();
        }

        yield return null;
    }

    void SaveScoreToDevice()
    {
        PlayerPrefs.SetInt("Highscore", highestScore);
        PlayerPrefs.Save();
    }

    IEnumerator DoRoundSucces()
    {
        isDoingRoundSucces = true;

        yield return new WaitForSeconds(orbLightTime + 0.5f);

        currentScore++;
        SetScoreText(currentScore);

        audioSource.PlayOneShot(positivePlingSFX);
        popupText.text = succesTextList[Random.Range(0, succesTextList.Length)];

        colorList.Add(Random.Range(1, 6));                      //Add a new color to the color list
        yield return new WaitForSeconds(2.5f);

        isDoingRoundSucces = false;
        currentState = States.LIGHT_ORBS;

        yield return null;
    }

    void DoPlayerTouch()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            isDoingPlayerTouch = true;

            if (hit.transform.name == "Orb_blue_dark") { currentOrbColor = 1; }
            else if (hit.transform.name == "Orb_green_dark") { currentOrbColor = 2; }
            else if (hit.transform.name == "Orb_red_dark") { currentOrbColor = 3; }
            else if (hit.transform.name == "Orb_purple_dark") { currentOrbColor = 4; }
            else if (hit.transform.name == "Orb_yellow_dark") { currentOrbColor = 5; }

            if (colorList[playerTouchIndex] == currentOrbColor)
            {
                // Color == Blue
                if (colorList[playerTouchIndex] == 1)
                {
                    StartCoroutine(LightUpOrb(1));
                }
                // Color == Green
                else if (colorList[playerTouchIndex] == 2)
                {
                    StartCoroutine(LightUpOrb(3));
                }
                // Color == Red
                else if (colorList[playerTouchIndex] == 3)
                {
                    StartCoroutine(LightUpOrb(5));
                }
                // Color == Purple
                else if (colorList[playerTouchIndex] == 4)
                {
                    StartCoroutine(LightUpOrb(7));
                }
                // Color == Yellow
                else if (colorList[playerTouchIndex] == 5)
                {
                    StartCoroutine(LightUpOrb(9));
                }
            }
            else
            {
                Debug.Log("Game Over!!!");
                currentState = States.GAME_OVER;
            }

            if (playerTouchIndex == colorList.Count - 1)
            {
                playerTouchIndex = 0;
                currentOrbColor = 0;

                if (currentState != States.GAME_OVER)
                {
                    currentState = States.ROUND_SUCCES;
                }
            }
            else
            {
                playerTouchIndex++;
            }
        }
    }

    IEnumerator LightUpOrb(int orbIndex)
    {
        orbs[orbIndex].SetActive(true);
        yield return new WaitForSeconds(orbLightTime);
        orbs[orbIndex].SetActive(false);
        isDoingPlayerTouch = false;

        yield return null;
    }

    //Light up the orbs, for the player to see which order he has to touch the orbs.
    IEnumerator LightOrbs()
    {
        isDoingOrbs = true;

        popupText.text = "";

        foreach (int color in colorList)
        {
            yield return new WaitForSeconds(orbLightTime);

            // Color == Blue
            if (color == 1)
            {
                orbs[1].SetActive(true);
                yield return new WaitForSeconds(orbLightTime);
                orbs[1].SetActive(false);
            }
            // Color == Green
            else if (color == 2)
            {
                orbs[3].SetActive(true);
                yield return new WaitForSeconds(orbLightTime);
                orbs[3].SetActive(false);
            }
            // Color == Red
            else if (color == 3)
            {
                orbs[5].SetActive(true);
                yield return new WaitForSeconds(orbLightTime);
                orbs[5].SetActive(false);
            }
            // Color == Purple
            else if (color == 4)
            {
                orbs[7].SetActive(true);
                yield return new WaitForSeconds(orbLightTime);
                orbs[7].SetActive(false);
            }
            // Color == Yellow
            else if (color == 5)
            {
                orbs[9].SetActive(true);
                yield return new WaitForSeconds(orbLightTime);
                orbs[9].SetActive(false);
            }
        }

        yield return new WaitForSeconds(0.5f);
        popupText.text = "Your turn!";
        yield return new WaitForSeconds(1.7f);
        popupText.text = "";

        isDoingOrbs = false;
        currentState = States.PLAYER_TOUCH;

        yield return null;
    }
}
