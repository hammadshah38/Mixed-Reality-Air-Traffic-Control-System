using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountMesh : MonoBehaviour
{
    public GameObject mesh;
	// Use this for initialization
	void Start ()
    {
        var numTriangles = mesh.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length / 3;
        Debug.Log("Mesh Number" + numTriangles);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
