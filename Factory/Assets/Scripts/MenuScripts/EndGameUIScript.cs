using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUIScript : MonoBehaviour {
    [SerializeField] private Text robScoreText;
    [SerializeField] private Text botScoreText;
    [SerializeField] private Image robOutcome;
    [SerializeField] private Image botOutcome;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;

    // Use this for initialization
    void OnEnable () {
        //set players score text 
        int player1Score = PlayerPrefs.GetInt("player1score");
        int player2Score = PlayerPrefs.GetInt("player2score");
        robScoreText.text = player1Score.ToString();
        botScoreText.text = player2Score.ToString();

        //set UI up for Rob winning
        //player 1 is rob
        if (PlayerPrefs.GetString("winner") == "player1")
        {
            robOutcome.sprite = winSprite;
            botOutcome.sprite = loseSprite;
        }
        //set up so both win
        else if (PlayerPrefs.GetString("winner") == "tie")
        {
            robOutcome.sprite = winSprite;
            botOutcome.sprite = winSprite;
        }
        //set ui up for Bot winning 
        //player 2 is bot 
        else if (PlayerPrefs.GetString("winner") == "player2")
        {
            robOutcome.sprite = loseSprite;
            botOutcome.sprite = winSprite;
        }
        else {
            robOutcome.sprite = loseSprite;
            botOutcome.sprite = loseSprite;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
