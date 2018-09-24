using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private Gamepad gamepad = Gamepad.MouseAndKeyboard;
    [SerializeField] private BuildZoneScript buildZone = null;
    [SerializeField] private GameObject playerLeftHand = null;
    [SerializeField] private GameObject playerRightHand = null;

    // Use this for initialization
    void Start()
    {
        if (playerLeftHand == null)
        {
            Debug.Log("The game controller is missing the player's left hand");
        }

        if (playerRightHand == null)
        {
            Debug.Log("The game controller is missing the player's right hand");
        }

        if (buildZone == null)
        {
            Debug.Log("The game controller is missing the build zone");
        }
    }

    public GameObject LeftHand
    {
        get
        {
            return playerLeftHand;
        }
    }

    public GameObject RightHand
    {
        get
        {
            return playerRightHand;
        }
    }

    public BuildZoneScript BuildZone
    {
        get
        {
            return buildZone;
        }
    }

    public bool GetInput(string s)
    {
        bool result = false;

        switch (gamepad)
        {
            case Gamepad.XboxController:
                result = GetXboxControllerInput(s);
                break;
            case Gamepad.DualshockController:
                result = GetPlayDualShockControllerInput(s);
                break;
            default:
                result = GetMouseAndKeyboardInput(s);
                break;
        }

        return result;
    }

    private bool GetXboxControllerInput(string s)
    {
        switch (s)
        {
            //check which action the player wants
            case "LeftHandInput":
            case "RightHandInput":
                {
                    return CrossPlatformInputManager.GetButtonDown("XB" + s);
                }
        }

        return false;
    }

    private bool GetPlayDualShockControllerInput(string s)
    {
        switch (s)
        {
            //check which action the player wants
            case "LeftHandInput":
            case "RightHandInput":
                return CrossPlatformInputManager.GetButtonDown("DS" + s);
        }

        return false;
    }

    private bool GetMouseAndKeyboardInput(string s)
    {
        switch (s)
        {
            //check which action the player wants
            case "LeftHandInput":
            case "RightHandInput":
                return CrossPlatformInputManager.GetButtonDown("MK" + s);
        }

        return false;
    }
}