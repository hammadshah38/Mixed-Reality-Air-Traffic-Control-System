using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        if(SceneManager.GetActiveScene().name == "MapView")
        {
            SceneManager.LoadScene("ModelExplorer");
        }
        else
        {
            SceneManager.LoadScene("MapView");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
