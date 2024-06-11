using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour {
    // Use this for initialization
    //GameObject txt;
    //TextMesh a;
    void Start ()
    {
        //txt = GameObject.Find("ARCameraText");
        //a = txt.GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        
        try
        {
            GameObject camera = GameObject.Find("MixedRealityCameraParent");
            GameObject arCamera = GameObject.Find("ARCamera");
            GameObject inputManager = GameObject.Find("InputManager");
            GameObject manager = GameObject.Find("Manager/DictationManager");
            //GameObject st = GameObject.Find("StatusText1");
            //TextMesh StatusText = st.GetComponent<TextMesh>();
            //StatusText.text = "Collided with " + collision.gameObject.name;

            if (collision.gameObject.name == "MapView")
            {
                Object.Destroy(camera);
                Object.Destroy(arCamera);
                Object.Destroy(inputManager);
                Object.Destroy(manager);
                //WorldAnchorManager.Instance.RemoveAllAnchors();
                //SceneManager.LoadScene("MapView");
                SceneManager.LoadScene(1);
            }
            else if (collision.gameObject.name == "AirportView")
            {
                Object.Destroy(camera);
                Object.Destroy(arCamera);
                Object.Destroy(inputManager);
                Object.Destroy(manager);
                //WorldAnchorManager.Instance.RemoveAllAnchors();
                //SceneManager.LoadScene("ModelExplorer");
                SceneManager.LoadScene(0);
            }
        }
        catch(System.Exception e)
        {
            //txt.SetActive(true);
            //a.text = e.Message;
        }
    }
}
