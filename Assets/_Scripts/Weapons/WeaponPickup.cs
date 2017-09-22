using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponPickup : MonoBehaviour {

	public string weaponName = "Colt";
	public bool isMelee = false;
	//public string pickupName = "Colt";


	private GameObject weaponManager;
	private WeaponManager weaponScript;
	private Text ammoText;


	// Use this for initialization
	void Start () {
		weaponManager = GameObject.FindGameObjectWithTag ("WeaponManager");
		weaponScript = weaponManager.GetComponent<WeaponManager> ();
		//ammoText = GameObject.
		//GameObject.find

//		GameObject canvasObject = GameObject.FindGameObjectWithTag("MainCanvas");
//		Transform textTr = canvasObject.transform.Find("ObjectName");
//		Text text = textTr .GetComponent<Text>();

		ammoText = GameObject.FindGameObjectWithTag ("PlayerCanvas").transform.Find("AmmoText").GetComponent<Text>();
		/*
		foreach (var x in GameObject.FindObjectsOfType<Text>()) {
			Debug.Log (x.gameObject.name);
		}*/
		//foreach(var i in )
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Interact()
	{
		if (isMelee) {
			ammoText.enabled = false;
		} else {
			ammoText.enabled = true;
		}
		// you can do this way by checking.
		/*
		if (weaponScript.hasWeapon == false) { // we do not have any weapon.
			GameObject weaponFound = weaponManager.transform.FindChild(weaponName).gameObject;
			weaponFound.SetActive (true);
			weaponScript.hasWeapon = true;
		}*/
		// or you can desactive its children object and active the current pickup.

		ActiveWeapon(weaponName);
		weaponScript.hasWeapon = true;
		Destroy (gameObject);
	}
	private void ActiveWeapon(string WeaponName) // this script will activate the current weapon and desactivate the other one active.
	{
		foreach(Transform child in weaponManager.transform)
		{			
			if (child.gameObject.name == weaponName) {				
				child.gameObject.SetActive (true);
			} else {				
				child.gameObject.SetActive (false);
			}
		}
	}
}
