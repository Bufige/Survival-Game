using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class ShootingSystem : MonoBehaviour {

	[Header("Debug")]
	public bool debugMode;


	[Header("GunMainStats")]
	private KeyCode reloadingKey = KeyCode.R;
	public float reloadingTime = 1f;
	private float nextReloading;
	public int clip = 7;
	public int clipMax = 7;
	public int reserveAmmo = 28;
	public bool clipEmpty;
	public float fireRate = 0.2f;
	public bool isReloading;
	public float hitPower = 500f;
	private float nextBullet;



	[Header("Configuration")]
	public int minimumDamage = 15;
	public int maximumDamage = 30;
	public float weaponRange = 100f;
	public float aimSpeed = 2f;
	public float recoilAmount = 0.5f;
	public float vignetteAimIntensity = 0.3f;
	public float vignetteSpeed = 5f;
	public float fovChange = 15f;
	public float fovSpeed = 5f;
	private float defaultFOV;
	private float aimFov;




	public Camera FPSCamera;

	public float xSwayAmount = 0.1f;
	public float ySwayAmount = 0.1f;
	public float xSwayMax = 0.3f;
	public float ySwayMax = 0.3f;
	public float ySwaySmoothAmount = 3f;
	private Vector3 newSwayPosition = Vector3.zero;

	public Text ammoText;
	public GameObject weaponObject;
	public Vector3 normalPosition;
	public Vector3 aimingPosition;
	public bool isAiming;


	[Header("Audio")]
	public AudioClip gunShotClip;
	private AudioSource audioSource;




	private PauseManager pauseManager;
	private AdvancedEnemyAI enemy;
	private VignetteAndChromaticAberration vignette;

	private Ray ray;
	private RaycastHit hitInfo;
	private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
		weaponManager = GameObject.FindGameObjectWithTag ("WeaponManager").GetComponent<WeaponManager>();
		audioSource = GetComponent<AudioSource> (); // find audio source
		pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager> ();
		vignette = FPSCamera.GetComponent<VignetteAndChromaticAberration> ();


		reloadingKey = weaponManager.reloadingKey;
		defaultFOV = FPSCamera.fieldOfView; // default fog
		aimFov = FPSCamera.fieldOfView - fovChange;
		ammoText.enabled = true;
		isAiming = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (pauseManager.gameState != PauseManager.GameState.Playing) {
			return;
		}

		ray = FPSCamera.ScreenPointToRay (new Vector2(Screen.width/2,Screen.height/2));


		if (debugMode)
			Debug.DrawRay (ray.origin,ray.direction*weaponRange,Color.yellow);

		UpdateReload ();
		Shot ();


		if (clip <= 0) {
			clipEmpty = true;


		} else {
			clipEmpty = false;
		}


		if (Input.GetKeyDown (reloadingKey)) {
			if (isReloading == false) {
				CheckReload ();
			}
		} else if (clipEmpty == true) {
			if (isReloading == false) {
				CheckReload ();
			}
		}


		if (Input.GetKey (KeyCode.Mouse1)) {
			isAiming = true;
		} else {
			isAiming = false;
		}
	}

	private void FixedUpdate()
	{
		if (pauseManager.gameState != PauseManager.GameState.Playing) {
			return;
		}


		UpdateText ();

		//Weapon Sway
		float x = Input.GetAxis("Mouse X") * xSwayAmount;
		float y = Input.GetAxis ("Mouse Y") * ySwayAmount;

		if (x > xSwayMax) {
			x = xSwayMax;
		}
		if (x < -xSwayMax) {
			x = xSwayMax;
		}
		if (y > ySwayMax) {
			y = ySwayMax;
		}
		if (y < -ySwayMax) {
			y = ySwayMax;
		}

		Vector3 detection = new Vector3 (newSwayPosition.x + x, newSwayPosition.y + y,newSwayPosition.z);
		transform.localPosition = Vector3.Lerp (transform.localPosition,detection,Time.deltaTime * ySwaySmoothAmount);

		// Aiming

		if (isAiming == true) {
			weaponObject.transform.localPosition = Vector3.Lerp (
				weaponObject.transform.localPosition ,
				aimingPosition, Time.deltaTime * aimSpeed
			);

			vignette.intensity = Mathf.Lerp (vignette.intensity,vignetteAimIntensity,Time.deltaTime * vignetteSpeed);
			FPSCamera.fieldOfView = Mathf.Lerp (FPSCamera.fieldOfView,aimFov - fovChange,Time.deltaTime * fovSpeed);

		} else if (isAiming == false) {
			weaponObject.transform.localPosition = Vector3.Lerp (
				weaponObject.transform.localPosition,
				normalPosition,Time.deltaTime * aimSpeed
			);
			vignette.intensity = Mathf.Lerp (vignette.intensity,0f,Time.deltaTime * vignetteSpeed);
			FPSCamera.fieldOfView = Mathf.Lerp (FPSCamera.fieldOfView,defaultFOV,Time.deltaTime * fovSpeed);
		}
	}

	private void Shot()
	{
		if (Input.GetKeyDown (KeyCode.Mouse0) && Time.time > nextBullet  && clipEmpty == false && isReloading == false ) {	

			nextBullet = Time.time + fireRate;
			clip -= 1;

			audioSource.PlayOneShot (gunShotClip);

			// Recoil code.
			Vector3 weaponObjectLocalPosition = weaponObject.transform.localPosition;
			weaponObjectLocalPosition.z -= recoilAmount;
			weaponObject.transform.localPosition = weaponObjectLocalPosition;

			if(Physics.Raycast (ray,out hitInfo,weaponRange)){
				if (hitInfo.collider.tag == "Enemy") {
					enemy = hitInfo.collider.GetComponent<AdvancedEnemyAI> ();
					enemy.TakeDamage (Damage());

					if (debugMode)
						Debug.Log ("Shoot :" + enemy.name);
				}
				else if (hitInfo.rigidbody != null) {
					var col = hitInfo.collider.GetComponent<Rigidbody> ();
					if (col != null) {
						col.AddForceAtPosition (transform.TransformDirection (Vector3.right) * hitPower, hitInfo.point);
				
					}
				}
			}
		}

	}
	private void CheckReload()
	{
		if (clip > 0) {
			reserveAmmo += clip;
			clip = 0;
		}

		if (reserveAmmo > 0) {
			nextReloading = Time.time + reloadingTime;
		}

		if (reserveAmmo >= clipMax) {
			reserveAmmo -= clipMax;
			clip = clipMax;
		} else {
			clip = reserveAmmo;
			reserveAmmo = 0;
		}
	}

	private void UpdateReload()
	{
		if (nextReloading > Time.time) {			
			isReloading = true;
		} else {
			isReloading = false;
		}
	}

	private void UpdateText()
	{
		ammoText.text = "Ammo: " + clip.ToString () + "/" + reserveAmmo;
	}

	private int Damage()
	{
		return Random.Range (minimumDamage, maximumDamage); //Get random damage;
	}
}
