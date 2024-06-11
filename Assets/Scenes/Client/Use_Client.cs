using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use_Client : MonoBehaviour
{
    ATC_Client obj;
	// Use this for initialization
	void Start () {
        obj = new ATC_Client("Hammad\n");
        obj.ConnectUnity("223.195.36.189", "10000");
//#if UNITY_EDITOR
//        obj.ConnectUnity("223.195.36.189", "10000");
//        #else
//                await obj.ConnectUnity("223.195.36.189", "10000");
//        #endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}
