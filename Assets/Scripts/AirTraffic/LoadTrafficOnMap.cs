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
public class LoadTrafficOnMap : MonoBehaviour
{
    public GameObject Airplane;
    public TextMesh a, b, c;
    public string TopLevelName = "Airport";
    int counter;
    public float simulaionSpeed;
    private Dictionary<string, GameObject> _airplanes;

    //    private readonly TimeSpan _waitTime = TimeSpan.FromSeconds(5);

    //    private DateTimeOffset _lastUpdate = DateTimeOffset.MinValue;

    //    //private Queue<FlightSet> _receivedData;

    private GameObject _topLevelObject;
    DateTime previous, current;
    //    // Use this for initialization
    Vector3  upperRightCorner, tower, lowerRightCorner, upperLeftCorner, lowerLeftCorner;
    //List<string> lines;
    Vector4 newUpperRightCorner, newUpperLeftfCorner;
    //List<Vector3> flightCoordinates;
    //List<List<Vector3>> flights;

    Dictionary<string, Vector4> flightCoordinates;
    Dictionary<string, Dictionary<string, Vector4>> flights;

    string[] filenames;
    DateTime earliestFlightTime;
    //#if WINDOWS_UWP
    private async void Start()
    {
        try
        {
            System.Random random = new System.Random();
            int min = 1;
            int max = 7;

            int randNum1 = random.Next(min, max);
            int randNum2 = random.Next(min, max);
            int randNum3 = random.Next(min, max);
            int randNum4 = random.Next(min, max);

            string squawk = randNum1.ToString() + randNum2.ToString() + randNum3.ToString() + randNum4.ToString();

            b.text = "Loading Trajectories. Please wait until this text disappears...";
            earliestFlightTime = new DateTime();
            //lines = new List<string>();

            previous = DateTime.Now;
            current = DateTime.Now;
            flights = new Dictionary<string, Dictionary<string, Vector4>>();
            //flightCoordinates = new List<Vector3>();

            filenames = new string[11];

            counter = 1;

            //30 april arrivals only
            //filenames[0] = "7C509_20414ce0_traj";
            //filenames[1] = "KE1953_2041490f_traj";
            //filenames[2] = "MUSE_traj_GoAround";
            //filenames[3] = "RWY_STOP_test";
            //filenames[4] = "TW244_20412ef2_traj";
            //filenames[5] = "TW903_20414f07_traj";


            //30 april arrivals only
            //filenames[0] = "7C509_20414ce0_traj";
            //filenames[1] = "KE1222_20414a9e_traj";
            //filenames[2] = "KE1953_2041490f_traj";
            //filenames[3] = "LJ318_20415dc1_traj";
            //filenames[4] = "LJ584_20415be9_traj";
            //filenames[5] = "MUSE_traj_GoAround";
            //filenames[6] = "OZ8924_2041614f_traj";
            //filenames[7] = "RWY_STOP_test";
            //filenames[8] = "TW244_20412ef2_traj";
            //filenames[9] = "TW903_20414f07_traj";
            //filenames[10] = "ZE214_20415e99_traj";

            //may 1
            filenames[0] = "7C509_20414ce0_traj";
            filenames[1] = "KE1222_20414a9e_traj";
            filenames[2] = "KE1953_2041490f_traj";
            filenames[3] = "ZE214_20415e99_traj";
            filenames[4] = "LJ584_20415be9_traj";
            filenames[5] = "MUSE_traj_GoAround";
            filenames[6] = "OZ8924_2041614f_traj";
            filenames[7] = "TW903_20414f07_traj";
            filenames[8] = "TW244_20412ef2_traj";
            filenames[9] = "RWY_STOP_test_traj";
            filenames[10] = "LJ318_20415dc1_traj";

            //may 9
            //filenames[0] = "7C509_20414ce0_traj";
            //filenames[1] = "KE1222_20414a9e_traj";
            //filenames[2] = "KE1953_2041490f_traj";
            //filenames[3] = "LJ318_20415dc1_traj";
            //filenames[4] = "LJ584_20415be9_traj";
            //filenames[5] = "MUSE_traj_GoAround";
            //filenames[6] = "OZ8924_2041614f_traj";
            //filenames[7] = "TW903_20414f07_traj";
            //filenames[8] = "TW244_20412ef2_traj";
            //filenames[9] = "RWY_STOP_test_traj";
            //filenames[10] = "ZE214_20415e99_traj";

            //newFiles 16 April
            //filenames[0] = "9C8625_1e9ac7f2_traj";
            //filenames[1] = "9C8913_1e9ac359_traj";
            //filenames[2] = "FE722_1e9ab05a_traj";
            //filenames[3] = "KE1237_1e9da1b3_traj";
            //filenames[4] = "MU5028_1e9dbcdb_traj";
            //filenames[5] = "OZ8995_1e9ad393_traj";
            //filenames[6] = "ZE228_1e9dc71c_traj";
            //filenames[7] = "ZE230_1e9dbc2f_traj";
            //filenames[8] = "ZE551_1e9dbd3a_traj";
            //filenames[9] = "ZE706_1e9dc682_traj";
            //filenames[10] = "GP_traj";
            //filenames[0] = "RWY_STOP";
            //filenames[0] = "AAR8995_1e9ad3931_traj";
            //filenames[1] = "CES5028_1e9dbcdb_traj";
            //filenames[2] = "CQH8913_1e9ac3591_traj";
            //filenames[3] = "ESR228_1e9dc71c_traj";
            //filenames[4] = "ESR230_1e9dbc2f_traj";
            //filenames[5] = "ESR551_1e9dbd3a_traj";
            //filenames[6] = "ESR706_1e9dc682_traj";
            //filenames[7] = "FEA722_1e9ab05a1_traj";
            //filenames[8] = "KAL1237_1e9da1b31_traj";


            //string[] lines = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\"+filenames[3]);


            #region coords
            lowerRightCorner.x = 32.206510f;
            lowerRightCorner.y = 128.153088f;
            lowerRightCorner.z = 0;
            #endregion
            upperRightCorner.x = 34.677486f;
            upperRightCorner.y = 128.153088f;
            upperRightCorner.z = 0;

            newUpperLeftfCorner.x = 36.551281f;
            newUpperLeftfCorner.y = 124.696947f;
            newUpperLeftfCorner.z = 0;
            newUpperLeftfCorner.w = 1.1f;

            newUpperRightCorner.x = 36.551281f;
            newUpperRightCorner.y = 133.409981f;
            newUpperRightCorner.z = 0;
            newUpperRightCorner.w = 1.1f;

            upperLeftCorner.x = 34.677486f;
            upperLeftCorner.y = 125.182356f;
            upperLeftCorner.z = 0;

            lowerLeftCorner.x = 32.206510f;
            lowerLeftCorner.y = 125.182356f;
            lowerLeftCorner.z = 0;

            tower.x = 33.506015f;
            tower.y = 126.525843f;
            tower.z = 0;

            _topLevelObject = GameObject.Find(TopLevelName);
            
            int j = 0;
            earliestFlightTime = DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture);
            //if(DataClass.trajectories == null)
            //{
                //earliestFlightTime = DateTime.ParseExact("23:59:59.000", "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                foreach (string file in filenames)
                {
                    string oldDate = string.Empty;
                    string[] fileName = file.Split('_');

                    flightCoordinates = new Dictionary<string, Vector4>();

                    DateTime startDateTime = new DateTime();

#if WINDOWS_UWP
                //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\NewFiles\\30April\\EditedShort\\" + file);
                //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\NewFiles\\May1\\" + file);
                //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\NewFiles\\May9\\" + file);

                List<string> data = new List<string>();
                data = await getData(file, j);
                j++;

                int i = 0;
                //b.text = (b.text + lines.First());

                foreach (string line in data)//for (int i = 0; i < lines.Length; i++)
                {
                    //c.text = "Called foreach";
                    string[] tokenized = line.Split(',');//lines[i].Split(',');
                                                         //b.text = (b.text + line);
                    int tempHeading = Convert.ToInt32(tokenized[5]);

                    if (tempHeading < 0)
                    {
                        tempHeading += 360;
                    }

                    tokenized[5] = tempHeading.ToString();

                    if (tokenized.Length > 1)
                    {
                        tokenized[0] = "0" + tokenized[0];
                        DateTime dt = DateTime.ParseExact(tokenized[0], "HH:mm:ss", CultureInfo.InvariantCulture);
                        //Debug.Log("Last " + "Filename" + file + " " + tokenized[0]);
                        //DateTime dt = DateTime.ParseExact(tokenized[0], "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        if (i == 0)
                        {
                            startDateTime = dt;
                            i++;
                        }

                        float height = (((float)Convert.ToDouble(tokenized[3]) / 3.28084f) * /*old scale 0.00000363f*/ /*new scale 0.00000139f*/ /*/*test*/ 0.00000278f);

                        string head = tokenized[5].ToString();
                        head = (tokenized[4].ToString() + "." + head);

                        head = head.Replace(" ", string.Empty);

                        tokenized[5] = head;
                        double a = Convert.ToDouble(head);
                        float b = Convert.ToSingle(a);
                        a = Convert.ToDouble(b);

                        //5th index for heading 4th for speed
                        Vector4 flightCoordinate = new Vector4((float)Convert.ToDouble(tokenized[1]), (float)Convert.ToDouble(tokenized[2]), height, (float)Convert.ToDouble(tokenized[5]));

                        //CreateAircraft(tower, flightCoordinate,i ,"");
                        string[] time = (earliestFlightTime).ToString().Split(' ');
                        if (oldDate.ToString() != tokenized[0])
                        {
                            if (flightCoordinates.ContainsKey(tokenized[0]) != true)
                            {
                                flightCoordinates.Add(tokenized[0], flightCoordinate);
                                oldDate = tokenized[0];
                            }                            
                        }
                    }
                }
#endif

                    if (startDateTime < earliestFlightTime)
                    {
                        earliestFlightTime = startDateTime;
                    }

                    int code = Convert.ToInt32(squawk);
                    fileName[0] += " " + (code + 1).ToString();

                    flights.Add(fileName[0], flightCoordinates);
                }
            //    DataClass.trajectories = flights;
            //    DataClass.earliestTime = earliestFlightTime;
            //    a.text = "Data loaded from scratch " + DataClass.trajectories.Count.ToString() + "/" + flights.Count.ToString();
            //}
            //else
            //{
            //    flights = DataClass.trajectories;
            //    earliestFlightTime = DataClass.earliestTime;
            //    a.text = "Data loaded from data class " + DataClass.trajectories.Count.ToString() + "/" + flights.Count.ToString();
            //}
            
            b.text = "";
            _airplanes = new Dictionary<string, GameObject>();

            //CreateAirplane(tower, newUpperRightCorner, 0, "test");
            //CreateAirplane(tower, newUpperLeftfCorner, 0, "test1");
            //CreateAirplane(tower, lowerRightCorner, 0, "flight1");
            //CreateAirplane(tower, upperLeftCorner, 0, "flight2");
            //CreateAirplane(tower, lowerLeftCorner, 0, "flight3");
            //CreateAirplane(tower, new Vector3(rw31endpoint.x, rw31endpoint.y, 0), 0, "flight1");
        }
        catch (Exception e)
        {
            a.text = e.Message.ToString();
        }
    }
