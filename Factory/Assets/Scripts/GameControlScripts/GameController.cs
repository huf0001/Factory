using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour
{

    [SerializeField] AudioController audioController;
    [SerializeField] private GameObject endGameUi;
    [SerializeField] float timer = 60;

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
        endGameUi.SetActive(false);

        switch (PlayerPrefs.GetString("difficulty"))
        {
            case "hard":
                difficulty = 3;
                break;
            case "medium":
                difficulty = 2;
                break;
            default:
                difficulty = 1;
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
        if (((p == 1) && (p2BuildCount < difficulty)) || ((p == 2) && (p1BuildCount < difficulty)))
        {
            return false;
        }

        return true;
    }

    private void Update()
    {
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            
            if ((!countdown) && (timer < 10))
            {
                audioController.StartCountdown();
                countdown = true;
                Debug.Log("Countdown started");
            }

            if ((p1BuildCount >= difficulty) || (p2BuildCount >= difficulty) || (timer < 0))
            {
                timer = 0;
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
                    SetScoresForEnd();
                }
            }
        }
    }

    private void SetScoresForEnd()
    {
        //sets scores to display when end game screen is enabled
        PlayerPrefs.SetInt("player1score", p1BuildCount);
        PlayerPrefs.SetInt("player2score", p2BuildCount);

        if (p1BuildCount > p2BuildCount)
        {
            PlayerPrefs.SetString("winner", "player1");
        }
        else
        {
            PlayerPrefs.SetString("winner", "player2");
        }

        //enables end game ui
        endGameUi.SetActive(true);
    }
}