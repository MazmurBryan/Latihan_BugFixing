using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

	public delegate void GameDelegate ();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countdownPage;
	public Text scoreText;
	public int _score;
	

	enum PageState{
		None,
		Start,
		GameOver,
		Countdown
	}

	//string score = "";
    bool gameOver = true;

	public bool GameOver { get { return gameOver; } }

	void Awake(){
	
		Instance = this;
	}

	void OnEnable(){
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	
	}

	void OnDisable(){
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	
	}

	void OnCountdownFinished(){
		SetPageState (PageState.None);
		OnGameStarted ();
		
		gameOver = false;
	
	}

	void OnPlayerDied(){
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt ("highscore");
		if (_score > savedScore) {
            //score = _score.ToString();
            PlayerPrefs.SetInt ("highscore", _score);
			
		
		}
		_score = 0;
		SetPageState (PageState.GameOver);
	}

	void OnPlayerScored(){

		_score = _score + 1;
		scoreText.text = _score.ToString();
	}

	void SetPageState(PageState state){

		switch (state) {

		case PageState.None:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (false);
			break;
		case PageState.Start:
			startPage.SetActive (true);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (false);
			break;
		case PageState.GameOver:
			startPage.SetActive (false);
			gameOverPage.SetActive (true);
			countdownPage.SetActive (false);
			break;
		case PageState.Countdown:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (true);
			break;

		}
	}

	public void ConfirmedGameOver(){
		OnGameOverConfirmed();
		scoreText.text="0";
		SetPageState (PageState.Start);
	}
	public void StartGame(){
		SetPageState(PageState.Countdown);
	}
}
