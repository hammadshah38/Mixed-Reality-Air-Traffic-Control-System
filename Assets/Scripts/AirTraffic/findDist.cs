using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class findDist : MonoBehaviour {
    public GameObject a, b;
    //public TextMesh a;
    //// Use this for initialization
    void Start()
    {
        float dist = Vector3.Distance(a.transform.position, b.transform.position);

        Debug.Log("Distance between spheres" + dist.ToString());
    }
    //    private void Start()
    //    {
    //#if WINDOWS_UWP
    //        getPath();
    //#endif
    //        //Debug.Log(path);
    //    }
    //#if WINDOWS_UWP
    //    public async void getPath()
    //    {
    //        try
    //        {
    //        //9C8625_1e9ac7f2.csv_traj

    //        //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
    //        StorageFolder localFolder = Windows.Storage.KnownFolders.CameraRoll;

    //        //Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
    //        StorageFile file = await localFolder.GetFileAsync("9C8625_1e9ac7f2.csv_traj");

    //        string text = await Windows.Storage.FileIO.ReadTextAsync(file);
    //        string path = text + " -> " + localFolder.Name + " folder path: " + localFolder.Path;
    //        a.text = path;

    //        }
    //        catch(Exception e)
    //        {
    //            a.text = e.Message.ToString();
    //        }
    //    }
    //#endif
    // Update is called once per frame
    void Update () {
		
	}
}
