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

            PlayerPrefs.SetInt("difficulty", dropdownBar.value);
        });
    }
}
