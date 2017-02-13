using UnityEngine;
using System.Collections;

public class npcDetector : MonoBehaviour {
    public GameObject Hero;
    private bool CloseToHero;
    private float distance;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        distance = (Hero.transform.position - this.transform.position).sqrMagnitude;

        if (distance < 5.0f)
            CloseToHero = true;
    }

    public bool npcClick()
    {

        if (Input.GetButtonDown("Fire1"))
        {

            // Find where the ray hit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Detect who the ray hit and allow interactions
            if (Physics.Raycast(ray, out hit) == true)
            {
                Vector3 characterPosition = hit.transform.gameObject.transform.position;
                Vector3 heroPosition = Hero.transform.position;
                if (hit.transform.gameObject.CompareTag("NPC") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 2)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
