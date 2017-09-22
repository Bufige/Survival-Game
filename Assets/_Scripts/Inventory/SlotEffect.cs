using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SlotEffect : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {


	private Outline outline;


	// Use this for initialization
	void Start () {
		outline = GetComponent<Outline> ();
		outline.enabled = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		outline.enabled = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		outline.enabled = false;
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
