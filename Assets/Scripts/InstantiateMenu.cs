using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateMenu : MonoBehaviour {
    public GameObject menu, camera;
    public float distance;
	// Use this for initialization
	void Start ()
    {
        //Instantiate(menu, Camera.main.transform.position + Camera.main.transform.forward * distance, transform.rotation);
        menu.SetActive(true);
        Vector3 cameraPos = Camera.main.transform.position + Camera.main.transform.forward;
        Vector3 menuPos = new Vector3(cameraPos.x + 0.15f, cameraPos.y, cameraPos.z + 0.15f);
        menu.transform.position = (cameraPos * distance);
        menu.transform.position = new Vector3(menu.transform.position.x + 0.15f, menu.transform.position.y, menu.transform.position.z /*+ 0.15f*/);
        menu.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);

        menu.transform.LookAt(Camera.main.transform);
        menu.transform.rotation = new Quaternion(menu.transform.rotation.x, menu.transform.rotation.y - 10, menu.transform.rotation.z, menu.transform.rotation.w);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
