using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class ManageAnchors : MonoBehaviour {

    public GameObject obj;
    public GameObject cursor;
    public GameObject loader;
    public TextMesh txt;
	// Use this for initialization
	void Start ()
    {
        try
        {
            if(this.GetComponent<WorldAnchor>() != null)
            {
                cursor.SetActive(false);
                loader.SetActive(true);
            }
            
        }
        catch (Exception e)
        {
            txt.text = e.ToString();
        }
        //WorldAnchor wa = WorldAnchorManager.Instance.AnchorStore.Load(obj.name, obj);
        
        //if (wa != null)
        //{
        //    WorldAnchorManager.Instance.AttachAnchor(obj);
        //    txt.text = "loaded";
        //    cursor.SetActive(false);
        //    loader.SetActive(true);
        //}
        //else
        //{
        //    txt.text = "not loaded";
        //}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
