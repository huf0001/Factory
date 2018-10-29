using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour
{

    [SerializeField] private AudioController audioController;
    [SerializeField] private InputController inputController;
    [SerializeField] private GameObject pauseGameUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private float easytimer = 90;
    [SerializeField] private float mediumtimer = 120;
    [SerializeField] private float hardtimer = 180;
    private float timer = 0;

    private bool countdown = false;
    private bool audioFinished = false;
    private bool finished = false;
    private int difficulty;        
    private int p1BuildCount = 0;
    private int p2BuildCount = 0;
    private bool p1BuiltObjectShowing = false;
    private bool p2BuiltObjectShowing = false;

    void Start()
    {
        //ensures endgameUI doesn't popup on startup
        endGameUI.SetActive(false);
        pauseGameUI.SetActive(false);
        PlayerPrefs.SetString("active", "true");
        PlayerPrefs.SetString("EndGame", "false");

        switch (PlayerPrefs.GetString("difficulty"))
        {
            case "hard":
                difficulty = 4;
                timer = hardtimer;
                break;
            case "medium":
                difficulty = 3;
                timer = mediumtimer;
                break;
            default:
                difficulty = 2;
                timer = easytimer;
                break;
        }
    }

    public void IncrementPlayer1BuildCount()
    {
        if (p2BuildCount < difficulty)
        {
            p1BuildCount += 1;
        }
    }

    public void IncrementPlayer2BuildCount()
    {
        if (p1BuildCount < difficulty)
        {
            p2BuildCount += 1;
        }
    }

    public bool P1BuiltObjectShowing
    {
        set
        {
            p1BuiltObjectShowing = value;
        }
    }

    public bool P2BuiltObjectShowing
    {
        set
        {
            p2BuiltObjectShowing = value;
        }
    }

    public float Timer
    {
        get
        {
            return timer;
        }
    }

    public bool PlayerLost(int p)
    {
        if (timer != 0)
        {
            if (((p == 1) && (p2BuildCount < difficulty)) || ((p == 2) && (p1BuildCount < difficulty)))
            {
                return false;
            }
        }
        else
        {
            if (((p == 1) && (p1BuildCount >= difficulty)) || ((p == 2) && (p2BuildCount >= difficulty)))
            {
                return false;
            }
        }

        return true;
    }

    private void Update()
    {
        CheckPauseMenu();

        if (timer != 0)
        {
            if (PlayerPrefs.GetString("active") != "false")
            {
                timer -= Time.deltaTime;
            
                if ((!countdown) && (timer < 10))
                {
                    audioController.StartCountdown();
                    countdown = true;
                }

                if ((p1BuildCount >= difficulty) || (p2BuildCount >= difficulty) || (timer < 0))
                {
                    timer = 0;
                }
            }
        }
        else
        {
            if (!audioFinished)
            {
                if ((p1BuildCount >= difficulty) || (p2BuildCount >= difficulty))
                {
                    if (countdown)
                    {
                        audioController.StopCountdown();
                        countdown = false;
                    }

                    audioController.Victory();
                }

                audioFinished = true;
            }

            if (!finished)
            {
                if 
                (
                        ((p1BuildCount >= difficulty) && (!p1BuiltObjectShowing))
                    || ((p2BuildCount >= difficulty) && (!p2BuiltObjectShowing))
                    || ((p1BuildCount < difficulty) && (p2BuildCount < difficulty))
                )
                {
                    finished = true;
                    EndRound();
                }
            }
        }
    }

    private void CheckPauseMenu()
    {
        if (PlayerPrefs.GetString("EndGame") != "true")
        {
            if (inputController.GetButtonDown(1, "Pause") || inputController.GetButtonDown(2, "Pause") || CrossPlatformInputManager.GetButtonDown("MKPause"))
            {
                if (PlayerPrefs.GetString("active") == "true")
                {
                    PlayerPrefs.SetString("active", "false");
                    SetScores();
                    pauseGameUI.SetActive(true);
                }
                else
                {
                    PlayerPrefs.SetString("active", "true");
                    pauseGameUI.SetActive(false);
                }
            }
        }
    }

    private void SetScores()
    {
        //sets scores to display when pause / end game screen is enabled
        PlayerPrefs.SetInt("player1score", p1BuildCount);
        PlayerPrefs.SetInt("player2score", p2BuildCount);
    }

    private void EndRound()
    {
        SetScores();

        if (p1BuildCount > p2BuildCount)
        {
            PlayerPrefs.SetString("winner", "player1");
        }
        else if (p2BuildCount == p1BuildCount && p1BuildCount != 0)
        {
            PlayerPrefs.SetString("winner", "tie");
        }
        else if (p1BuildCount < p2BuildCount)
        {
            PlayerPrefs.SetString("winner", "player2");
        }
        else { //if they both got zero
            PlayerPrefs.SetString("winner", "none");
        }

        //enables end game ui
        endGameUI.SetActive(true);
        PlayerPrefs.SetString("active", "false");
        PlayerPrefs.SetString("EndGame", "true");
    }
}