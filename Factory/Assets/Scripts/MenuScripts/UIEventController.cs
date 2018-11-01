using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class UIEventController : MonoBehaviour {

    //[SerializeField] private Gamepad gamepad = Gamepad.XboxController;
    [SerializeField] private EventSystem DSeventSystem;
    [SerializeField] private EventSystem XboxeventSystem;
    [SerializeField] private GameObject DSeventSystemObject;
    [SerializeField] private GameObject XboxeventSystemObject;
    [SerializeField] private GameObject firstSelected;

    private bool buttonSelected;

    // Use this for initialization
    void Start () {
        string gamepadPreference = "";
        gamepadPreference = PlayerPrefs.GetString("gamepad");
        DSeventSystemObject.SetActive(false);
        XboxeventSystemObject.SetActive(false);
        if (gamepadPreference == "dualshock")
        {
            //gamepad = Gamepad.DualshockController;
            if (Input.GetAxis("UIDSVertical") != 0 && buttonSelected == false) {
                DSeventSystemObject.SetActive(true);
                DSeventSystem.SetSelectedGameObject(firstSelected);
                buttonSelected = true;
            }
        }
        else
        {
            //gamepad = Gamepad.XboxController;
            if (Input.GetAxis("UIXboxVertical") != 0 && buttonSelected == false)
            {
                XboxeventSystemObject.SetActive(true);
                XboxeventSystem.SetSelectedGameObject(firstSelected);
                buttonSelected = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }

}
