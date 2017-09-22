using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	[Header("Debug")]
	public bool debugMode;

	[Header("Health")]
	public float health = 100;
	public float maxHealth = 100;
	public float healthSpeed = 0.05f;

	[Header("Hunger")]
	public float hunger = 100f;
	public float maxHunger = 100f;
	public float hungerSpeed = 0.10f;

	[Header("Thrist")]
	public float thrist = 100f;
	public float maxThrist = 100f;
	public float thristSpeed = 0.25f;

	[Header("SlideBars")]
	public Slider hungerBar;
	public Slider thristBar;
	public Slider healthBar;

	private CursorManager cursorManager;
	public GameObject deathCanvas;
	public GameObject playerBarCanvas;

	public Vector3 spawnPoint = Vector3.zero;

	private bool IsDying,IsDyingFast;



	private Utility utility;

	// Use this for initialization
	void Start () {
		utility = GameObject.FindGameObjectWithTag("Utility").GetComponent<Utility>();
		deathCanvas.SetActive (false);
		cursorManager = GameObject.FindGameObjectWithTag ("CursorManager").GetComponent<CursorManager>();

		healthBar.maxValue = maxHealth;
		thristBar.maxValue = maxThrist;
		hungerBar.maxValue = maxHunger;
	}
	
	// Update is called once per frame
	void Update () {
		TakeScreenShot ();
		CheckDeath ();	//Check if the player is dead.
		CheckStats ();

		#region UIBarUpdate
		hungerBar.value = hunger;
		thristBar.value = thrist;
		healthBar.value = health;
		#endregion

	}

	private void CheckDeath()
	{
		if (health <= 0) {
			Die ();

		} else {
			
		}
	}
	private void Die()
	{		
		cursorManager.pauseManager.gameState = PauseManager.GameState.Death;

		deathCanvas.SetActive (true);
		playerBarCanvas.SetActive (false);
	}

	public void Respawn()
	{
		ResetBars ();
		cursorManager.pauseManager.gameState = PauseManager.GameState.Playing;
		deathCanvas.SetActive (false);
		playerBarCanvas.SetActive (true);
		transform.position = spawnPoint;
	}


	private void ResetBars()
	{
		//We reset the stats value;
		health = maxHealth;
		thrist = maxThrist;
		hunger = maxHunger;
	}

	public void AddFood(float amount)
	{
		hunger += amount;
	}
	public void AddThrist(float amount){
		thrist += amount;
	}

	public void AttackPlayer(float amount)
	{
		health -= amount;
	}


	private void CheckStats()
	{
		if (hunger > 0) {
			hunger -= Time.deltaTime * hungerSpeed;
		}
		if (hunger > maxHunger) {
			hunger = maxHunger;
		}
		if (thrist > 0) {
			thrist -= Time.deltaTime * thristSpeed;
		}
		if (thrist > 100) {
			thrist = maxThrist;
		}

		if (hunger == 0 || thrist == 0) {
			IsDying = true;
		} else {
			IsDying = false;
		}
		if (hunger == 0 && thrist == 0) {
			IsDyingFast = true;
		} else {
			IsDyingFast = false;
		}


		if (IsDying == true && !IsDyingFast) {
			health -= Time.deltaTime * healthSpeed;
		} else if (IsDyingFast == true) {
			health -= Time.deltaTime * (healthSpeed * 3);
		}
	}

	private void TakeScreenShot()
	{
		if (Input.GetKeyDown (KeyCode.F5)) {
			if (debugMode) {
				Debug.Log ("Player took a ScreenShot");
			}
			utility.TakeScreenshot ();
		}
	}
}
