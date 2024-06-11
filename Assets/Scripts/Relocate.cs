using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relocate : MonoBehaviour {

    public GameObject loader;
    public GameObject tower;
	// Use this for initialization
	void Start ()
    {
		if(this.gameObject.active == true)
        {
            tower.SetActive(true);
            loader.SetActive(true);            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
