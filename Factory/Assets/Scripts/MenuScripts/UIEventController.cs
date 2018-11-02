using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class UIEventController : MonoBehaviour
{

    //[SerializeField] private Gamepad gamepad = Gamepad.XboxController;
    [SerializeField] private EventSystem DSEventSystem;
    [SerializeField] private EventSystem XBEventSystem;
    [SerializeField] private GameObject DSEventSystemObject;
    [SerializeField] private GameObject XBEventSystemObject;
    [SerializeField] private GameObject firstSelected;

    private Gamepad gamepadPreference;

    private bool buttonSelected;

    // Use this for initialization
    void Start ()
    {
        //gamepadPreference = PlayerPrefs.GetString("gamepad");

        switch (PlayerPrefs.GetString("gamepad"))
        {
            case "dualshock":
                gamepadPreference = Gamepad.DualshockController;
                break;
            default:
                gamepadPreference = Gamepad.XboxController;
                break;
        }

        DSEventSystemObject.SetActive(false);
        XBEventSystemObject.SetActive(false);

        if (gamepadPreference == Gamepad.DualshockController)
        {
            //gamepad = Gamepad.DualshockController;
            //if (Input.GetAxis("UIDSVertical") != 0 && buttonSelected == false)
            //{
                DSEventSystemObject.SetActive(true);
                DSEventSystem.SetSelectedGameObject(firstSelected);
                buttonSelected = true;
            //}
        }
        else
        {
            //gamepad = Gamepad.XboxController;
            //if (Input.GetAxis("UIXBVertical") != 0 && buttonSelected == false)
            //{
                XBEventSystemObject.SetActive(true);
                XBEventSystem.SetSelectedGameObject(firstSelected);
                buttonSelected = true;
            //}
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*if (gamepadPreference == Gamepad.DualshockController)
        {
            Debug.Log(DSEventSystem.currentSelectedGameObject.name + " selected");
        }
        else
        {
            Debug.Log(XBEventSystem.currentSelectedGameObject.name + " selected");
        }*/

        if (gamepadPreference == Gamepad.XboxController)
        {
            if (CrossPlatformInputManager.GetButtonDown("P1XBSubmit") || CrossPlatformInputManager.GetButtonDown("P2XBSubmit"))
            {
                //Debug.Log("Clicking " + XBEventSystem.currentSelectedGameObject.name);
                XBEventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
        else
        {
            if (CrossPlatformInputManager.GetButtonDown("P1DSSubmit") || CrossPlatformInputManager.GetButtonDown("P2DSSubmit"))
            {
                //Debug.Log("Clicking " + DSEventSystem.currentSelectedGameObject.name);
                DSEventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }

}
