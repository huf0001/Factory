using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputController : MonoBehaviour
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

    [SerializeField] private GameController gameController;
    [SerializeField] private Gamepad gamepad = Gamepad.XboxController;
    [SerializeField] private PlayerAndHands[] players;
    private bool triggeredThrow = false;

    // Use this for initialization
    void Start()
    {
        int playerNumber = 0;
        string gamepadPreferece = "";

        for (int i = 0; i < players.Length; i++)
        {
            playerNumber = i + 1;

            if (players[i].Player == null)
            {
                Debug.Log("The input controller is missing player " + playerNumber);
            }

            if (players[i].LeftHand == null)
            {
                Debug.Log("The input controller is missing player " + playerNumber + "'s left hand");
            }

            if (players[i].RightHand == null)
            {
                Debug.Log("The input controller is missing player " + playerNumber + "'s right hand");
            }
        }

        gamepadPreferece = PlayerPrefs.GetString("gamepad");

        if (gamepadPreferece == "dualshock")
        {
            gamepad = Gamepad.DualshockController;
        }
        else
        {
            gamepad = Gamepad.XboxController;
        }
    }

    private void Update()
    {
        if (gameController.Timer <= 10 && !triggeredThrow)
        {
            triggeredThrow = true;
            players[0].Player.GetComponent<HandsController>().TriggerThrow();
            players[1].Player.GetComponent<HandsController>().TriggerThrow();
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

    //Checks if the player clicked the specified button
    public bool GetButtonDown(int p, string s)
    {
        bool result = false;

        switch (s)
        {
            //check which action the player wants
            case "Jump":
            case "Pause":
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
            case "Pause":
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
