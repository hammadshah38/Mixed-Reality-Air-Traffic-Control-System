using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.Assertions;
using System;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

//#if Windows_UWP
////using Windows.Devices.Sensors;
////using Windows.Devices.Geolocation;
//using Windows.Devices.Sensors;
//using Windows.Foundation;
//using Windows.UI.Core;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Navigation;
//#endif

public class GazeUsingHands : MonoBehaviour
{

    /// <summary>
    /// HandDetected tracks the hand detected state.
    /// Returns true if the list of tracked hands is not empty.
    /// </summary>
    public bool HandDetected
    {
        get { return trackedHands.Count > 0; }
    }

    private bool pressed;
    private HashSet<uint> trackedHands = new HashSet<uint>();
    private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
    private uint activeId;
    private GameObject manObj, cam;
    private GazeManager gazeObj;
    public GameObject cursor;

    public TextMesh Info, Info1, Info2;
//#if Windows_UWP
////Compass _compass;
//    Magnetometer _magnetometer;
//#endif
    void Awake()
    {
//#if Windows_UWP
////_compass = Compass.GetDefault();
////var status = await Geolocator.RequestAccessAsync();
////if (status == GeolocationAccessStatus.Allowed)
////{
////    var locator = new Geolocator();
////    var position = await locator.GetGeopositionAsync();
////    var lat = position.Coordinate.Latitude;
////    var lon = position.Coordinate.Longitude;
////}
//        _magnetometer = Magnetometer.GetDefault();
//            if (_magnetometer != null)
//            {
//                // Select a report interval that is both suitable for the purposes of the app and supported by the sensor.
//                // This value will be used later to activate the sensor.
//                //uint minReportInterval = _magnetometer.MinimumReportInterval;
//                //_desiredReportInterval = minReportInterval > 16 ? minReportInterval : 16;
//                Info1.text = "Magnetometer found";
//            }
//            else
//            {
//                Info1.text = "No magnetometer found";
//            }
//#endif
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;


        manObj = GameObject.Find("InputManager");
        cam = GameObject.Find("MixedRealityCamera");

        gazeObj = new GazeManager();

        gazeObj = (GazeManager)manObj.GetComponent<GazeManager>();
        gazeObj.GazeTransform = cam.transform;
    }
    private void Start()
    {

    }

    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        uint id = args.state.source.id;
        gazeObj.GazeTransform = this.transform;
        gazeObj.OnPreRaycast();

        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }
        trackedHands.Add(id);
        activeId = id;
        Vector3 pos, objPos = new Vector3();
        Quaternion rot, objRot = new Quaternion();
        if (args.state.sourcePose.TryGetPosition(out pos))
        {
            Vector3 updatedPos = new Vector3(pos.x, pos.y, pos.z + 1.0f);
            objPos = updatedPos;
            this.transform.position = updatedPos;
        }
        if (args.state.sourcePose.TryGetRotation(out rot))
        {
            this.transform.rotation = rot;
            objRot = rot;
        }
    }
    Vector3 oldHandGestureDirection = new Vector3();
    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        uint id = args.state.source.id;
        Vector3 pos;
        Quaternion rot;

        if (args.state.source.kind == InteractionSourceKind.Hand)
        {
            var newHandGestureDirection = new Vector3();
            if (args.state.sourcePose.TryGetPosition(out pos))
            {
                newHandGestureDirection = pos - Camera.main.transform.position;
                oldHandGestureDirection = newHandGestureDirection;

                Vector3 CamPos = pos;
                //Vector3 updatedCamPos = new Vector3(CamPos.x - 0.2f, CamPos.y + 0.4f, CamPos.z);

                Vector3 updatedCamPos = new Vector3(CamPos.x, CamPos.y + 0.4f, CamPos.z);

                this.transform.position = updatedCamPos;//+ (handGestureDirection.normalized * 0.3F);

                Info.text = "x: " + this.transform.position.x + ", y: " + this.transform.position.y + ", z: " + this.transform.position.z;
                this.transform.rotation = Quaternion.LookRotation(newHandGestureDirection);
                //this.transform.forward = newHandGestureDirection.normalized;
            }
            if (args.state.sourcePose.TryGetRotation(out rot))
            {
                this.transform.rotation = rot;
                this.transform.LookAt(Camera.main.transform);
            }
        }
    }
    private void Update()
    {

    }
    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        uint id = args.state.source.id;

        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        if (trackedHands.Contains(id))
        {
            trackedHands.Remove(id);
        }

        if (trackingObject.ContainsKey(id))
        {
            var obj = trackingObject[id];
            trackingObject.Remove(id);
            Destroy(obj);
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
        }
        gazeObj.GazeTransform = cam.transform;
    }
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        pressed = true;

    }
    private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs args)
    {
        pressed = false;
    }
    void OnDestroy()
    {
        InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed -= InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceReleased -= InteractionManager_InteractionSourceReleased;
    }
}