#if WINDOWS_UWP
    private async Task<List<string>> getData(string fileName, int j)
    {
        try
        {
            //9C8625_1e9ac7f2.csv_traj
            //a.text = (a.text + "Get Data: " + j.ToString());
            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder localFolder = Windows.Storage.KnownFolders.CameraRoll;

            //Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(fileName);

            var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);

            //string[] data =  new string[readFile.Count];
            //int i = 0;
            //foreach (var line in readFile)
            //{
            //    data[i] = line;
            //    i++;
            //}
            
            //lines = new List<string>(readFile);
            
            return new List<string>(readFile);
            //b.text = (b.text + lines.First());
        }
        catch (Exception e)
        {
            a.text = (string)e.Message.ToString() + " in get data func";
            return null;
        }
    }
#endif
    private void CreateAirplane(Vector3 origin, Vector4 destination, int i, string name)
    {
        var airplane = Instantiate(Airplane);
        airplane.transform.name = name;
        airplane.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        airplane.SetActive(false);
        SetNewFlightData(airplane, origin, destination, name);
        airplane.SetActive(true);
        _airplanes.Add(name, airplane);
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
            controller.SetNewFlightData(origin, destination, name, "map");
        }
    }
    void Update()
    {
        try
        {
            current = DateTime.Now;
            TimeSpan ts = (current - previous);
            float timeDifference = (float)ts.TotalMilliseconds;

            if (timeDifference >= simulaionSpeed)
            {
                for (int i = 0; i < flights.Count; i++)
                {
                    var flightName = flights.ElementAt(i).Key;
                    var flightData = flights.ElementAt(i).Value;
                    string[] time = (earliestFlightTime).ToString().Split(' ');

                    
                    string[] tempTime = time[1].Split(':');

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

                    if (flightData.ContainsKey(time[1]))
                    {
                        Vector4 flightCoords = new Vector4();
                        flightCoords = flightData[time[1]];

                        if (_airplanes.ContainsKey(flightName))
                        {
                            UpdateAirplane(tower, flightCoords, flightName);
                        }
                        if (!_airplanes.ContainsKey(flightName))
                        {
                            CreateAirplane(tower, flightCoords, 0, flightName);
                        }
                    }
                }
                earliestFlightTime = earliestFlightTime.AddSeconds(1);
                //earliestFlightTime = earliestFlightTime.AddMilliseconds(50);
                previous = current;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
