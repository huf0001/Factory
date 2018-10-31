using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1GameUIScript : MonoBehaviour {
    //robs stuff
    [SerializeField] private Image[] robBuildObjectUI = new Image[3];

    //bots stuff
    [SerializeField] private Image[] botBuildObjectUI = new Image[3];

    //sprites for builds
    [SerializeField] private Sprite[] buildSprites = new Sprite[4];
    private  List<Sprite> buildList = new List<Sprite>();

    //timer stuff
    [SerializeField] private Text timerUIObject;
    [SerializeField] private GameController gameController;
    private int difficulty;
    private int p1BuildCount;
    private int p2BuildCount;

    // Use this for initialization
    void Start()
    {
        int timerInt = Mathf.RoundToInt(gameController.Timer);
        string timerText = timerInt.ToString();
        timerUIObject.text = timerText;

        p1BuildCount = 0;
        p2BuildCount = 0;

        switch (PlayerPrefs.GetString("difficulty"))
        {
            case "hard":
                difficulty = 4;
                break;
            case "medium":
                difficulty = 3;
                break;
            default:
                difficulty = 2;
                break;
        }

        //create buildlist to cycle through based on difficulty
        for (int i = 0; i < buildSprites.Length; i++)
        {
            if (difficulty > i)
            {
                buildList.Add(buildSprites[i]);
            }
        }

        //for intial setup first two sprites are always going to be the same 3rd based on difficulty
        for (int c = 0; c < 2; c++) {
            robBuildObjectUI[c].sprite = buildList[c];
            botBuildObjectUI[c].sprite = buildList[c];
        }
        if (difficulty == 2) {
            robBuildObjectUI[2].sprite = buildList[0];
            botBuildObjectUI[2].sprite = buildList[0];
        }  
        else {
            robBuildObjectUI[2].sprite = buildList[2];
            botBuildObjectUI[2].sprite = buildList[2];
        }
    }
	
	// Update is called once per frame
	void Update () {
        //update time //continually retrieve timer value from game controller 
        int timerInt = Mathf.RoundToInt(gameController.Timer);
        string timerText = timerInt.ToString();
        timerUIObject.text = timerText;

        if (gameController.P1BuildCount > p1BuildCount) {
            p1BuildCount = gameController.P1BuildCount;
            SpriteComplete(robBuildObjectUI[0]);
            UpdateRobsBuilds();
        }

        if (gameController.P2BuildCount > p2BuildCount)
        {
            p2BuildCount = gameController.P2BuildCount;
            SpriteComplete(botBuildObjectUI[0]);
            UpdateBotsBuilds();
        }
    }

    private void SpriteComplete(Image completedBuild) {
        completedBuild.color = Color.green;
    }

    private void UpdateRobsBuilds() {
        robBuildObjectUI[0].color = Color.white;
        string name = "rob";
        switch (difficulty) {
            case 4:
                UpdateHard(robBuildObjectUI[0].sprite, name);
                break;
            case 3:
                UpdateMedium(robBuildObjectUI[0].sprite, name);
                break;
            default:
                UpdateEasy(robBuildObjectUI[0].sprite, name);
                break;
        }
    }
    private void UpdateBotsBuilds() {
        botBuildObjectUI[0].color = Color.white;
        string name = "bot";
        switch (difficulty)
        {
            case 4:
                UpdateHard(botBuildObjectUI[0].sprite, name);
                break;
            case 3:
                UpdateMedium(botBuildObjectUI[0].sprite, name);
                break;
            default:
                UpdateEasy(botBuildObjectUI[0].sprite, name);
                break;
        }
    }

    private void UpdateEasy(Sprite currentSprite, string name) {
        if (name == "rob") {
            if (currentSprite = buildList[0]) {
                robBuildObjectUI[0].sprite = buildList[1];
                robBuildObjectUI[1].sprite = buildList[0];
                robBuildObjectUI[2].sprite = buildList[1];
            }
            else {
                robBuildObjectUI[0].sprite = buildList[0];
                robBuildObjectUI[1].sprite = buildList[1];
                robBuildObjectUI[2].sprite = buildList[0];
            }
        }
        else {
            if (currentSprite = buildList[0])
            {
                botBuildObjectUI[0].sprite = buildList[1];
                botBuildObjectUI[1].sprite = buildList[0];
                botBuildObjectUI[2].sprite = buildList[1];
            }
            else
            {
                botBuildObjectUI[0].sprite = buildList[0];
                botBuildObjectUI[1].sprite = buildList[1];
                botBuildObjectUI[2].sprite = buildList[0];
            }
        }
    }
    private void UpdateMedium(Sprite currentSprite, string name) { }
    private void UpdateHard(Sprite currentSprite, string name) { }
}


