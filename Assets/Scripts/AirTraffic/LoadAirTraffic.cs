using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using FlightDataService.DataObjects;
using System.Globalization;
using System.Diagnostics;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
#endif
public class LoadAirTraffic : MonoBehaviour
{
    public GameObject Airplane, ground;
    bool isGroundActive;
    public TextMesh status, b,c;
    Dictionary<string, string> squawks;
    int counter;
    public float simulaionSpeed;
    private Dictionary<string, GameObject> _airplanes;
    public GameObject simulation;

    DateTime previous, current;
    Dictionary<string,Vector4> flightCoordinates;
    Dictionary<string, Dictionary<string, Vector4>> flights;

    string[] filenames;
    DateTime earliestFlightTime;
    int checkCounter=0;
//#if WINDOWS_UWP
    private async void Start()
    {
        try
        {
            isGroundActive = false;
            if (DataClass.dataLoadedFlag == false)
            {
                status.text = "Loading Data!";
                //DataClass.dataLoadedFlag = false;
                Application.targetFrameRate = 300;
                System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                int min = 1;
                int max = 7;
                squawks = new Dictionary<string, string>();
                //int randNum1 = random.Next(min, max);
                ////random = new System.Random();
                //int randNum2 = random.Next(min, max);
                ////random = new System.Random();
                //int randNum3 = random.Next(min, max);
                ////random = new System.Random();
                //int randNum4 = random.Next(min, max);

                //string squawk = randNum1.ToString() + randNum2.ToString() + randNum3.ToString() + randNum4.ToString();
                string squawk = "";
                b.text = "Loading Trajectories. Please wait until this text disappears...";
                earliestFlightTime = new DateTime();
                //lines = new List<string>();

                previous = DateTime.Now;
                current = DateTime.Now;
                flights = new Dictionary<string, Dictionary<string, Vector4>>();
                //flightCoordinates = new List<Vector3>();

                filenames = new string[16];

                counter = 1;

                //may 1 latest
                //filenames[0] = "7C509_20414ce0_traj";
                //filenames[1] = "KE1222_20414a9e_traj";
                //filenames[2] = "KE1953_2041490f_traj";
                //filenames[3] = "ZE214_20415e99_traj";
                //filenames[4] = "LJ584_20415be9_traj";
                //filenames[5] = "MUSE_traj_GoAround";
                //filenames[6] = "OZ8924_2041614f_traj";
                //filenames[7] = "TW903_20414f07_traj";
                //filenames[8] = "TW244_20412ef2_traj";
                //filenames[9] = "RWY_STOP_test_traj";
                //filenames[10] = "LJ318_20415dc1_traj";

                //final demo 19Nov data
                //filenames[0] = "JNA562_traj";
                //filenames[1] = "TWB707_traj_PC628";
                //filenames[2] = "TWB904_traj";
                //filenames[3] = "JJA601_traj_PC628";
                //filenames[4] = "JJA108_traj";
                //filenames[5] = "ESR209_traj_PC628";
                //filenames[6] = "KAL1901_traj_PC628";
                //filenames[7] = "JJA142_traj";
                //filenames[8] = "JJA851_traj_PC628";
                //filenames[9] = "KAL1216_traj";
                //filenames[10] = "KAL1005_traj_PC628";
                //filenames[11] = "KAL1213_traj_PC628";
                //filenames[12] = "JJA504_traj";
                //filenames[13] = "JNA309_traj_PC628";
                //filenames[14] = "JNA584_traj";

                //191214
                filenames[0] = "Aircraft01_traj";
                filenames[1] = "Aircraft03_traj";
                filenames[2] = "Aircraft04_traj_PC628";
                filenames[3] = "Aircraft05_traj";
                filenames[4] = "Aircraft06_traj_PC628";
                filenames[5] = "Aircraft07_traj";
                filenames[6] = "Aircraft08_traj_PC628";
                filenames[7] = "Aircraft10_traj_PC628";
                filenames[8] = "Aircraft11_traj";
                filenames[9] = "Aircraft12_traj_PC628";
                filenames[10] = "Aircraft13_traj";
                filenames[11] = "Aircraft14_traj_PC628";
                filenames[12] = "Aircraft18_traj_PC628";
                filenames[13] = "Aircraft19_traj";
                filenames[14] = "Aircraft20_traj_PC628";
                filenames[15] = "Aircraft21_traj";

                //video recording
                //filenames[0] = "Aircraft03_traj";
                //filenames[1] = "Aircraft08_traj_PC628";

                int j = 0;
                earliestFlightTime = DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture);
                
                foreach (string file in filenames)
                {
                    int randNum1 = random.Next(min, max);
                    //random = new System.Random();
                    int randNum2 = random.Next(min, max);
                    //random = new System.Random();
                    int randNum3 = random.Next(min, max);
                    //random = new System.Random();
                    int randNum4 = random.Next(min, max);

                    squawk = randNum1.ToString() + randNum2.ToString() + randNum3.ToString() + randNum4.ToString();

                    string oldDate = string.Empty;
                    string[] fileName = file.Split('_');
                    fileName[0] = fileName[0].Replace(" ", String.Empty);
                    flightCoordinates = new Dictionary<string, Vector4>();

                    DateTime startDateTime = new DateTime();

                    //#if WINDOWS_UWP
                    //for new data 1 MAY
                    //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\NewFiles\\30April\\" + file);
                    //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\NewFiles\\May1\\" + file);

                    //for final demo data 19 Nov
                    //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\Flight Trajectories19nov\\" + file);

                    //for final demo data 05 Dec
                    //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\Flight Trajectories05dec\\" + file);

                    //(heading altitude debug)
                    //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\191214\\" + file);
                    List<string> data = new List<string>();
#if WINDOWS_UWP
                                                            data = await getData(file, j);
                                                            j++;
#endif
                    int i = 0;
                    //b.text = (b.text + lines.First());
                    try
                    {
                        foreach (string line in data)//for (int i = 0; i < lines.Length; i++)
                        {
                            //c.text = "Called foreach";
                            string[] tokenized = line.Split(',');//lines[i].Split(',');
                                                                 //b.text = (b.text + line);


                            //for 19 nov data for excluding 60th second
                            string[] checkSecond = tokenized[0].Split(':');
                            if (checkSecond[2] != "60")
                            {
                                int tempHeading = Convert.ToInt32(tokenized[5]);

                                tempHeading = (int)ClampAngle((float)tempHeading);
                                if (tempHeading <= 0)
                                {
                                    tempHeading += 360;
                                    //tempHeading *= -1;
                                }
                                //if (tempHeading == 0)
                                //{
                                //    tempHeading = 360;
                                //}

                                tokenized[5] = tempHeading.ToString();

                                if (tokenized.Length > 1)
                                {
                                    tokenized[0] = "0" + tokenized[0];
                                    //UnityEngine.Debug.Log(file + " " + line + " " + tokenized[0]);
                                    DateTime dt = DateTime.ParseExact(tokenized[0], "HH:mm:ss", CultureInfo.InvariantCulture);

                                    if (i == 0)
                                    {
                                        startDateTime = dt;
                                        i++;
                                    }

                                    //float height = (((float)Convert.ToDouble(tokenized[3]) / 3.28084f) * 0.0007f);

                                    //nov 19 data
                                    float height = (float)Convert.ToDouble(tokenized[3]);
                                    //

                                    string head = tokenized[5].ToString();
                                    string[] neghead = head.Split('-');

                                    if (neghead.Length > 1)
                                    {
                                        head = neghead[1];
                                    }

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
                                checkCounter++;
                            }
                        }
                        //#endif
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.Log(e.ToString());
                    }


                    if (startDateTime < earliestFlightTime)
                    {
                        earliestFlightTime = startDateTime;
                        DataClass.earliestTime = earliestFlightTime;
                    }
                    int code = Convert.ToInt32(squawk);



                    //if(fileName.Length == 2)
                    //{
                    //    squawks.Add(fileName[0], code.ToString());
                    //    fileName[0] += " " + (code).ToString();                        
                    //}
                    //else
                    //{
                    //    squawks.Add(fileName[0] + fileName[2], code.ToString());
                    //    fileName[0] += fileName[2] + " " + (code).ToString();
                    //}
                    squawks.Add(fileName[0], code.ToString());
                    fileName[0] += " " + (code).ToString();
                    flights.Add(fileName[0], flightCoordinates);
                    UnityEngine.Debug.Log(fileName[0]);
                }
                
                b.text = "";
                DataClass.trajectories = flights;
                
                DataClass.dataLoadedFlag = true;
                DataClass.flightsSquawkCodes = squawks;
                UnityEngine.Debug.Log("Data Loaded");

                //for no marker map
                status.text = "";
                simulation.SetActive(true);
                isGroundActive = true;
                //simulation.SetActive(true);
            }
            //for no marker
            else
            {
                status.text = "";
                simulation.SetActive(true);
                isGroundActive = true;
            }
        }
        catch (Exception e)
        {
            int k = checkCounter;
            UnityEngine.Debug.Log(e.Message.ToString());
            //a.text = e.Message.ToString();
        }
    }
#if WINDOWS_UWP
    public async Task<List<string>> getData(string fileName, int j)
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
            //a.text = (string)e.Message.ToString() + " in get data func";
            return null;
        }
    }
