using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClass
{
    private static Dictionary<string, Dictionary<string, Vector4>> flightTrajectories;
    private static Dictionary<string, Dictionary<string, Vector4>> currentTrajectories;
    private static Dictionary<string, string> squawkCodes;
    private static Dictionary<string, string> currentSquawkCodes;

    private static DateTime earlyTime, currentEarlyTime;
    private static bool dataLoaded = false;
    public static Dictionary<string, Dictionary<string, Vector4>> trajectories
    {
        get
        {
            return flightTrajectories;
        }
        set
        {
            flightTrajectories = value;
        }
    }

    public static Dictionary<string, Dictionary<string, Vector4>> currentSceneTrajectories
    {
        get
        {
            return currentTrajectories;
        }
        set
        {
            currentTrajectories = value;
        }
    }
    public static Dictionary<string, string> flightsSquawkCodes
    {
        get
        {
            return squawkCodes;
        }
        set
        {
            squawkCodes = value;
        }
    }
    public static Dictionary<string, string> currentFlightsSquawkCodes
    {
        get
        {
            return currentSquawkCodes;
        }
        set
        {
            currentSquawkCodes = value;
        }
    }

    public static DateTime earliestTime
    {
        get
        {
            return earlyTime;
        }
        set
        {
            earlyTime = value;
        }
    }
    public static DateTime currentEarliestTime
    {
        get
        {
            return currentEarlyTime;
        }
        set
        {
            currentEarlyTime = value;
        }
    }
    public static bool dataLoadedFlag
    {
        get
        {
            return dataLoaded;
        }
        set
        {
            dataLoaded = value;
        }
    }
}
