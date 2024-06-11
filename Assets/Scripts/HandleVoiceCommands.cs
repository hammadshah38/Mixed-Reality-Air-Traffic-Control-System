using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleVoiceCommands : MonoBehaviour, IDictationHandler
{
    public TextMesh command;
    public TextMesh outputResult;
    private new Renderer renderer;
    private bool isRecording;
    private static string msg;
    private bool commandReceived;
    private DateTime c,c1;
    
    // Use this for initialization
    void Start ()
    {
        try
        {
            renderer = GetComponent<Renderer>();
            msg = "";
            c = DateTime.Now;
            //isRecording = false;
            //command.text += "Start..."/* + isRecording*/;
            //commandReceived = false;
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if(commandReceived == true)
        //{
        //    if (string.Compare(command.text, msg) != 0)
        //    {
        //        command.text = msg;
        //    }
        //}
    }

    public void OnVoiceCommand()
    {
        try
        {
            renderer = command.GetComponent<Renderer>();
            renderer.material.color = Color.red;
            command.text = " Listening...";
            ToggleRecording();
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
    }

    public void OnDictationHypothesis(DictationEventData eventData)
    {
        try
        {
            //command.text = eventData.DictationResult;
            command.text = " Listening...";
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
    }

    public void OnDictationResult(DictationEventData eventData)
    {
        try
        {
            if(eventData.DictationResult != null)
            {
                //command.text = eventData.DictationResult + " Result";
                command.text = " Listening...";


                //renderer = command.GetComponent<Renderer>();
                //renderer.material.color = Color.green;
                //command.text = eventData.DictationResult;
                //msg = eventData.DictationResult;
            }
            else
            {
                command.text = "Null Result";
            }
            //msg = eventData.DictationResult;
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
    }

    public void OnDictationComplete(DictationEventData eventData)
    {
        try
        {
            string result = eventData.DictationResult;
            result = result.Replace(" ", String.Empty);
            result = result.ToLower();

            string receivedCommand = String.Empty;
            string airplane = String.Empty;
            #region check aircraft
            if (eventData.DictationResult.Contains("707"))
            {
                receivedCommand = "TWB707(#4), Direct to YUMIN";
                airplane = "Aircraft04";
            }
            else if (eventData.DictationResult.Contains("601"))
            {
                receivedCommand = "JJA601(#6), Direct to YUMIN";
                airplane = "Aircraft06";
            }
            else if (eventData.DictationResult.Contains("209"))
            {
                receivedCommand = "ESR209(#8), Direct to YUMIN";
                airplane = "Aircraft08";
            }
            else if (eventData.DictationResult.Contains("1901"))
            {
                receivedCommand = "KAL1901(#10), Direct to YUMIN";
                airplane = "Aircraft10";
            }
            else if (eventData.DictationResult.Contains("8092"))
            {
                receivedCommand = "ABL8092(#3), Direct to YUMIN";
                airplane = "Aircraft03";
            }
            else if (eventData.DictationResult.Contains("851"))
            {
                receivedCommand = "JJA851(#12), Direct to YUMIN";
                airplane = "Aircraft12";
            }
            else if (eventData.DictationResult.Contains("1005"))
            {
                receivedCommand = "KAL1005(#14), Direct to YUMIN";
                airplane = "Aircraft14";
            }
            else if (eventData.DictationResult.Contains("1213"))
            {
                receivedCommand = "KAL1213(#18), Direct to YUMIN";
                airplane = "Aircraft18";
            }
            else if (eventData.DictationResult.Contains("309"))
            {
                receivedCommand = "JNA309(#20), Direct to YUMIN";
                airplane = "Aircraft20";
            }
            #endregion
            command.text = receivedCommand;// + "\nComplete";
            //command.text = eventData.DictationResult + " Complete";
            msg = command.text;
            //SendMessageToServer(msg);
            //c = DateTime.Now;
            updateTrajectory(airplane);
            //command.text += "After updating";
            //c1 = DateTime.Now;
            //TimeSpan ts = (c1 - c);
            //command.text += "\nInteraction Time: " + ts.Milliseconds.ToString() + " ms";
            //ToggleRecording();
            //renderer = command.GetComponent<Renderer>();
            //renderer.material.color = Color.green;
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
    }

    public void OnDictationError(DictationEventData eventData)
    {
        try
        {
            isRecording = false;
            command.color = Color.yellow;
            command.text = "Error";
            StartCoroutine(DictationInputManager.StopRecording());
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
    }

    private void ToggleRecording()
    {
        try
        {
            if (isRecording)
            {
                //command.text += "isRecording True";
                isRecording = false;
                StartCoroutine(DictationInputManager.StopRecording());
                command.color = Color.red;
            }
            else
            {
                //command.text = "isRecording False";
                isRecording = true;
                StartCoroutine(DictationInputManager.StartRecording(this.gameObject, 5f, 12f, 12));
                command.color = Color.red;
            }
        }
        catch (Exception e)
        {
            command.text = e.Message;
        }
    }
    private void SendMessageToServer(string msg)
    {
        msg = msg.Replace(" ", String.Empty);
        msg = msg.ToLower();
        if (msg.Contains("hammad") == true)
        {
            command.text = msg + " True";
        }
        else
        {
            command.text = msg + " False";
        }
    }

    public void updateTrajectory(string airplaneName)
    {
        if (airplaneName != String.Empty)
        {
            DataClass.dataLoadedFlag = false;
            string name = (airplaneName + " " + DataClass.flightsSquawkCodes[airplaneName]);
            //flights.Remove(name);
            //outputResult.text = name;
            GameObject objectToDelete = GameObject.Find(name);
            if (objectToDelete != null)
            {
                //outputResult.text = name;
                AirplaneLocation locationObj;
                double[] gpsUtils;
                string airplane, lessDistanceIndex;
                Dictionary<string, Vector2> wayPoints;

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

                AirplaneController playerScript = objectToDelete.GetComponent<AirplaneController>();
                Vector2 currentLocaionOfPlane = new Vector2(playerScript.latLong.x, playerScript.latLong.y);
                double lastDistance = 0.0;
                for (int i = 1; i <= 7; i++)
                {
                    Vector2 tempWaypoint = new Vector2();
                    tempWaypoint = wayPoints["PC62" + i];
                    gpsUtils = locationObj.GeodeticToEnu(currentLocaionOfPlane.x, currentLocaionOfPlane.y, 0, tempWaypoint.x, tempWaypoint.y, 0);

                    if (i == 1)
                    {
                        lastDistance = gpsUtils[6];
                        lessDistanceIndex = "PC62" + i;
                    }
                    else
                    {
                        if (lastDistance > gpsUtils[6])
                        {
                            lastDistance = gpsUtils[6];
                            lessDistanceIndex = "PC62" + i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                GameObject.Destroy(objectToDelete);
                //DataClass.currentSceneTrajectories.Remove(name);
                DataClass.trajectories.Remove(name);
                DataClass.flightsSquawkCodes.Remove(airplaneName);
                //DataClass.currentFlightsSquawkCodes.Remove(airplane);

                LoadAirTraffic obj = new LoadAirTraffic();
                //UnityEngine.Debug.Log("New Loaded Flight " + airplaneName + "_traj_" + lessDistanceIndex);
                obj.loadingFlightData(airplaneName + "_traj_" + lessDistanceIndex);
                outputResult.text = "Flight Directed to YUMIN!"; // "New Loaded Flight " + airplaneName + "_traj_" + lessDistanceIndex;
                DataClass.dataLoadedFlag = true;
                StartCoroutine(emptyText(4f));
            }
            else
            {
                DataClass.dataLoadedFlag = true;
                UnityEngine.Debug.Log("Flight does not exist!");
                outputResult.text += "Flight does not exist!";
                StartCoroutine(emptyText(4f));
            }
        }
        else
        {
            UnityEngine.Debug.Log("Could not recognize command!\nPlease speak again");
            command.text = "Please try again from start!" ;//"Could not recognize command!\nPlease speak again";
            StartCoroutine(emptyText(4f));
        }
    }
    public IEnumerator emptyText(float timeToMove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            yield return null;
        }
        outputResult.text = "";
        command.text = "";
    }
}
//if(eventData.DictationResult.Contains("8092") == true)
//{
//    if (eventData.DictationResult.Contains("4000") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), climb and maintain 4000, \nand contact jeju approach, 121.2";
//    }
//    else if (eventData.DictationResult.Contains("340") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), turn left heading 340";
//    }
//    else if (eventData.DictationResult.Contains("250") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), turn left heading 250";
//    }
//    else if (eventData.DictationResult.Contains("2500") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), descend to 2500";
//    }
//    else if (eventData.DictationResult.Contains("160") == true && eventData.DictationResult.Contains("2100") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), turn left heading 160, \nand descend to 2100";
//    }
//    else if (eventData.DictationResult.Contains("100") == true && eventData.DictationResult.Contains("1500") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), turn left heading 100, \nand descend to 1500";
//    }
//    else if (eventData.DictationResult.Contains("70") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), turn left heading 070, \nreport when ILS established";
//    }
//    else if (eventData.DictationResult.Contains("118") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), contact tower 118.2";
//    }
//    else if (eventData.DictationResult.Contains("land") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), clear to land";
//    }
//    else if (eventData.DictationResult.Contains("121") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), contact ground, \n121.65 after inspection";
//    }
//    else if (eventData.DictationResult.Contains("taxi") == true && eventData.DictationResult.Contains("reuqest") == true)
//    {
//        receivedCommand = "Ground Control, \nAirbusan8092(#3), request taxi";
//    }
//    else if (eventData.DictationResult.Contains("taxi") == true && eventData.DictationResult.Contains("gate") == true)
//    {
//        receivedCommand = "Airbusan8092(#3), taxi to gate15 via P, G1";
//    }
//    else
//    {
//        receivedCommand = "Airbusan8092(#3), clear to take-off";
//    }
//}
//else if (eventData.DictationResult.Contains("209") == true)
//{
//    receivedCommand = "Easterjet209(#8), direct to YUMIN";
//}
//else if (eventData.DictationResult.Contains("1901") == true)
//{
//    receivedCommand = "Koreanair1901(#10), direct to YUMIN";
//}
//else if (eventData.DictationResult.Contains("851") == true)
//{
//    if (eventData.DictationResult.Contains("90") == true && eventData.DictationResult.Contains("7000") == true)
//    {
//        receivedCommand = "Jejuair851(#12), turn left heading 090, \nmaintain 7000";
//    }
//    else if (eventData.DictationResult.Contains("180") == true && eventData.DictationResult.Contains("7000") == true)
//    {
//        receivedCommand = "Jejuair851(#12), turn right heading 180, \nmaintain 7000";
//    }
//    else if (eventData.DictationResult.Contains("270") == true && eventData.DictationResult.Contains("7000") == true)
//    {
//        receivedCommand = "Jejuair851(#12), turn right heading 270, \nmaintain 7000";
//    }
//    else if (eventData.DictationResult.Contains("360") == true && eventData.DictationResult.Contains("7000") == true)
//    {
//        receivedCommand = "Jejuair851(#12), turn right heading 360, \nmaintain 7000";
//    }
//    else if(eventData.DictationResult.Contains("direct") == true)
//    {
//        receivedCommand = "Jejuair851(#12), direct to YUMIN";
//    }
//}
//else if (eventData.DictationResult.Contains("1005") == true)
//{
//    if (eventData.DictationResult.Contains("90") == true && eventData.DictationResult.Contains("8000") == true)
//    {
//        receivedCommand = "Koreanair1005(#14), turn left heading 090, \nmaintain 8000";
//    }
//    else if (eventData.DictationResult.Contains("180") == true && eventData.DictationResult.Contains("8000") == true)
//    {
//        receivedCommand = "Koreanair1005(#14), turn right heading 180, \nmaintain 8000";
//    }
//    else if (eventData.DictationResult.Contains("270") == true && eventData.DictationResult.Contains("8000") == true)
//    {
//        receivedCommand = "Koreanair1005(#14), turn right heading 270, \nmaintain 8000";
//    }
//    else if (eventData.DictationResult.Contains("360") == true && eventData.DictationResult.Contains("8000") == true)
//    {
//        receivedCommand = "Koreanair1005(#14), turn right heading 360, \nmaintain 8000";
//    }
//    else if (eventData.DictationResult.Contains("direct") == true)
//    {
//        receivedCommand = "Koreanair1005(#14), direct to YUMIN";
//    }
//}