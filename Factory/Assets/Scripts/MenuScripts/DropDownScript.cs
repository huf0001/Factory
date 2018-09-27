using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DropDownScript : MonoBehaviour {

    Dropdown dropdownBar;

    void Start()
    {
       dropdownBar = GetComponent<Dropdown>();

        dropdownBar.onValueChanged.AddListener(delegate {
            //stores difficulty based on dropdown choice 
            if (dropdownBar.value == 0) { PlayerPrefs.SetString("difficulty", "easy"); }
            else if (dropdownBar.value == 1) { PlayerPrefs.SetString("difficulty", "medium"); }
            else { PlayerPrefs.SetString("difficulty", "hard"); }
        });
    }
}
