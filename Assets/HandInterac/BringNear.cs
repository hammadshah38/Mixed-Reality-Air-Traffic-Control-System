using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringNear : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-0.25f);
        this.transform.position = this.transform.position + Camera.main.transform.forward * 0.004f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
