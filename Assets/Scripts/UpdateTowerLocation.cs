using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA;

public class UpdateTowerLocation : MonoBehaviour {
    public GameObject airport;
    public TextMesh txt;
	// Use this for initialization
	void Start ()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "MapView")
        {
            this.transform.position = new Vector3(airport.transform.position.x + 0.1f, airport.transform.position.y - 0.041f, airport.transform.position.z + 0.38f);
        }
        else
        {
            this.transform.position = new Vector3(airport.transform.position.x - 0.14f, airport.transform.position.y + 1.0618f, airport.transform.position.z + 2.8f);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        try
        {
            string sceneName = SceneManager.GetActiveScene().name;
            txt.text = airport.name;
            //if (WorldAnchorManager.Instance.AnchorStore != null)
            //{
            //    if (WorldAnchorManager.Instance.AnchorStore.GetAllIds().Length > 1)
            //    {
            //        if (sceneName == "ModelExplorer")
            //        {

            //            WorldAnchor wa = WorldAnchorManager.Instance.AnchorStore.Load("Airport(New)", GameObject.Find("Airport(New)"));
            //            Vector3 vec = new Vector3(wa.gameObject.transform.position.x, wa.gameObject.transform.position.y, wa.gameObject.transform.position.z);
            //            this.transform.position = new Vector3(vec.x + 0.1f, vec.y - 0.041f, vec.z + 0.38f);
            //            txt.text = "Using Anchor " + wa.name;
            //        }
            //        else
            //        {
            //            WorldAnchor wa = WorldAnchorManager.Instance.AnchorStore.Load("Depth_Map", GameObject.Find("Depth_Map"));
            //            Vector3 vec = new Vector3(wa.gameObject.transform.position.x, wa.gameObject.transform.position.y, wa.gameObject.transform.position.z);
            //            this.transform.position = new Vector3(vec.x + 0.1f, vec.y - 0.041f, vec.z + 0.38f);
            //            txt.text = "Using Anchor " + wa.name;
            //        }
            //    }
            //}
            //else
            //{
            //    txt.text = "Not Using Anchor ";
                if (sceneName == "MapView")
                {
                    this.transform.position = new Vector3(airport.transform.position.x + 0.1f, airport.transform.position.y - 0.041f, airport.transform.position.z + 0.38f);
                }
                else
                {
                    this.transform.position = new Vector3(airport.transform.position.x - 0.14f, airport.transform.position.y + 1.0618f, airport.transform.position.z + 2.8f);
                }
            //}
        }
        catch(Exception e)
        {
            txt.text = e.Message.ToString();
        }     
    }
}
