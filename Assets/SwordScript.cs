using UnityEngine;
using System.Collections;

public class ExtinguisherScript : MonoBehaviour {

    public GameObject SP1;
    public GameObject SP2;
    public GameObject SP3;
    public GameObject SP4;

    void Start()
    {
        int x = Random.Range(1, 4);
        Debug.Log(x);

        if (x == 1)
        {
            transform.position = SP1.transform.position;
        }
        else if (x == 2)
        {
           transform.position = SP2.transform.position;
        }
        else if (x == 3)
        {
            transform.position = SP3.transform.position;
        }
        else if (x == 4)
        {
            transform.position = SP4.transform.position;
        }
    }

	void Update () {
        transform.Rotate(new Vector3(0,180, 0) * Time.deltaTime);
	}
}
