using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatLongTo3D : MonoBehaviour
{
    float x, y, z, longitude = 127.0741f, latitude = 37.5519f, radius = 6371;
    double x1, y1, z1;
    private GameObject referencePoint;
    const double a = 6378137.0;         // WGS-84 Earth semimajor axis (m)
    const double b = 6356752.314245;     // Derived Earth semiminor axis (m)
    const double f = (a - b) / a;           // Ellipsoid Flatness
    const double f_inv = 1.0 / f;       // Inverse flattening

    //const double f_inv = 298.257223563; // WGS-84 Flattening Factor of the Earth 
    //const double b = a - a / f_inv;
    //const double f = 1.0 / f_inv;

    const double a_sq = a * a;
    const double b_sq = b * b;
    const double e_sq = f * (2 - f);
    float sizeFactor = 1;

    double[] longArray = new double[11];
    double[] latArray = new double[11];
    Vector2 rw25, rw07, rw13, rw31endpoint, rw31displaced;
    // Use this for initialization
    void Start()
    {
        //double[] gpsUtils = GeodeticToEnu(37.547829, 127.074412, 0, 37.556924, 127.079298, 0);//37.5519f, 127.0741f, 0, 33.472017711413734f, 126.56847141418461f, 0);//53.201233f, 5.799913f, 0, 52.307687f, 4.767424f,0);
        referencePoint = GameObject.Find("RW25const");
        latArray[0] = 37.551993;
        longArray[0] = 127.073920;
        latArray[1] = 37.552009;
        longArray[1] = 127.073910;
        latArray[2] = 37.552025;
        longArray[2] = 127.073881;
        latArray[3] = 37.552038;
        longArray[3] = 127.073862;
        latArray[4] = 37.552048;
        longArray[4] = 127.073840;
        latArray[5] = 37.552058;
        longArray[5] = 127.073816;
        latArray[6] = 37.552066;
        longArray[6] = 127.073795;
        latArray[7] = 37.552078;
        longArray[7] = 127.073776;
        latArray[8] = 37.552095;
        longArray[8] = 127.073749;
        latArray[9] = 37.552242;
        longArray[9] = 127.073604;
        latArray[10] = 37.550266;
        longArray[10] = 127.073137;

        //x for lat and y for long
        rw07.x = 33.499881f;
        rw07.y = 126.468472f;

        rw25.x = 33.514878f;
        rw25.y = 126.497642f;

        rw13.x = 33.515461f;
        rw13.y = 126.487394f;

        rw31displaced.x = 33.507642f;
        rw31displaced.y = 126.500419f;

        rw31endpoint.x = 33.505483f;
        rw31endpoint.y = 126.504014f;

        //double[] gpsUtils = GeodeticToEnu(37.551993, 127.073920, 0, latArray[2], longArray[2], 0);

        //double[] gpsUtils = GeodeticToEnu(32.781244,129.079469,0,33.394560,126.560242,0);

        //towards top right
        //double[] gpsUtils = GeodeticToEnu(33.394560, 126.560242, 0, 35.012927, 129.079469, 0);
        //double x = -11.279178, y = 11.445289;
        //double x1 = -10.65956, y1 = 11.794344;
        //towards bottom right
        //double[] gpsUtils = GeodeticToEnu(33.394560, 126.560242, 0, 32.781244, 129.079469, 0);

        double[] gpsUtils = GeodeticToEnu(rw25.x, rw25.y, 0, rw07.x, rw07.y, 0);

        //towards top left
        //double[] gpsUtils = GeodeticToEnu(33.394560, 126.560242, 0, 35.012927, 125.645607, 0);

        //towards bottom left
        //double[] gpsUtils = GeodeticToEnu(33.394560, 126.560242, 0, 32.781244, 125.645607, 0);

        //double[] gpsUtils = GeodeticToEnu(35.012927, 125.645607, 0, 32.781244, 125.645607, 0);
        //double[] gpsUtils = GeodeticToEnu(35.012927, 125.645607, 0, 35.012927, 129.079469, 0);

        //zero distance
        //double[] gpsUtils = GeodeticToEnu(33.394560, 126.560242, 0, 33.394560, 126.560242, 0);

        //double[] gpsUtils1 = GeodeticToEnu(52.368739, 4.706697, 0, 52.307687, 4.767424, 0);

        //double[] gpsUtils = GeodeticToEnu(53.201233, 5.799913, 0, 52.307687, 4.767424,0);
        Debug.Log("GpsUtils -> xEast: " + (gpsUtils[0]/ 300000).ToString() + " ,yNorth: " + (gpsUtils[1]/ 300000).ToString() + " ,zUp: " + gpsUtils[2].ToString());

        double dist = ((gpsUtils[0] * gpsUtils[0]) + (gpsUtils[1] * gpsUtils[1]));
        dist = System.Math.Sqrt(dist);

        Vector3 endpoint = referencePoint.transform.position +(dire.normalized * ((float)dist) / 1900);

        //important
        this.transform.position = new Vector3(endpoint.x, endpoint.y, endpoint.z);

        Debug.Log("Distance: " + (dist).ToString() + "meters");

        double dist1 = ((this.transform.position.x * this.transform.position.x) + (this.transform.position.z * this.transform.position.z));
        dist1 = System.Math.Sqrt(dist);
        Debug.Log("Distance1: " + (dist1/*/1000*/).ToString() + "meters");
    }



    Vector3 PolarToCartesian(Vector2 polar)
    {

        //an origin vector, representing lat,lon of 0,0. 

        var origin = new Vector3(0, 0, 1);
        origin = new Vector3(0.17474f, -0.263f, 1.513f);
        //build a quaternion using euler angles for lat,lon
        var rotation = Quaternion.Euler(polar.x, polar.y, 0);

        //transform our reference vector by the rotation. Easy-peasy!
        var point = rotation * origin;
        return point;
    }

    public static double[] GeodeticToEcef(double lat, double lon, double h)
    {
        // Convert to radians in notation consistent with the paper:
        double lambda = DegreesToRadians(lat);
        var phi = DegreesToRadians(lon);
        var s = System.Math.Sin(lambda);
        var N = a / System.Math.Sqrt(1 - e_sq * s * s);

        var sin_lambda = System.Math.Sin(lambda);
        var cos_lambda = System.Math.Cos(lambda);
        var cos_phi = System.Math.Cos(phi);
        var sin_phi = System.Math.Sin(phi);

        double x2 = (h + N) * cos_lambda * cos_phi;
        double y2 = (h + N) * cos_lambda * sin_phi;
        double z2 = (h + (1 - e_sq) * N) * sin_lambda;

        double[] array = new double[3];
        array[0] = x2;
        array[1] = y2;
        array[2] = z2;

        return array;
    }
    public double[] GeodeticToEnu(double lat, double lon, double h, double lat0, double lon0, double h0)
    {
        double x, y, z;
        double[] gtoe = GeodeticToEcef(lat, lon, h);

        Debug.Log("XYZ -> x: " + gtoe[0].ToString() + " ,y: " + gtoe[1].ToString() + " ,z: " + gtoe[2].ToString());

        double[] enu = EcefToEnu(gtoe[0], gtoe[1], gtoe[2], lat0, lon0, h0);
        return enu;
    }
    Vector3 or,tar,dire,dif; 
    public double[] EcefToEnu(double x, double y, double z, double lat0, double lon0, double h0)
    {
        // Convert to radians in notation consistent with the paper:
        var lambda = DegreesToRadians(lat0);
        var phi = DegreesToRadians(lon0);
        var s = System.Math.Sin(lambda);
        var N = a / System.Math.Sqrt(1 - e_sq * s * s);

        var sin_lambda = System.Math.Sin(lambda);
        var cos_lambda = System.Math.Cos(lambda);
        var cos_phi = System.Math.Cos(phi);
        var sin_phi = System.Math.Sin(phi);

        double x0 = (h0 + N) * cos_lambda * cos_phi;
        double y0 = (h0 + N) * cos_lambda * sin_phi;
        double z0 = (h0 + (1 - e_sq) * N) * sin_lambda;

        Debug.Log("XYZ -> x: " + (x / 3000000).ToString() + " ,y: " + (y / 3000000).ToString() + " ,z: " + (z / 3000000).ToString());
        Debug.Log("XYZ0 -> x: " + (x0 / 3000000).ToString() + " ,y: " + (y0 / 3000000).ToString() + " ,z: " + (z0 / 3000000).ToString());

        Vector3 origin = new Vector3((float)x, 0.5f, (float)y);
        Vector3 target = new Vector3((float)x0, 0.5f, (float)y0);

        or = new Vector3(origin.x/ 300000, origin.y,origin.z/ 300000);
        

        Vector3 direction = target - origin;
        dire = direction;

        Debug.Log("or: " + or.ToString());

        //Airport.transform.position = new Vector3((float)((x / 300000)-0.2585984), (float)(0.5f), (float)((y / 300000)-0.0674521));
        //testcube.transform.position = new Vector3((float)x0 / 300000, 0, (float)y0 / 300000);
        //Cam.transform.position = new Vector3((Airport.transform.position.x), (float)(1), Airport.transform.position.z - 3.5f);

        Debug.Log("Direction: " + direction.ToString());

        double xd, yd, zd;
        xd = x - x0;
        yd = y - y0;
        zd = z - z0;

        // This is the matrix multiplication
        double xEast = -sin_phi * xd + cos_phi * yd;
        double yNorth = -cos_phi * sin_lambda * xd - sin_lambda * sin_phi * yd + cos_lambda * zd;
        double zUp = cos_lambda * cos_phi * xd + cos_lambda * sin_phi * yd + sin_lambda * zd;

        double[] array = new double[3];
        array[0] = xEast;
        array[1] = yNorth;
        array[2] = zUp;
        tar = new Vector3((float)array[0] / 300000, (float)target.y, (float)array[1] / 300000);
        return array;
    }

    static double DegreesToRadians(double degrees)
    {
        float PI = 3.14f;
        return (PI / 180.0 * degrees);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 forward = CamMix.transform.forward;
        //Debug.Log("Forward: " + forward.ToString());
        //Debug.Log("New Dierct: " + (CamMix.transform.forward - this.transform.position).ToString());
    }

    float latToZ(double lat)
    {

        lat = (lat - 53.178469) / 0.00001 * 0.12179047095976932582726898256213;
        double z = lat;

        return (float)z;
    }

    float lonToX(double lon)
    {

        lon = (lon - 6.503091) / 0.000001 * 0.00728553580298947812081345114627;
        double x = lon;

        return (float)x;
    }
    private float angleFromCoordinate(float lat1, float long1, float lat2, float long2)
    {
        lat1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;
        long1 *= Mathf.Deg2Rad;
        long2 *= Mathf.Deg2Rad;

        float dLon = (long2 - long1);
        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2);
        float x = (Mathf.Cos(lat1) * Mathf.Sin(lat2)) - (Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(dLon));
        float brng = Mathf.Atan2(y, x);
        brng = Mathf.Rad2Deg * brng;
        brng = (brng + 360) % 360;
        brng = 360 - brng;
        return brng;
    }
}