#endif
    
    void Update()
    {
        //if (isGroundActive == false)
        //{
        //    if(ground.active == true && DataClass.dataLoadedFlag == true)
        //    {
        //        simulation.SetActive(true);
        //        isGroundActive = true;
        //    }
        //}
    }
    public static float ClampAngle(float _Angle)
    {
        float ReturnAngle = _Angle;

        if (_Angle < 0f)
            ReturnAngle = (_Angle + (360f * ((_Angle / 360f) + 1)));

        else if (_Angle > 360f)
            ReturnAngle = (_Angle - (360f * (_Angle / 360f)));

        else if (ReturnAngle == 360) //Never use 360, only go from 0 to 359
            ReturnAngle = 0;

        return ReturnAngle;
    }
    public async void loadingFlightData(string file)
    {
        //string[] data = System.IO.File.ReadAllLines(@"D:\\Unity\\Hololens\\Trajectories\\191214\\" + file);
        //        Dictionary<string, Vector4> coordinates = new Dictionary<string, Vector4>();
        List<string> data = new List<string>();
#if WINDOWS_UWP
                                                        data = await getData(file, 0);
#endif

        System.Random random = new System.Random();
        int min = 1;
        int max = 7;
        int randNum1 = random.Next(min, max);
        int randNum2 = random.Next(min, max);
        int randNum3 = random.Next(min, max);
        int randNum4 = random.Next(min, max);
        squawks = new Dictionary<string, string>();
        string squawk = randNum1.ToString() + randNum2.ToString() + randNum3.ToString() + randNum4.ToString();

        string[] fileName = file.Split('_');
        fileName[0] = fileName[0].Replace(" ", String.Empty);
        int code = Convert.ToInt32(squawk);
        //if (fileName.Length == 2)
        //{
        //    DataClass.flightsSquawkCodes.Add(fileName[0], code.ToString());
        //    fileName[0] += " " + (code).ToString();            
        //}
        //else
        //{
        //    DataClass.flightsSquawkCodes.Add(fileName[0] + fileName[2], code.ToString());
        //    fileName[0] += fileName[2] + " " + (code).ToString();
        //}

        //DataClass.currentFlightsSquawkCodes.Add(fileName[0], code.ToString());
        DataClass.flightsSquawkCodes.Add(fileName[0], code.ToString());
        fileName[0] += " " + (code).ToString();

        string oldDate = string.Empty;
        int i = 0;
        flightCoordinates = new Dictionary<string, Vector4>();
        //b.text = (b.text + lines.First());
        try
        {

            foreach (string line in data)//for (int i = 0; i < lines.Length; i++)
            {
                //c.text = "Called foreach";
                string[] tokenized = line.Split(',');//lines[i].Split(',');
                                                     //b.text = (b.text + line);


                //for 19 nov data for excluding 60th second
                string[] checkSecond = tokenized[0].Split(':');
                if (checkSecond[2] != "60")
                {
                    int tempHeading = Convert.ToInt32(tokenized[5]);

                    if (tempHeading < 0)
                    {
                        //tempHeading += 360;
                        tempHeading *= -1;
                    }
                    if (tempHeading == 0)
                    {
                        tempHeading = 360;
                    }

                    tokenized[5] = tempHeading.ToString();

                    if (tokenized.Length > 1)
                    {
                        tokenized[0] = "0" + tokenized[0];
                        //UnityEngine.Debug.Log(file + " " + line + " " + tokenized[0]);
                        DateTime dt = DateTime.ParseExact(tokenized[0], "HH:mm:ss", CultureInfo.InvariantCulture);

                        //float height = (((float)Convert.ToDouble(tokenized[3]) / 3.28084f) * 0.0007f);

                        //nov 19 data
                        float height = (float)Convert.ToDouble(tokenized[3]);
                        //

                        string head = tokenized[5].ToString();
                        string[] neghead = head.Split('-');

                        if (neghead.Length > 1)
                        {
                            head = neghead[1];
                        }

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
                    //UnityEngine.Debug.Log("Check Counter: " + checkCounter);
                    checkCounter++;
                }
            }
            //DataClass.currentSceneTrajectories.Add(fileName[0], flightCoordinates);
            DataClass.trajectories.Add(fileName[0], flightCoordinates);

            //#endif
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
    }
}