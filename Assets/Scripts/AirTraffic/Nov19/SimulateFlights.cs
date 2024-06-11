using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using FlightDataService.DataObjects;
using System.Globalization;
using System.Diagnostics;
using UnityEngine.SceneManagement;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
#endif
public class SimulateFlights : MonoBehaviour
{
    public GameObject Airplane, waypoint;
    public TextMesh b, c;
    public string TopLevelName; /*= "Airport(New)";*/
    int counter;
    public float simulaionSpeed;
    private Dictionary<string, GameObject> _airplanes;
    Vector3 tower;
    private GameObject _topLevelObject;
    DateTime previous, current;
    Dictionary<string, Dictionary<string, Vector4>> flights;
    DateTime earliestFlightTime;
    int checkFlag = 0;

    private async void Start()
    {
        //DataClass.dataLoadedFlag = false;
        flights = new Dictionary<string, Dictionary<string, Vector4>>();
        earliestFlightTime = DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture);
        tower.x = 33.505194f;
        tower.y = 126.491972f;
        tower.z = 0;
        previous = DateTime.Now;
        current = DateTime.Now;
        _topLevelObject = GameObject.Find(TopLevelName);
        _airplanes = new Dictionary<string, GameObject>();

        //DataClass.currentSceneTrajectories = new Dictionary<string, Dictionary<string, Vector4>>(DataClass.trajectories);
        //DataClass.currentFlightsSquawkCodes = new Dictionary<string, string>(DataClass.flightsSquawkCodes);

        //temp.Clear();
        //DataClass.currentEarliestTime = DataClass.earliestTime;

