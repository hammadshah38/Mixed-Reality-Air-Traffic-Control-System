using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneLocation : MonoBehaviour
{
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
    //float sizeFactor = 1;

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
        double[] gtoe = GeodeticToEcef(lat, lon, h);

        //Debug.Log("XYZ -> x: " + gtoe[0].ToString() + " ,y: " + gtoe[1].ToString() + " ,z: " + gtoe[2].ToString());

        double[] enu = EcefToEnu(gtoe[0], gtoe[1], gtoe[2], lat0, lon0, h0);
        return enu;
    }
    Vector3 or, dire, dif;
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

        //Debug.Log("XYZ -> x: " + (x / 300000).ToString() + " ,y: " + (y / 300000).ToString() + " ,z: " + (z / 300000).ToString());
        //Debug.Log("XYZ0 -> x: " + (x0 / 300000).ToString() + " ,y: " + (y0 / 300000).ToString() + " ,z: " + (z0 / 300000).ToString());

        Vector3 origin = new Vector3((float)x, 1.046f, (float)y);
        Vector3 target = new Vector3((float)x0, 1.046f, (float)y0);
        or = new Vector3(origin.x / 1621.68199f/*3000000*/, origin.y, origin.z / 1621.68199f/*3000000*/);

        Vector3 direction = target - origin;
        dire = direction;
        var l = direction.magnitude;
        //Debug.Log("Magnitude: " + l.ToString());
        //Debug.Log("direction: " + direction.ToString());
        double xd, yd, zd;
        xd = x - x0;
        yd = y - y0;
        zd = z - z0;

        // This is the matrix multiplication
        double xEast = -sin_phi * xd + cos_phi * yd;
        double yNorth = -cos_phi * sin_lambda * xd - sin_lambda * sin_phi * yd + cos_lambda * zd;
        double zUp = cos_lambda * cos_phi * xd + cos_lambda * sin_phi * yd + sin_lambda * zd;

        double[] array = new double[13];
        array[0] = xEast;
        array[1] = yNorth;
        array[2] = zUp;
        array[3] = dire.x;
        array[4] = dire.y;
        array[5] = dire.z;
        array[6] = l;

        //new
        array[7] = x;
        array[8] = y;
        array[9] = z;
        array[10] = x0;
        array[11] = y0;
        array[12] = z0;

        return array;
    }

    static double DegreesToRadians(double degrees)
    {
        float PI = 3.14f;
        return (PI / 180.0 * degrees);
    }
}
