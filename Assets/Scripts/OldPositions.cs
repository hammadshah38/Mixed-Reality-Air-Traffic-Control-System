using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OldPositions
{
    private static Vector3 mapPos, airportPos;
    private static bool mapSceneReload, airportSceneReload;

    public static Vector3 MapPos
    {
        get
        {
            return mapPos;
        }
        set
        {
            mapPos = value;
        }
    }
    public static Vector3 AirportPos
    {
        get
        {
            return airportPos;
        }
        set
        {
            airportPos = value;
        }
    }
    public static bool MapSceneReload
    {
        get
        {
            return mapSceneReload;
        }
        set
        {
            mapSceneReload = value;
        }
    }
    public static bool AirportSceneReload
    {
        get
        {
            return airportSceneReload;
        }
        set
        {
            airportSceneReload = value;
        }
    }
}