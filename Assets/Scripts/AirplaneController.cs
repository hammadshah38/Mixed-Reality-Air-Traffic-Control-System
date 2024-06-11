using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    AirplaneLocation locationObj;
    public Vector2 latLong;
    Vector2 rw25, rw07, rw13, rw31endpoint, rw31displaced;
    int i = 0;
    int checkTrail = 0;
    AudioSource snd;
    private GameObject referencePoint;
    double[] gpsUtils;
    float currentLerpTime = 0;
    private void Start()
    {
        latLong = new Vector2(0,0);
    }
    public int SetNewFlightData(Vector3 origin, Vector4 destination, string name, string category)
    {
        int tempHeight;

        if(category == "map" || category == "waypoint")
        {
            referencePoint = GameObject.Find("newtower");
            //tempHeight = (int)((destination.z * 3.28084f) / /*old scale 0.00000363f*/ /*new scale 0.00000139f*/ /*/*test*/ 0.00000278f);
            //referencePoint = GameObject.Find("CheckTower");

            //for nov 19 data
            tempHeight = (int)(destination.z);
            float height = (((float)Convert.ToDouble(destination.z) / 3.28084f) * 0.00000278f);
            destination = new Vector4(destination.x, destination.y, height, destination.w);
        }
        else
        {
            referencePoint = GameObject.Find("newtower(new) (1)");
            //tempHeight = (int)((destination.z * 3.28084f) / /*old scale 0.00000363f*/ /*new scale*/ 0.0007f);
            //referencePoint = GameObject.Find("CheckTower");

            //for nov 19 data
            tempHeight = (int)(destination.z);
            float height = (((float)Convert.ToDouble(destination.z) / 3.28084f) * 0.0007f);
            destination = new Vector4(destination.x, destination.y, height, destination.w);
        }
        double heading = 0.0f;
        string speed = "";
        if (category != "realWorld" && category != "waypoint")
        {
            //extract heading and speed
            float th1 = (float)destination.w;
            string th = th1.ToString();
            string[] stringSeparators = new string[] { "." };
            string[] headingArray = th.Split(stringSeparators, StringSplitOptions.None);

            try
            {
                heading = Convert.ToDouble(headingArray[1]);
            }
            catch(Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
            speed = headingArray[0];
        }

        locationObj = new AirplaneLocation();
        gpsUtils = new double[6];
        gpsUtils = locationObj.GeodeticToEnu(origin.x, origin.y, 0, destination.x, destination.y, 0);

        double dist = ((gpsUtils[0] * gpsUtils[0]) + (gpsUtils[1] * gpsUtils[1]));
        dist = System.Math.Sqrt(dist);
        //Debug.Log("Distance by controller: " + dist.ToString());
        Vector3 dire = new Vector3((float)gpsUtils[3], (float)gpsUtils[4], (float)gpsUtils[5]);

        //GameObject a = this.transform.GetChild(0).gameObject;

        if (category != "realWorld" && category != "waypoint")
        {
            //forPlane
            GameObject a = this.transform.GetChild(3).gameObject;
            GameObject meshParent = a.transform.GetChild(0).gameObject;
            TextMesh mesh = meshParent.GetComponent<TextMesh>();
            string message = "";
            float height = ((destination.z * 3.28084f) / 0.00000363f);
            //System.Random random = new System.Random();
            //int randNum1 = random.Next(0, 7);
            //int randNum2 = random.Next(0, 7);
            //int randNum3 = random.Next(0, 7);
            //int randNum4 = random.Next(0, 7);
            if (name.Contains("Aircraft03") && tempHeight <= 500)
            {
                if (name.Contains("Aircraft04"))
                {
                    name = name.Replace("Aircraft04", "TWB707");
                }
                else if (name.Contains("Aircraft01"))
                {
                    name = name.Replace("Aircraft01", "JNA562");
                }
                else if (name.Contains("Aircraft05"))
                {
                    name = name.Replace("Aircraft05", "JNA584");
                }
                else if (name.Contains("Aircraft07"))
                {
                    name = name.Replace("Aircraft07", "JJA108");
                }
                else if (name.Contains("Aircraft11"))
                {
                    name = name.Replace("Aircraft11", "JJA142");
                }
                else if (name.Contains("Aircraft13"))
                {
                    name = name.Replace("Aircraft13", "KAL1216");
                }
                else if (name.Contains("Aircraft19"))
                {
                    name = name.Replace("Aircraft19", "JJA504");
                }
                else if (name.Contains("Aircraft21"))
                {
                    name = name.Replace("Aircraft21", "TWB904");
                }
                else if (name.Contains("Aircraft06"))
                {
                    name = name.Replace("Aircraft06", "JJA601");
                }
                else if (name.Contains("Aircraft08"))
                {
                    name = name.Replace("Aircraft08", "ESR209");
                }
                else if (name.Contains("Aircraft10"))
                {
                    name = name.Replace("Aircraft10", "KAL1901");
                }
                else if (name.Contains("Aircraft03"))
                {
                    name = name.Replace("Aircraft03", "ABL8092");
                }
                else if (name.Contains("Aircraft12"))
                {
                    name = name.Replace("Aircraft12", "JJA851");
                }
                else if (name.Contains("Aircraft14"))
                {
                    name = name.Replace("Aircraft14", "KAL1005");
                }
                else if (name.Contains("Aircraft18"))
                {
                    name = name.Replace("Aircraft18", "KAL1213");
                }
                else if (name.Contains("Aircraft20"))
                {
                    name = name.Replace("Aircraft20", "JNA309");
                }

                GameObject light = this.transform.GetChild(1).gameObject;
                GameObject dirlight = this.transform.GetChild(2).gameObject;
                if (light.active != true)
                {
                    light.SetActive(true);
                    mesh.color = Color.red;
                    //GameObject cube = a.transform.GetChild(0).gameObject;
                    //Renderer r = cube.GetComponent<Renderer>();
                    //r.material.color = Color.yellow;
                    //cube.SetActive(true);
                    dirlight.SetActive(true);
                }
                int th = 0;
                int sp = 0;

                if (tempHeight >= 1000)
                {
                    th = tempHeight / 10;
                }
                else
                {
                    th = tempHeight;
                }
                if (tempHeight >= 100)
                {
                    sp = Convert.ToInt32(speed) / 10;
                }
                else
                {
                    sp = Convert.ToInt32(speed);
                }
                string[] splittedName = name.Split(' ');
                message = splittedName[0] + "\n" + th + "/" + sp + "\n" + splittedName[1];
                //message = name /*+ " " + randNum1.ToString() + randNum2.ToString() + randNum3.ToString() + randNum4.ToString() */+ "\nHeight: " + (int)tempHeight + " ft.\n" + "Bird Striked";
            }
            else
            {
                if (name.Contains("Aircraft04"))
                {
                    name = name.Replace("Aircraft04", "TWB707");
                }
                else if (name.Contains("Aircraft01"))
                {
                    name = name.Replace("Aircraft01","JNA562");
                }
                else if (name.Contains("Aircraft05"))
                {
                    name = name.Replace("Aircraft05","JNA584");
                }
                else if (name.Contains("Aircraft07"))
                {
                    name = name.Replace("Aircraft07","JJA108");
                }
                else if (name.Contains("Aircraft11"))
                {
                    name = name.Replace("Aircraft11","JJA142");
                }
                else if (name.Contains("Aircraft13"))
                {
                    name = name.Replace("Aircraft13","KAL1216");
                }
                else if (name.Contains("Aircraft19"))
                {
                    name = name.Replace("Aircraft19","JJA504");
                }
                else if (name.Contains("Aircraft21"))
                {
                    name = name.Replace("Aircraft21", "TWB904");
                }
                else if (name.Contains("Aircraft06"))
                {
                    name = name.Replace("Aircraft06", "JJA601");
                }
                else if (name.Contains("Aircraft08"))
                {
                    name = name.Replace("Aircraft08", "ESR209");
                }
                else if (name.Contains("Aircraft10"))
                {
                    name = name.Replace("Aircraft10", "KAL1901");
                }
                else if (name.Contains("Aircraft03"))
                {
                    name = name.Replace("Aircraft03", "ABL8092");
                }
                else if (name.Contains("Aircraft12"))
                {
                    name = name.Replace("Aircraft12", "JJA851");
                }
                else if (name.Contains("Aircraft14"))
                {
                    name = name.Replace("Aircraft14", "KAL1005");
                }
                else if (name.Contains("Aircraft18"))
                {
                    name = name.Replace("Aircraft18", "KAL1213");
                }
                else if (name.Contains("Aircraft20"))
                {
                    name = name.Replace("Aircraft20", "JNA309");
                }

                int th = 0;
                int sp = 0;

                if (tempHeight >= 1000)
                {
                    th = tempHeight / 10;
                }
                else
                {
                    th = tempHeight;
                }
                if (tempHeight >= 100)
                {
                    sp = Convert.ToInt32(speed) / 10;
                }
                else
                {
                    sp = Convert.ToInt32(speed);
                }
                string[] splittedName = name.Split(' ');
                //message = name /*+ " " + randNum1.ToString() + randNum2.ToString() + randNum3.ToString() + randNum4.ToString()*/ + "\nHeight: " + (int)tempHeight + " ft.\n" + "Speed: " + speed + "kts";
                message = splittedName[0] + "\n" + th + "/" + sp + "\n" + splittedName[1];
            }

            //float step = 2 * Time.deltaTime;
            //Vector3 dir = Vector3.RotateTowards(this.transform.position, Camera.main.transform.position, step, 0.0f);
            //a.transform.rotation = Quaternion.LookRotation(dir);

            mesh.text = message;
        }

        if (category == "detail")
        {
            //height = ((destination.z * 3.28084f) / 0.0007f);
            //Vector3 endpoint = referencePoint.transform.position + (dire.normalized * ((float)dist) / 5565.723405f/*1621.68199f*/ /*1900*/);
            Vector3 endpoint = referencePoint.transform.position + (dire.normalized * ((float)gpsUtils[6] * 0.0007f));


            //runway alt 1.1
            //Vector3 newDestination = new Vector3(endpoint.x, endpoint.y, endpoint.z);
            //this.transform.position = Vector3.Lerp(this.transform.position, newDestination, 0.5f);//new Vector3(endpoint.x, endpoint.y, endpoint.z);

            float groundPosition = referencePoint.transform.position.y + destination.z + 0.002f;//1.069f + destination.z;

            Vector3 newPosition = new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);
            
            
            //new Code for rotation

            Vector3 targetDir = newPosition - this.transform.position;


            //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1 * Time.deltaTime, 0.0f);

            //Vector3 newDir = newPosition + targetDir.normalized;
            //newDir = new Vector3(newDir.x, newDir.y - 0.5f, newDir.z);

            // Move our position a step closer to the target.
            //this.transform.LookAt(newPosition + this.transform.forward);
            //this.transform.rotation = Quaternion.LookRotation(newDir);
            //this.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 0.05f, transform.rotation.z, transform.rotation.w);

            //this.transform.RotateAround(newPosition, this.transform.up, 20*Time.deltaTime);

            //setRotation((float)destination.w);

            //setRotation((float)heading);
            //StartCoroutine(Rotate(Vector3.up, (float)heading, 0.95f));

            //Quaternion rotation2 = Quaternion.Euler(new Vector3(0, (float)heading-25, 0));
            //StartCoroutine(rotateObject(rotation2/*new Vector3(0, (float)heading - 25, 0)*/, 0.5f));

            if (this.gameObject.active == false)
            {
                this.transform.position = newPosition;
                this.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(MoveToPosition(this.gameObject.transform, newPosition, 0.3f));

                Quaternion rotation2 = Quaternion.Euler(new Vector3(0, (float)heading - 25, 0));
                StartCoroutine(rotateObject(rotation2/*new Vector3(0, (float)heading - 25, 0)*/, 0.3f));
            }
            //StartCoroutine(MoveFromTo(this.gameObject.transform, this.transform.position, newPosition, 1000));
            //StartCoroutine(MoveToPosition(this.gameObject.transform, newPosition, 0.9f));
            
            //this.transform.position = newPosition;
            //this.transform.position = Vector3.Lerp(this.transform.position, newPosition, 1.0F);
            //rotation code end

            TrailRenderer trail = this.GetComponent<TrailRenderer>();
            if(checkTrail == 1)
            {
                trail.Clear();
            }
            

            //if (i < 30)
            //{
            //    trail.widthMultiplier = 0;
            //}
            //else
            //{
            //    trail.widthMultiplier = 0.002f;
            //}
            if (i > 1)
            {
                trail.enabled = true;
            }
            /*if (dist > 3180 && dist < 6380)
            {
                this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                trail.widthMultiplier = 0.003f;
                mesh.fontSize = 70;
            }
            else if (dist > 6380 && dist < 9540)
            {
                this.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
                trail.widthMultiplier = 0.004f;
                mesh.fontSize = 80;
            }
            else if (dist > 9540 && dist < 12720)
            {
                this.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                trail.widthMultiplier = 0.01f;
            }
            else if (dist > 12720)
            {
                this.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
                trail.widthMultiplier = 0.015f;
            }
            else if (dist < 3180)
            {
                this.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
                trail.widthMultiplier = 0.002f;
                mesh.fontSize = 70;
            }*/
            //this.transform.localScale = new Vector3(0.00056f, 0.00056f, 0.00056f);
            float newX = (((float)gpsUtils[0] * 0.00007f) - 50);
            float newY = (((float)gpsUtils[1] * 0.00007f) - 50);
            float newZ = (((float)gpsUtils[2] * 0.00007f) - 50);

            float d = Vector3.Distance(referencePoint.transform.position, this.transform.position);
            //Debug.Log("Distance by controller after: " + dist.ToString());
            i++;
            checkTrail++;
        }
        else if (category == "realWorld")
        {

            //Vector3 endpoint = GameObject.Find("MixedRealityCameraParent").transform.position + (dire.normalized * ((float)gpsUtils[6]));

            //float groundPosition = GameObject.Find("MixedRealityCameraParent").transform.position.y;

            //new code

            Vector3 endpoint = GameObject.Find("MixedRealityCamera").transform.position + (dire.normalized * ((float)gpsUtils[6]));

            float groundPosition = GameObject.Find("MixedRealityCamera").transform.position.y;

            //

            this.transform.position = new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);

            this.transform.position = new Vector3((float)(gpsUtils[10]- gpsUtils[7]),Camera.main.transform.position.y, (float)(gpsUtils[11] - gpsUtils[8]));

            this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            float angle = Vector3.SignedAngle(this.transform.position, GameObject.Find("MixedRealityCameraParent").transform.forward, Vector3.up);

            Debug.Log("Angle " + angle.ToString());
        }
        else if (category == "map")
        {
            
            Vector3 endpoint = referencePoint.transform.position + (dire.normalized * ((float)gpsUtils[6] * /*old scale 0.00000363f*/ /*new scale 0.00000139f*/ /*/*test*/ 0.00000278f));
            //this.transform.localScale = new Vector3(0.00056f, 0.00056f, 0.00056f);
            float groundPosition = referencePoint.transform.position.y + destination.z;//0.89f + destination.z;
            //this.transform.position = new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);
            float sharpness = 0.1f;
            float blend = 1f - Mathf.Pow(1f - sharpness, 30f * Time.deltaTime);

            Vector3 newPosition = new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);
            //setRotation((float)destination.w);

            //setRotation((float)heading);
            //StartCoroutine(Rotate(Vector3.up, (float)heading, 0.95f));

            //Quaternion rotation2 = Quaternion.Euler(new Vector3(0, (float)heading - 25, 0));
            //StartCoroutine(rotateObject(rotation2/*new Vector3(0, (float)heading - 25, 0)*/, 0.5f/*0.95f*/));

            //this.transform.position = new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);
            if (this.gameObject.active == false)
            {
                this.transform.position = newPosition;
                this.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(MoveToPosition(this.gameObject.transform, newPosition, 0.3f));
                Quaternion rotation2 = Quaternion.Euler(new Vector3(0, (float)heading - 25, 0));
                StartCoroutine(rotateObject(rotation2/*new Vector3(0, (float)heading - 25, 0)*/, /*0.35f*/0.3f));
            }

            TrailRenderer trail = this.GetComponent<TrailRenderer>();
            
            if (checkTrail == 1)
            {
                trail.Clear();
            }
            
            //if (i<30)
            //{
            //    trail.widthMultiplier = 0;
            //}
            //else
            //{
            //    trail.widthMultiplier = 0.002f;
            //}
            if (i > 1)
            {
                //trail.enabled = true;
            }
            
            //if (dist < 100000)
            //{
            //    this.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
            //    trail.widthMultiplier = 0.002f;
            //    mesh.fontSize = 60;
            //}
            //else
            //{
            //    this.transform.localScale = new Vector3(0.016f, 0.016f, 0.016f);
            //    trail.widthMultiplier = 0.002f;
            //    mesh.fontSize = 70;
            //}
            checkTrail++;
            i++;
        }
        else
        {
            GameObject a = this.transform.GetChild(0).gameObject;
            GameObject meshParent = a.transform.GetChild(0).gameObject;
            TextMesh mesh = meshParent.GetComponent<TextMesh>();
            string message = "";

            string[] splittedName = name.Split(' ');
            
            message = splittedName[1];
            mesh.text = message;

            Vector3 endpoint = referencePoint.transform.position + (dire.normalized * ((float)gpsUtils[6] * 0.00000278f));
            
            float groundPosition = referencePoint.transform.position.y + destination.z;
            
            float sharpness = 0.1f;
            float blend = 1f - Mathf.Pow(1f - sharpness, 30f * Time.deltaTime);

            Vector3 newPosition = new Vector3(endpoint.x, groundPosition/*endpoint.y + 0.005f*/, endpoint.z);

            if (this.gameObject.active == false)
            {
                this.transform.position = newPosition;
                this.gameObject.SetActive(true);
            }
        }

        return 0;
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
    void setRotation(float heading)
    {
        //heading -= 25;

        //if (heading < 0)
        //{
        //    heading += 360;
        //}

        var rotation = Quaternion.AngleAxis((heading /*+ 65*/), Vector3.up).eulerAngles;
        this.transform.eulerAngles/*localEulerAngles*/ = rotation;

        //if ((this.transform.localEulerAngles.y - 25) < 25)
        //{
        this.transform.eulerAngles/*localEulerAngles*/ = new Vector3(this.transform.eulerAngles/*localEulerAngles*/.x, (this.transform.eulerAngles/*localEulerAngles*/.y - 25) /*+ 360*/, this.transform.eulerAngles/*localEulerAngles*/.z);
        //}
        //else
        //{
        //    this.transform.eulerAngles/*localEulerAngles*/ = new Vector3(this.transform.eulerAngles/*localEulerAngles*/.x, this.transform.eulerAngles/*localEulerAngles*/.y - 30, this.transform.eulerAngles/*localEulerAngles*/.z);
        //}        
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 0.95f)
    {
        Quaternion from = this.transform.rotation;
        Quaternion to = this.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / duration;
            this.transform.rotation = Quaternion.Slerp(from, to, /*elapsed / duration*/elapsed);
            //elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
    }
    bool rotating = false;
    IEnumerator rotateObject(Quaternion newRot, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = this.transform.rotation;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            this.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }
    //IEnumerator rotateObject(Vector3 eulerAngles, float duration)
    //{
    //    if (rotating)
    //    {
    //        yield break;
    //    }
    //    rotating = true;

    //    Vector3 newRot = this.transform.eulerAngles + eulerAngles;

    //    Vector3 currentRot = this.transform.eulerAngles;

    //    float counter = 0;
    //    while (counter < duration)
    //    {
    //        counter += Time.deltaTime;
    //        this.transform.eulerAngles = Vector3.Lerp(currentRot, newRot, counter / duration);
    //        yield return null;
    //    }
    //    rotating = false;
    //}
    // Use this for initialization
    //void Start ()
    //{
    //    referencePoint = GameObject.Find("RW25const");
    //    locationObj = new AirplaneLocation();
    //    //double[] gpsUtils = locationObj.GeodeticToEnu(37.551993, 127.073920, 0, 37.552025, 127.073881, 0);

    //    //towards top right
    //    //double[] gpsUtils = locationObj.GeodeticToEnu(33.394560, 126.560242, 0, 35.012927, 129.079469, 0);

    //    //towards bottom right
    //    //double[] gpsUtils = locationObj.GeodeticToEnu(33.394560, 126.560242, 0, 32.849112, 129.079469, 0);

    //    //towards top left
    //    //double[] gpsUtils = locationObj.GeodeticToEnu(33.394560, 126.560242, 0, 35.012927, 125.722764, 0);

    //    //towards bottom left
    //    //double[] gpsUtils = locationObj.GeodeticToEnu(33.394560, 126.560242, 0, 32.849112, 125.722764, 0);

    //    rw07.x = 33.499881f;
    //    rw07.y = 126.468472f;

    //    rw25.x = 33.514878f;
    //    rw25.y = 126.497642f;

    //    rw13.x = 33.515461f;
    //    rw13.y = 126.487394f;

    //    rw31displaced.x = 33.507642f;
    //    rw31displaced.y = 126.500419f;

    //    rw31endpoint.x = 33.505483f;
    //    rw31endpoint.y = 126.504014f;

    //    double[] gpsUtils = locationObj.GeodeticToEnu(rw25.x, rw25.y, 0, rw07.x, rw07.y, 0);

    //    //double[] gpsUtils = locationObj.GeodeticToEnu(33.394560, 126.560242, 0, 33.441505, 127.492215, 0);

    //    //double[] gpsUtils = GeodeticToEnu(35.012927, 125.645607, 0, 32.781244, 125.645607, 0);
    //    //double[] gpsUtils = GeodeticToEnu(35.012927, 125.645607, 0, 35.012927, 129.079469, 0);

    //    double dist = ((gpsUtils[0] * gpsUtils[0]) + (gpsUtils[1] * gpsUtils[1]));
    //    dist = System.Math.Sqrt(dist);

    //    Vector3 dire = new Vector3((float)gpsUtils[3], (float)gpsUtils[4], (float)gpsUtils[5]);

    //    Vector3 endpoint = referencePoint.transform.position + (dire.normalized * ((float)dist) / 1900);

    //    this.transform.position = new Vector3(endpoint.x, endpoint.y, endpoint.z);
    //}
    // Update is called once per frame
    void Update () {
		
	}
}
