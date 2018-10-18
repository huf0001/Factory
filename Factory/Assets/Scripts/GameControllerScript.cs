using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameControllerScript : MonoBehaviour
{
    [System.Serializable]
    public class PlayerAndHands
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;

        public GameObject Player
        {
            get
            {
                return player;
            }
        }

        public GameObject LeftHand
        {
            get
            {
                return leftHand;
            }
        }

        public GameObject RightHand
        {
            get
            {
                return rightHand;
            }
        }
    }

    [SerializeField] AudioController audioController;
    [SerializeField] private Gamepad gamepad = Gamepad.XboxController;
    [SerializeField] private PlayerAndHands[] players;
    /*[SerializeField]*/ int difficulty;
    [SerializeField] float timer = 60;
    private int player1BuildCount = 0;
    private int player2BuildCount = 0;
    private bool finished = false;
    private bool triggeredThrow = false;
    [SerializeField] private GameObject endGameUi;

    // Use this for initialization
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


        int playerNumber = 0;

        for(int i = 0; i < players.Length; i++)
        {
            playerNumber = i + 1;

            if (players[i].Player == null)
            {
                Debug.Log("The game controller is missing player " + playerNumber);
            }

            if (players[i].LeftHand == null)
            {
                Debug.Log("The game controller is missing player " + playerNumber + "'s left hand");
            }

            if (players[i].RightHand == null)
            {
                Debug.Log("The game controller is missing player " + playerNumber + "'s right hand");
            }
        }
    }

    public GameObject LeftHand(int player)
    {
        return players[player - 1].LeftHand;
    }


    public GameObject RightHand(int player)
    {

        return players[player - 1].RightHand;
    }

    public Gamepad Gamepad
    {
        get
        {
            return gamepad;
        }
    }

    /*public int Difficulty
    {
        get
        {
            return difficulty;
        }
    }*/

    public int PlayerCount
    {
        get
        {
            return players.Length;
        }
    }

    public int GetPlayerNumber(GameObject player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Player == player)
            {
                return i + 1;
            }
        }

        return 0;
    }

    public void IncrementPlayer1BuildCount()
    {
        player1BuildCount += 1;
    }

    public void IncrementPlayer2BuildCount()
    {
        player2BuildCount += 1;
    }

    private void Update()
    {
        if (!finished)
        {
            if (player1BuildCount >= difficulty)
            {
                finished = true;
                audioController.EndRound();
                SetScoresForEnd();
            }
            else if (player2BuildCount >= difficulty)
            {
                finished = true;
                audioController.EndRound();
                SetScoresForEnd();
            }
            else if (timer <= 10 && !triggeredThrow)
            {
                triggeredThrow = true;
                players[0].Player.GetComponent<PickUpScript>().TriggerThrow();
                players[1].Player.GetComponent<PickUpScript>().TriggerThrow();
                audioController.StartCountdown();
            }
            else if (timer <= 0)
            {
                finished = true;
                Debug.Log("Time's up!");
                audioController.EndRound();
                SetScoresForEnd();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void SetScoresForEnd()
    {
        //sets scores to display when end game screen is enabled
        PlayerPrefs.SetInt("player1score", player1BuildCount);
        PlayerPrefs.SetInt("player2score", player2BuildCount);

        if (player1BuildCount > player2BuildCount)
        {
            PlayerPrefs.SetString("winner", "player1");
        }
        else { PlayerPrefs.SetString("winner", "player1"); }

        //enables end game ui
        endGameUi.SetActive(true);
    }
    //Checks if the player clicked the specified button
    public bool GetButtonDown(int p, string s)
    {
        bool result = false;

        switch (s)
        {
            //check which action the player wants
            case "Jump":
                result = CrossPlatformInputManager.GetButtonDown("P" + p + GetGamepadPrefix() + s);
                break;
            case "LeftArm":
            case "RightArm":
            case "LookHorizontal":
                if (GetAxis(p, s) != 0)
                {
                    result = true;
                }

                break;
        }

        return result;
    }

    //Checks if the player is holding the specified button down
    public bool GetButton(int p, string s)
    {
        bool result = false;

        switch (s)
        {
            //check which action the player wants
            case "Jump":
                result = CrossPlatformInputManager.GetButton("P" + p + GetGamepadPrefix() + s);
                break;
            case "LeftArm":
            case "RightArm":
            case "LookHorizontal":
                if (GetAxis(p, s) != 0)
                {
                    result = true;
                }

                break;
        }

        return result;
    }

    public float GetAxis(int p, string s)
    {
        float result = 0f;

        //check which action the player wants
        switch (s)
        {
            case "MoveHorizontal":
            case "MoveVertical":
            case "LookHorizontal":
            case "LeftArm":
            case "RightArm":
                result = CrossPlatformInputManager.GetAxis("P" + p + GetGamepadPrefix() + s);
                break;
        }

        return result;
    }

    //Returns the matching input prefix for the gamepad selected by the player
    private string GetGamepadPrefix()
    {
        string result = "";

        switch (gamepad)
        {
            case Gamepad.XboxController:
                result = "XB";
                break;
            case Gamepad.DualshockController:
                result = "DS";
                break;
            default:
                result = "MK";
                break;
        }

        return result;
    }
}