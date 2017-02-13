using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public bool buttonpressed;
	public GameObject hero;
	public Animator ButtonAnimator;

	// Use this for initialization
	void Start () {
		hero = GameObject.FindGameObjectWithTag ("Hero");
		buttonpressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Input.GetButtonDown ("Fire1")) {
			if (Physics.Raycast (ray, out hit) == true) {
				if (hit.transform.gameObject.CompareTag ("Button") &&
				    Mathf.Abs (Vector3.Distance (hit.transform.gameObject.transform.position, hero.transform.position)) < 2) {
					ButtonAnimator.SetTrigger ("ButtonPressed");
					buttonpressed = true;
				}
			}
		}
	}
}
