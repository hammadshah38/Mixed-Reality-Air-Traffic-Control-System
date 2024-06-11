using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using FlightDataService.DataObjects;
using System.Globalization;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class LoadAtRealWorldLocation : MonoBehaviour
{
    public GameObject Airplane;
    public TextMesh a, b, c;
    public string TopLevelName = "Airport";
    int counter;
    public float simulaionSpeed;
    private Dictionary<string, GameObject> _airplanes;

    //    private readonly TimeSpan _waitTime = TimeSpan.FromSeconds(5);

    //    private DateTimeOffset _lastUpdate = DateTimeOffset.MinValue;

    //    //private Q
    private async void Start()
    {
        try
        {
            //CreateAirplane(new Vector3(37.5523f, 127.0735f, 0), new Vector3(37.5524f, 127.0736f, 0), 0, "flight");

            CreateAirplane(new Vector3(37.550555f, 127.075f, 0), new Vector3(37.550560f, 127.074668f, 0), 0, "flight");
            //reverse
            //CreateAirplane(new Vector3(37.550560f, 127.074668f, 0), new Vector3(37.550555f, 127.075f, 0), 0, "flight");

            //CreateAirplane(tower, new Vector3(rw31endpoint.x, rw31endpoint.y, 0), 0, "flight1");
        }
        catch (Exception e)
        {
            a.text = e.Message.ToString();
        }
    }
    private void CreateAirplane(Vector3 origin, Vector4 destination, int i, string name)
    {
        //foreach (var flight in flights)
        //{
        var airplane = Instantiate(Airplane);
        airplane.transform.name = name;
        airplane.transform.parent = GameObject.Find("MixedRealityCameraParent").transform;
        airplane.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
        //GameObject ab = this.transform.GetChild(0).gameObject;
        //TextMesh l = ab.GetComponent<TextMesh>();
        //l.text = name;
        //ab.transform.LookAt(Camera.main.transform);
        SetNewFlightData(airplane, origin, destination, name);

        Vector3 direction = airplane.transform.position - Camera.main.transform.position;
        var l = direction.magnitude;
        Debug.Log("Magnitude after mapping: " + l.ToString());

        _airplanes.Add(name, airplane);
        //}
    }
    private void UpdateAirplane(Vector3 origin, Vector4 destination, string name)
    {
        var airplane = GameObject.Find(name);
        SetNewFlightData(airplane, origin, destination, name);
    }
    private void SetNewFlightData(GameObject aircraft, Vector3 origin, Vector4 destination, string name)
    {
        var controller = aircraft.GetComponent<AirplaneController>();
        if (controller != null)
        {
            controller.SetNewFlightData(origin, destination, name, "realWorld");
        }
    }
    void Update()
    {
        try
        {
            c.text = Camera.main.transform.position.ToString();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message.ToString());
        }
    }
}