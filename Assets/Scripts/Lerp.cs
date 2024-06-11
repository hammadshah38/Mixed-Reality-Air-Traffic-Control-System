using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour {
    public GameObject a;
    int j,i;
	// Use this for initialization
	void Start () {
        j = 0;
        i = 0;
        while (true)
        {
            if (j == 0)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, a.transform.position, Time.deltaTime/5);//new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);
                j = 1;
            }
            else if (j == 1)
            {
                i++;
                Debug.Log(i.ToString());
                if (this.transform.position == a.transform.position)
                {
                    Debug.Log("Reached");
                    break;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
