using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using FlightDataService.DataObjects;
using System.Globalization;
using System.Net.Sockets;
using System.Net;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class RealtimeSimulation : MonoBehaviour {
    //public GameObject Airplane;
    //public TextMesh a, b, c;
    //public string TopLevelName = "Airport(New)";
    //int counter;
    //public float simulaionSpeed;
    //private Dictionary<string, GameObject> _airplanes;

    //private GameObject _topLevelObject;
    
    //Vector3 tower;

    //public float roll, pitch, heading, speed, altitude, lat, lon;
    //private int clientPort = 55555;        // Client Port
    //UdpClient udpServer;

    ////#if WINDOWS_UWP
    //void Start()
    //{
    //    try
    //    {
    //        udpServer = new UdpClient(clientPort);
    //        b.text = "Loading Trajectories. Please wait until this text disappears...";
    //        counter = 1;

            
    //        tower.x = 33.505194f;
    //        tower.y = 126.491972f;
    //        tower.z = 0;

    //        _topLevelObject = GameObject.Find(TopLevelName);

    //        b.text = "";
    //        _airplanes = new Dictionary<string, GameObject>();
    //    }
    //    catch (Exception e)
    //    {
    //        a.text = e.Message.ToString();
    //    }
    //}

    //private void CreateAirplane(Vector3 origin, Vector4 destination, int i, string name)
    //{
    //    //foreach (var flight in flights)
    //    //{
    //    var airplane = Instantiate(Airplane);
    //    airplane.transform.name = name;
    //    //airplane.transform.parent = _topLevelObject.transform;
    //    airplane.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
    //    //GameObject ab = this.transform.GetChild(0).gameObject;
    //    //TextMesh l = ab.GetComponent<TextMesh>();
    //    //l.text = name;
    //    //ab.transform.LookAt(Camera.main.transform);
    //    _airplanes.Add(name, airplane);
    //    SetNewFlightData(airplane, origin, destination, name);
        
    //    //}
    //}
    //private void UpdateAirplane(Vector3 origin, Vector4 destination, string name)
    //{
    //    var airplane = GameObject.Find(name);

    //    SetNewFlightData(airplane, origin, destination, name);
    //}
    //private void SetNewFlightData(GameObject aircraft, Vector3 origin, Vector4 destination, string name)
    //{
    //    var controller = aircraft.GetComponent<AirplaneController>();
    //    if (controller != null)
    //    {
    //        controller.SetNewFlightData(origin, destination, name, "detail");
    //    }
    //}
    //void Update()
    //{
    //    try
    //    {
    //        if (udpServer.Available > 0)
    //        {
    //            IPEndPoint dataServer = new IPEndPoint(IPAddress.Any, clientPort);
    //            byte[] udpInput = udpServer.Receive(ref dataServer);
    //            int index = 9;
    //            speed = System.BitConverter.ToSingle(udpInput, index);
    //            pitch = System.BitConverter.ToSingle(udpInput, index + 36);
    //            roll = -System.BitConverter.ToSingle(udpInput, index + 40);
    //            heading = System.BitConverter.ToSingle(udpInput, index + 44);
    //            altitude = System.BitConverter.ToSingle(udpInput, index + 80);
    //            lat = System.BitConverter.ToSingle(udpInput, index + 72);
    //            lon = System.BitConverter.ToSingle(udpInput, index + 76);

    //            float height = (((float)Convert.ToDouble(altitude) / 3.28084f) * 0.0007f);

    //            var flightName = "aaar";
    //            var flightCoords = new Vector4(lat, lon, 0.0f, speed);

    //            if (_airplanes.ContainsKey(flightName))
    //            {
    //                UpdateAirplane(tower, flightCoords, flightName);
    //            }
    //            if (!_airplanes.ContainsKey(flightName))
    //            {
    //                CreateAirplane(tower, flightCoords, 0, flightName);
    //            }
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e.Message.ToString());
    //    }
    //}
}
