using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public GameObject target;
	// Use this for initialization
	void Start ()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1 * Time.deltaTime, 0.0f);

        Vector3 newDir = target.transform.position + targetDir.normalized;
        newDir = new Vector3(newDir.x, newDir.y - 0.5f, newDir.z);
        Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our position a step closer to the target.
        transform.LookAt(target.transform.position + target.transform.forward);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.rotation = new Quaternion(transform.rotation.x + 0.05f, transform.rotation.y - 0.35f, transform.rotation.z,transform.rotation.w);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
