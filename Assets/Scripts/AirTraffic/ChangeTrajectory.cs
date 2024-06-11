using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTrajectory : MonoBehaviour
{
    public string command;
    AirplaneLocation locationObj;
    double[] gpsUtils;
    string airplane, lessDistanceIndex;
    Dictionary<string, Vector2> wayPoints;
    HandleVoiceCommands hvc;
	// Use this for initialization
	void Start ()
    {
        hvc = new HandleVoiceCommands();

        locationObj = new AirplaneLocation();
        gpsUtils = new double[6];
        airplane = String.Empty;
        lessDistanceIndex = String.Empty;

        wayPoints = new Dictionary<string, Vector2>();
        wayPoints.Add("PC621", new Vector2(33.74094444f, 126.2246944f));
        wayPoints.Add("PC622", new Vector2(33.72955556f, 126.1257222f));
        wayPoints.Add("PC623", new Vector2(33.6945f, 126.0350556f));
        wayPoints.Add("PC624", new Vector2(33.63886111f, 125.9606111f));
        wayPoints.Add("PC625", new Vector2(33.56752778f, 125.9088333f));
        wayPoints.Add("PC626", new Vector2(33.48663889f, 125.8841389f));
        wayPoints.Add("PC627", new Vector2(33.40325f, 125.8886389f));

        command = command.Replace(" ", String.Empty);
        command = command.ToLower();

        if (command.Contains("707"))
        {
            airplane = "Aircraft04";
        }
        else if(command.Contains("601"))
        {
            airplane = "Aircraft06";
        }
        else if (command.Contains("209"))
        {
            airplane = "Aircraft08";
        }
        else if (command.Contains("1901"))
        {
            airplane = "Aircraft10";
        }
        else if (command.Contains("851"))
        {
            airplane = "Aircraft12";
        }
        else if (command.Contains("1005"))
        {
            airplane = "Aircraft14";
        }
        else if (command.Contains("1213"))
        {
            airplane = "Aircraft18";
        }
        else if (command.Contains("309"))
        {
            airplane = "Aircraft20";
        }

        if (airplane != String.Empty)
        {
            hvc.updateTrajectory(airplane);
            //DataClass.dataLoadedFlag = false;
            //string name = (airplane + " " + DataClass.flightsSquawkCodes[airplane]);
            ////flights.Remove(name);
            //GameObject objectToDelete = GameObject.Find(name);
            //if(objectToDelete != null)
            //{
            //    AirplaneController playerScript = objectToDelete.GetComponent<AirplaneController>();
            //    Vector2 currentLocaionOfPlane = new Vector2(playerScript.latLong.x, playerScript.latLong.y);
            //    double lastDistance = 0.0;
            //    for (int i = 1; i <= 7; i++)
            //    {
            //        Vector2 tempWaypoint = new Vector2();
            //        tempWaypoint = wayPoints["PC62" + i];
            //        gpsUtils = locationObj.GeodeticToEnu(currentLocaionOfPlane.x, currentLocaionOfPlane.y, 0, tempWaypoint.x, tempWaypoint.y, 0);

            //        if (i == 1)
            //        {
            //            lastDistance = gpsUtils[6];
            //            lessDistanceIndex = "PC62" + i;
            //        }
            //        else
            //        {
            //            if (lastDistance > gpsUtils[6])
            //            {
            //                lastDistance = gpsUtils[6];
            //                lessDistanceIndex = "PC62" + i;
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //    }

            //    GameObject.Destroy(objectToDelete);
            //    //DataClass.currentSceneTrajectories.Remove(name);
            //    DataClass.trajectories.Remove(name);
            //    DataClass.flightsSquawkCodes.Remove(airplane);
            //    //DataClass.currentFlightsSquawkCodes.Remove(airplane);
            //    LoadAirTraffic obj = new LoadAirTraffic();
            //    UnityEngine.Debug.Log("New Loaded Flight " + airplane + "_traj_" + lessDistanceIndex);
            //    obj.loadingFlightData(airplane + "_traj_" + lessDistanceIndex);
            //    DataClass.dataLoadedFlag = true;
            //}
            //else
            //{
            //    DataClass.dataLoadedFlag = true;
            //    UnityEngine.Debug.Log("Flight does not exist!");
            //}
        }
        else
        {
            UnityEngine.Debug.Log("Could not recognize command!\nPlease speak again");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
//old
//DataClass.dataLoadedFlag = false;
//            string name = ("Aircraft06" + " " + DataClass.flightsSquawkCodes["Aircraft06"]);
////flights.Remove(name);
//GameObject objectToDelete = GameObject.Find(name);

//AirplaneController playerScript = objectToDelete.GetComponent<AirplaneController>();
//Vector2 currentLocaionOfPlane = new Vector2(playerScript.latLong.x, playerScript.latLong.y);

//GameObject.Destroy(objectToDelete);
//            DataClass.currentSceneTrajectories.Remove(name);
//            DataClass.flightsSquawkCodes.Remove(DataClass.flightsSquawkCodes["Aircraft06"]);
//            LoadAirTraffic obj = new LoadAirTraffic();
//obj.loadingFlightData("Aircraft 06_traj_PC627");
//            DataClass.dataLoadedFlag = true;