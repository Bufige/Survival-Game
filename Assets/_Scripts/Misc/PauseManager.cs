using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
public class PauseManager : MonoBehaviour {
	public enum GameState
	{
		Playing,
		Paused,
		Death,
		AcessingInventory
	}

	[Header("Configuration")]
	public GameObject pauseCanvas;
	public KeyCode pauseKey = KeyCode.Escape;
	public GameState gameState = GameState.Playing;


	private Blur blur;

	// Use this for initialization
	void Start () {
		blur = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Blur> ();
		pauseCanvas.SetActive (false);		// we define pause as false.
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (pauseKey) ) {	
			if (gameState != GameState.Paused) {
				gameState = GameState.Paused;
			} else {
				gameState = GameState.Playing;
			}
		}


		if (gameState == GameState.Playing) {
			Time.timeScale = 1.0f; // normal game speed;
			pauseCanvas.SetActive (false);
			blur.enabled = false;
		} else if (gameState == GameState.Paused) {
			Time.timeScale = 0.0f; // freeze the game;
			pauseCanvas.SetActive (true);
			blur.enabled = true;
		} else if (gameState == GameState.Death) {
			Time.timeScale = 0.0f; // freeze the game;
		} else if (gameState == GameState.AcessingInventory) {
			// do something if acessing inventory
		}
	}

	public void ResumeGame()
	{		
		gameState = GameState.Playing;
	}
	public void ExitGame()
	{
		Application.Quit();
	}
}
