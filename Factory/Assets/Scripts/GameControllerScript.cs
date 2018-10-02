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

    //Checks if the player clicked the specified button
    public bool GetButtonDown(string s)
    {
        bool result = false;

        switch (s)
        {
            //check which action the player wants
            case "ToggleThrow":
            case "Jump":
            case "ToggleHorizontal":
            case "ChangeCamera":
                result = CrossPlatformInputManager.GetButtonDown(GetGamepadPrefix() + s);
                break;
            case "LeftArm":
            case "RightArm":
                if (gamepad == Gamepad.MouseAndKeyboard)
                {
                    result = CrossPlatformInputManager.GetButtonDown(GetGamepadPrefix() + s);
                }
                else if (GetAxis(s) != 0)
                {
                    result = true;
                }

                break;
        }

        return result;
    }

    //Checks if the player is holding the specified button down
    public bool GetButton(string s)
    {
        bool result = false;

        switch (s)
        {
            //check which action the player wants
            case "ToggleThrow":
            case "Jump":
            case "ToggleHorizontal":
            case "ChangeCamera":
                result = CrossPlatformInputManager.GetButton(GetGamepadPrefix() + s);
                break;
            case "LeftArm":
            case "RightArm":
                if (gamepad == Gamepad.MouseAndKeyboard)
                {
                    result = CrossPlatformInputManager.GetButton(GetGamepadPrefix() + s);
                }
                else if (GetAxis(s) != 0)
                {
                    result = true;
                }

                break;
        }

        return result;
    }

    public float GetAxis(string s)
    {
        float result = 0f;

        switch (s)
        {
            //check which action the player wants
            
            case "MoveHorizontal":
            case "MoveVertical":
            case "LookHorizontal":
            case "LookVertical":
            case "LeftArm":
            case "RightArm":
                result = CrossPlatformInputManager.GetAxis(GetGamepadPrefix() + s);
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