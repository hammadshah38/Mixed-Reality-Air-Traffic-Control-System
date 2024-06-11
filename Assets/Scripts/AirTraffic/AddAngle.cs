using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAngle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles/*localEulerAngles*/.x, (this.transform.eulerAngles/*localEulerAngles*/.y - 30) /*+ 360*/, this.transform.eulerAngles/*localEulerAngles*/.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
