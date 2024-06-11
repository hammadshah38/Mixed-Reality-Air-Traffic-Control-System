using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowAirplaneInfo : MonoBehaviour, IInputClickHandler
{


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject camera = GameObject.Find("MixedRealityCameraParent");
        GameObject inputManager = GameObject.Find("InputManager");
        if (this.name == "MapView")
        {
            Object.Destroy(camera);
            Object.Destroy(inputManager);
            SceneManager.LoadScene("MapView");
        }
        else if (this.name == "TowerView")
        {
            Object.Destroy(camera);
            Object.Destroy(inputManager);
            SceneManager.LoadScene("DetailedView");
        }
        else if (this.name == "AirportView")
        {
            Object.Destroy(camera);
            Object.Destroy(inputManager);
            SceneManager.LoadScene("ModelExplorer");
        }
        else
        {
            GameObject a = this.transform.GetChild(0).gameObject;

            if (a.active == true)
            {
                a.active = false;
            }
            else
            {
                a.active = true;
            }
        }
        


    }
}
