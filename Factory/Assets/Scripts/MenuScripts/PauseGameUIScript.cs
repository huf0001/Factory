using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGameUIScript : MonoBehaviour
{
    [SerializeField] private Text robScoreText;
    [SerializeField] private Text botScoreText;

    // Use this for initialization
    void OnEnable()
    {
        //set players score text 
        int player1Score = PlayerPrefs.GetInt("player1score");
        int player2Score = PlayerPrefs.GetInt("player2score");
        robScoreText.text = player1Score.ToString();
        botScoreText.text = player2Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
