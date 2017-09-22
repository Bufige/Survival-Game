using UnityEngine;
using System.Collections;

public class WeaponSway : MonoBehaviour {

	public GameObject weaponObject;
	public float amount = 0.02f;
	public float maxamount = 0.03f;
	public float smooth = 3;
	private Quaternion def;
	private bool  Paused = false;

	private PauseManager pauseManager;

	void  Start (){
		def = weaponObject.transform.localRotation;
		pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager> ();
	}

	void  Update (){
		if (pauseManager.gameState != PauseManager.GameState.Playing) {
			return;
		}



		float factorX = (Input.GetAxis("Mouse Y")) * amount;
		float factorY = -(Input.GetAxis("Mouse X")) * amount;
		//float factorZ = -Input.GetAxis("Vertical") * amount;
		float factorZ = 0 * amount;

		if(!Paused){
			if (factorX > maxamount)
				factorX = maxamount;

			if (factorX < -maxamount)
				factorX = -maxamount;

			if (factorY > maxamount)
				factorY = maxamount;

			if (factorY < -maxamount)
				factorY = -maxamount;

			if (factorZ > maxamount)
				factorZ = maxamount;

			if (factorZ < -maxamount)
				factorZ = -maxamount;

			Quaternion Final = Quaternion.Euler(def.x+factorX, def.y+factorY, def.z+factorZ);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Final, (Time.time * smooth));
		}
	}
}