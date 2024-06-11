using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using System;
using System.Linq;

public class HandInterNew : MonoBehaviour
{
    public Camera cam;
    bool _handTracked = false;
    int noOfHandsTracked = 0;
    private GameObject _crs1, _crs2;
    //public TextMesh info;

    public bool showHandCrsr = true;

    public GameObject CurFocusedObj { get; set; }


    //custom variables
    public GameObject TrackingObject;
    

    private HashSet<uint> trackedHands = new HashSet<uint>();
    private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();

    private uint activeId;




    // Use this for initialization
    void Awake()
    {
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
    }


    //Customization

    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }
        trackedHands.Add(id);
        activeId = id;

        var obj = Instantiate(TrackingObject) as GameObject;

        //Vector3 pos, objPos = new Vector3();
        //Quaternion rot, objRot = new Quaternion();
        //if (args.state.sourcePose.TryGetPosition(out pos))
        //{
        //    Vector3 updatedPos = new Vector3(pos.x, pos.y, pos.z + 1.0f);
        //    objPos = updatedPos;
        //    obj.transform.position = updatedPos;
        //    StatusText.text = pos.ToString();
        //}
        //if (args.state.sourcePose.TryGetRotation(out rot))
        //{
        //    obj.transform.rotation = rot;
        //    objRot = rot;
        //}
        trackingObject.Add(id, obj);
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        uint id = args.state.source.id;
        Vector3 pos;
        //Quaternion rot;
        RaycastHit hitInfo;
        try
        {
            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackingObject.ContainsKey(id))
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        var handGestureDirection = pos - cam.transform.position;//Camera.main.transform.position;
                        Vector3 CamPos = pos;//Camera.main.transform.position;
                        Vector3 updatedCamPos = new Vector3();

                        //if (handGestureDirection.x > 0)
                        //{

                        //    //updatedCamPos = new Vector3(CamPos.x - 0.25f, CamPos.y + 0.5f, CamPos.z);

                        //    //new
                        //    updatedCamPos = new Vector3(CamPos.x - 0.05f, CamPos.y + 0.15f, CamPos.z);
                        //}
                        //else
                        //{
                        //    //updatedCamPos = new Vector3(CamPos.x + 0.25f, CamPos.y + 0.5f, CamPos.z);

                        //    //new
                        //    updatedCamPos = new Vector3(CamPos.x + 0.05f, CamPos.y + 0.15f, CamPos.z);
                        //}
                        //for new experiment
                        //updatedCamPos = new Vector3(CamPos.x - 0.04f, CamPos.y + 0.13f, CamPos.z);
                        updatedCamPos = new Vector3(CamPos.x/*-0.02f*/, CamPos.y + 0.15f, CamPos.z);

                        Physics.Raycast(updatedCamPos, handGestureDirection, out hitInfo);

                        //old experiment
                        //if (hitInfo.collider != null)
                        //{
                        //    CurFocusedObj = hitInfo.collider.gameObject;

                        //    trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized) * hitInfo.distance * 0.95F;

                        //    //trackingObject[id].transform.position = new Vector3(_crs1.transform.position.x, _crs1.transform.position.y, _crs1.transform.position.z);
                        //}
                        //else
                        //{
                        //    //CurFocusedObj = null;
                        //    trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized * 0.2F);

                        //    //trackingObject[id].transform.position = new Vector3(_crs1.transform.position.x, _crs1.transform.position.y, _crs1.transform.position.z);
                        //}

                        //new experiment
                        trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized * 0.1F);
                        //StatusText.text = "No Coll: " + trackingObject[id].transform.position.ToString();

                        //if (hitInfo.collider != null)
                        //{
                        //    trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized) * hitInfo.distance * 0.95F;

                        //    //Debug.DrawRay(updatedCamPos, (updatedCamPos - trackingObject[id].transform.position), Color.white);
                        //}
                        //else
                        //{
                        //    trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized * 2F);

                        //    //Debug.DrawRay(updatedCamPos, (updatedCamPos - trackingObject[id].transform.position), Color.white);
                        //}
                        //trackingObject[id].transform.LookAt(Camera.main.transform);
                    }
                }
            }
        }
        catch (Exception e)
        {
            //StatusText.text = e.Message.ToString() + "\n" + e.Source.ToString();
        }
    }
    private void Update()
    {

    }
    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        if (trackedHands.Contains(id))
        {
            trackedHands.Remove(id);
        }

        if (trackingObject.ContainsKey(id))
        {
            var obj = trackingObject[id];
            trackingObject.Remove(id);
            Destroy(obj);
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
        }

        //StatusText.text = "Hand Lost";
    }






    //private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    //{
    //    _handTracked = true;
    //    if (showHandCrsr)
    //    {
    //        _crs1.SetActive(true);
    //    }
    //}

    //private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    //{
    //    RaycastHit hitInfo;
    //    Vector3 handGesturePos;
    //    if (args.state.sourcePose.TryGetPosition(out handGesturePos))
    //    {
    //        var handGestureDirection = handGesturePos - Camera.main.transform.position;
    //        info.text = "x:" + handGestureDirection.normalized.x.ToString() + ", y:" + handGestureDirection.normalized.y.ToString() + ", z:" + handGestureDirection.normalized.z.ToString();
    //        Vector3 CamPos = handGesturePos;//Camera.main.transform.position;
    //        Vector3 updatedCamPos = new Vector3(CamPos.x-0.25f, CamPos.y + 0.5f, CamPos.z);
    //        Physics.Raycast(updatedCamPos, handGestureDirection, out hitInfo);
    //        if (hitInfo.collider != null)
    //        {
    //            CurFocusedObj = hitInfo.collider.gameObject;
    //            if (CurFocusedObj != null)
    //            {
    //                CurFocusedObj.SendMessage("Highlight", this as HandInteractionMaster);
    //            }
    //            if (showHandCrsr)
    //            {
    //                _crs1.transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized) * hitInfo.distance * 0.95F;
    //                _crs1.transform.position = new Vector3(_crs1.transform.position.x, _crs1.transform.position.y, _crs1.transform.position.z);
    //            }
    //        }
    //        else
    //        {
    //            CurFocusedObj = null;
    //            if (showHandCrsr)
    //            {
    //                _crs1.transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized * 2F);
    //                _crs1.transform.position = new Vector3(_crs1.transform.position.x, _crs1.transform.position.y, _crs1.transform.position.z);
    //            }
    //        }
    //    }
    //}

    //private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    //{
    //    _handTracked = false;
    //    CurFocusedObj = null;
    //    if (showHandCrsr)
    //    {
    //        _crs1.SetActive(false);
    //    }
    //}
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        if (CurFocusedObj != null)
        {
            CurFocusedObj.SendMessage("Select");
        }
    }
    //void Start()
    //{
    //    if (showHandCrsr)
    //    {
    //        _crs1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //        _crs1.transform.localScale = new Vector3(0.04F, 0.04F, 0.04F);
    //        _crs1.GetComponent<Renderer>().material.color = Color.green;
    //        _crs1.GetComponent<Collider>().enabled = false;
    //        _crs1.transform.parent = transform;
    //        _crs1.SetActive(false);
    //    }
    //}

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (CurFocusedObj != null)
        {
            CurFocusedObj.SendMessage("Select");
        }
    }
}