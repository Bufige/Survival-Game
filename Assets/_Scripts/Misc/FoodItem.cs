using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour {

	public enum HungerType
	{
		Food,
		Water
	};

	public HungerType hungerType = new HungerType();
	public float amountToAdd = 25f;

	public void DestroyObject()
	{
		Destroy (gameObject);
	}

}