        if(SceneManager.GetActiveScene().name == "MapView")
        {
            CreateAirplane(tower, new Vector4(33.74094444f, 126.2246944f, 0, 0), 0, "waypoint PC621");
            CreateAirplane(tower, new Vector4(33.72955556f, 126.1257222f, 0, 0), 0, "waypoint PC622");
            CreateAirplane(tower, new Vector4(33.6945f, 126.0350556f, 0, 0), 0, "waypoint PC623");
            CreateAirplane(tower, new Vector4(33.63886111f, 125.9606111f, 0, 0), 0, "waypoint PC624");
            CreateAirplane(tower, new Vector4(33.56752778f, 125.9088333f, 0, 0), 0, "waypoint PC625");
            CreateAirplane(tower, new Vector4(33.48663889f, 125.8841389f, 0, 0), 0, "waypoint PC626");
            CreateAirplane(tower, new Vector4(33.40325f, 125.8886389f, 0, 0), 0, "waypoint PC627");
            CreateAirplane(tower, new Vector4(33.32455556f, 125.9218056f, 0, 0), 0, "waypoint PC628");

            CreateAirplane(tower, new Vector4(34.25427778f, 126.6101667f, 0, 0), 0, "waypoint DOTOL");
            CreateAirplane(tower, new Vector4(33.99647222f, 126.5833889f, 0, 0), 0, "waypoint CHUJA");
            CreateAirplane(tower, new Vector4(33.90894444f, 126.4984444f, 0, 0), 0, "waypoint PC726");
            CreateAirplane(tower, new Vector4(33.914556f, 127.331389f, 0, 0), 0, "waypoint MAKET");
            CreateAirplane(tower, new Vector4(33.914806f, 126.786028f, 0, 0), 0, "waypoint PC721");
            CreateAirplane(tower, new Vector4(33.867472f, 126.63025f, 0, 0), 0, "waypoint PC722");
            CreateAirplane(tower, new Vector4(33.79275f, 126.3860556f, 0, 0), 0, "waypoint KEROM");
            CreateAirplane(tower, new Vector4(33.470972f, 127.331389f, 0, 0), 0, "waypoint TAMNA");
            CreateAirplane(tower, new Vector4(33.384611f, 126.624111f, 0, 0), 0, "waypoint CJU");
            CreateAirplane(tower, new Vector4(33.595639f, 126.494972f, 0, 0), 0, "waypoint PC723");
            CreateAirplane(tower, new Vector4(33.714444f, 126.421972f, 0, 0), 0, "waypoint PC724");
            CreateAirplane(tower, new Vector4(33.841667f, 124.414611f, 0, 0), 0, "waypoint TOLIS");
            CreateAirplane(tower, new Vector4(33.496556f, 125.831417f, 0, 0), 0, "waypoint ELDIN");
            CreateAirplane(tower, new Vector4(33.323667f, 126.482472f, 0, 0), 0, "waypoint PC725");
            CreateAirplane(tower, new Vector4(33.003333f, 126.771861f, 0, 0), 0, "waypoint TOSAN");
            CreateAirplane(tower, new Vector4(33.003333f, 126.459722f, 0, 0), 0, "waypoint SOSDO");
        }
    }

    private void CreateAirplane(Vector3 origin, Vector4 destination, int i, string name)
    {
        if(name.Contains("waypoint") == true)
        {
            var wp = Instantiate(waypoint);
            wp.transform.name = name;

            wp.SetActive(false);
            SetNewFlightData(wp, origin, destination, name);
        }
        else
        {
            //foreach (var flight in flights)
            //{
            var airplane = Instantiate(Airplane);
            airplane.transform.name = name;
            //airplane.transform.parent = _topLevelObject.transform;

            if (SceneManager.GetActiveScene().name == "ModelExplorer")
            {
                airplane.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            }

            //GameObject ab = this.transform.GetChild(0).gameObject;
            //TextMesh l = ab.GetComponent<TextMesh>();
            //l.text = name;
            //ab.transform.LookAt(Camera.main.transform);

            airplane.SetActive(false);
            SetNewFlightData(airplane, origin, destination, name);
            _airplanes.Add(name, airplane);
            //a.text = name + " created";
            //}
        }
    }
    private void UpdateAirplane(Vector3 origin, Vector4 destination, string name)
    {
        var airplane = GameObject.Find(name);

        AirplaneController playerScript = airplane.GetComponent<AirplaneController>();
        playerScript.latLong = new Vector2(destination.x,destination.y);

        SetNewFlightData(airplane, origin, destination, name);
    }
    private void SetNewFlightData(GameObject aircraft, Vector3 origin, Vector4 destination, string name)
    {
        if (name.Contains("waypoint") == true)
        {
            var controller = aircraft.GetComponent<AirplaneController>();
            if (controller != null)
            {
                if (SceneManager.GetActiveScene().name == "MapView")
                {
                    controller.SetNewFlightData(origin, destination, name, "waypoint");
                }
                else
                {

                }
            }
        }
        else
        {
            var controller = aircraft.GetComponent<AirplaneController>();
            if (controller != null)
            {
                if (SceneManager.GetActiveScene().name == "MapView")
                {
                    controller.SetNewFlightData(origin, destination, name, "map");
                }
                else
                {
                    controller.SetNewFlightData(origin, destination, name, "detail");
                }
            }
        }
    }
    void Update()
    {
        try
        {
            if(DataClass.dataLoadedFlag == true)
            {
                if(checkFlag == 0)
                {
                    //flights = DataClass.trajectories;
                    flights = DataClass.trajectories;
                    //flights = DataClass.currentSceneTrajectories;
                    //earliestFlightTime = DataClass.currentEarliestTime;
                    earliestFlightTime = DataClass.earliestTime;
                    checkFlag = 1;
                }
                current = DateTime.Now;
                TimeSpan ts = (current - previous);
                float timeDifference = (float)ts.TotalMilliseconds;

                if (timeDifference >= simulaionSpeed)
                {
                    for (int i = 0; i < flights.Count; i++)
                    {
                        var flightName = flights.ElementAt(i).Key;
                        var flightData = flights.ElementAt(i).Value;
                        //UnityEngine.Debug.Log(i.ToString());
                        //for 19nov data
                        string tpTime = earliestFlightTime.ToString("HH:mm:ss");
                        string[] time = (tpTime).ToString().Split(' ');
                        //
                        //string[] time = (earliestFlightTime).ToString().Split(' ');
                        string[] tempTime = new string[3];
                        try
                        {
                            tempTime = time[0].Split(':');
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.Log(e.Message);
                        }
                        //UnityEngine.Debug.Log("before");
                        if (Convert.ToInt16(tempTime[0]) < 10)
                        {
                            if (tempTime[0].Contains('0'))
                            {

                            }
                            else
                            {
                                tempTime[0] = "0" + tempTime[0];
                                time[1] = tempTime[0] + ":" + tempTime[1] + ":" + tempTime[2];
                            }
                        }
                        //UnityEngine.Debug.Log(time[0]);
                        //at all places time[1] g=for may 1
                        if (flightData.ContainsKey(time[0]))
                        {
                            Vector4 flightCoords = new Vector4();
                            flightCoords = flightData[time[0]];
                            //UnityEngine.Debug.Log("Flight Coords: " + flightCoords.ToString() + " Flight Name: " + flightName);
                            if (_airplanes.ContainsKey(flightName))
                            {
                                UpdateAirplane(tower, flightCoords, flightName);
                                //UnityEngine.Debug.Log("Plane updated");
                            }
                            if (!_airplanes.ContainsKey(flightName))
                            {
                                CreateAirplane(tower, flightCoords, 0, flightName);
                                UnityEngine.Debug.Log("Plane created");
                            }
                        }
                    }
                    earliestFlightTime = earliestFlightTime.AddSeconds(1);
                    UnityEngine.Debug.Log(earliestFlightTime);
                    //DataClass.currentEarliestTime = earliestFlightTime;
                    DataClass.earliestTime = earliestFlightTime;
                    previous = current;
                }
            }
            else
            {
                checkFlag = 0;
            }
        }
        catch (Exception e)
        {
            //UnityEngine.Debug.Log(e.Source.ToString());
            //UnityEngine.Debug.Log(e.Message.ToString());
            //UnityEngine.Debug.Log(e.InnerException.ToString());
            //// Get stack trace for the exception with source file information
            //var st = new StackTrace(e, true);
            //// Get the top stack frame
            //var frame = st.GetFrame(0);
            //// Get the line number from the stack frame
            //var line = frame.GetFileLineNumber();

            //UnityEngine.Debug.Log("line " + line);
        }
    }

    public void deleteAirplane(string flightName)
    {
        string name = (flightName + " " + DataClass.flightsSquawkCodes[flightName]);
        //flights.Remove(name);
        GameObject objectToDelete = GameObject.Find(name);
        //DataClass.currentSceneTrajectories.Remove(name);
        DataClass.trajectories.Remove(name);
    }
}