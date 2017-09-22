using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interact : MonoBehaviour {

	[Header("Debug")]
	public bool debugMode;

	[Header("Configuration")]
	public float interactRange = 3f;
	public Camera FPSCamera;
	public KeyCode interactKey = KeyCode.E;


	public Text interactText;
	private FoodItem foodItem;
	private Player player;
	private Utility utility;

	// Use this for initialization
	void Start () {
		utility = GameObject.FindGameObjectWithTag("Utility").GetComponent<Utility>();
		InvokeRepeating ("GetPlayer", 5f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = FPSCamera.ScreenPointToRay (new Vector2(Screen.width/2,Screen.height/2)); // set ray at the center of the screen
		RaycastHit hitInfo;


		interactText.text = "[" + interactKey + "]" + " Interact";

		if (debugMode)
			Debug.DrawRay (ray.origin,ray.direction*interactRange,Color.black);

		if (Physics.Raycast (ray, out hitInfo, interactRange)) {


			if (hitInfo.collider.gameObject.layer == 8) { // 8 = interact
				interactText.enabled = true;
			} else {
				interactText.enabled = false;
			}


			if (Input.GetKeyDown (interactKey)) {
				if (hitInfo.collider.tag == "FoodItem") { // if food is detected.
					foodItem = hitInfo.collider.GetComponent<FoodItem> ();
					if (debugMode && player != null) {
						Debug.Log ("foodname: " + foodItem.name);
					}
					if (foodItem.hungerType == FoodItem.HungerType.Food) {
						if (player != null)
							player.AddFood (foodItem.amountToAdd);
						foodItem.DestroyObject ();
					} else if (foodItem.hungerType == FoodItem.HungerType.Water) {
						if (player != null)
							player.AddThrist (foodItem.amountToAdd);
						foodItem.DestroyObject ();
					}
				} else if (hitInfo.collider.tag == "WeaponPickup") { // weapon pickup detected
					WeaponPickup weaponPickup = hitInfo.collider.GetComponent<WeaponPickup> ();
					weaponPickup.Interact ();
				} else if (hitInfo.collider.tag == "AmmoPickup") { // ammo pickup detected
					AmmoPickup ammoPickup = hitInfo.collider.GetComponent<AmmoPickup> ();
					ammoPickup.Interact ();
				} else if (hitInfo.collider.tag == "WeaponPickup") {
					AmmoPickup ammoPickup = hitInfo.collider.GetComponent<AmmoPickup> ();
					ammoPickup.Interact ();
				}
			}
		} else { // player did not hit no item.
			interactText.enabled = false;
		}
	}

	private void GetPlayer()
	{
		player = utility.FindClosestPlayer (gameObject).GetComponent<Player>(); // We find closest player;
	}
}
