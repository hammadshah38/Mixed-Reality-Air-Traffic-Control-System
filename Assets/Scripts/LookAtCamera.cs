using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    public GameObject plane;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        string parent = this.transform.parent.name;
        if(parent.Contains("waypoint"))
        {
            transform.LookAt(Camera.main.transform);
            this.transform.localPosition = new Vector3(plane.transform.localPosition.x, plane.transform.localPosition.y-0.05f, plane.transform.localPosition.z);
        }
        else
        {
            transform.LookAt(Camera.main.transform);
            this.transform.localPosition = new Vector3(plane.transform.localPosition.x, plane.transform.localPosition.y /*- 0.1f*/, plane.transform.localPosition.z);
        }
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y , transform.rotation.z + 180, transform.rotation.w);
    }
    void LateUpdate()
    {
        
    }
}
