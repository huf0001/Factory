using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1GameUIScript : MonoBehaviour {
    //robs stuff
    [SerializeField] private Image[] robBuildObjectUI = new Image[3];
    [SerializeField] private BuildZone robBuildZone;

    //bots stuff
    [SerializeField] private Image[] botBuildObjectUI = new Image[3];
    [SerializeField] private BuildZone botBuildZone;

    //sprites for builds
    [SerializeField] private Sprite[] buildObjectSprites;
    private Sprite[] objectsToBuild;

    //timer stuff
    [SerializeField] private Text timerUIObject;
    [SerializeField] private GameController gameController;

    // Use this for initialization
    void Start()
    {
        int timerInt = Mathf.RoundToInt(gameController.Timer);
        string timerText = timerInt.ToString();
        timerUIObject.text = timerText;

        //setup rob
        for (int i = 0; i < robBuildObjectUI.Length; i++) {
            switch (robBuildZone.GetCurrentSchemaName(i)) {
                case "hard":
                    break;
                case "medium":
                    break;
                default:
                    break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //update time //continually retrieve timer value from game controller 
        int timerInt = Mathf.RoundToInt(gameController.Timer);
        string timerText = timerInt.ToString();
        timerUIObject.text = timerText;
    }
}
