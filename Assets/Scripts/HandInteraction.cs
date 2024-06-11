using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using System;
using UnityEngine.UI;

public class HandInteraction : MonoBehaviour
{

    bool _handTracked = false;
    private GameObject _crs;
    public TextMesh posi;
    public bool showHandCrsr = true;

    public GameObject CurFocusedObj { get; set; }

    // Use this for initialization
    void Awake()
    {
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;

        //InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        //InteractionManager.SourceLost += InteractionManager_SourceLost;
        //InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        //InteractionManager.SourcePressed += InteractionManager_SourcePressed;
    }
    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        _handTracked = true;
        if (showHandCrsr)
        {
            _crs.SetActive(true);
        }
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        RaycastHit hitInfo;
        Vector3 handGesturePos;
        if (args.state.sourcePose.TryGetPosition(out handGesturePos))
        {
            var handGestureDirection = handGesturePos - Camera.main.transform.position;
            Vector3 CamPos = handGesturePos;//Camera.main.transform.position;
            Vector3 updatedCamPos = new Vector3(CamPos.x - 0.25f, CamPos.y + 0.5f, CamPos.z);

            _crs.transform.position = /*Camera.main.transform.position*/CamPos + (handGestureDirection.normalized);
            posi.text = ("x: " + handGesturePos.x.ToString() + ", y: " + handGesturePos.y.ToString() + ", z: " + handGesturePos.z.ToString());
            //Physics.Raycast(updatedCamPos, handGestureDirection, out hitInfo);
            //if (hitInfo.collider != null)
            //{
            //    CurFocusedObj = hitInfo.collider.gameObject;
            //    if (CurFocusedObj != null)
            //    {
            //        CurFocusedObj.SendMessage("Highlight", this as HandInteractionMaster);
            //    }
            //    if (showHandCrsr)
            //    {
            //        _crs.transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized) * hitInfo.distance * 0.95F;
            //        _crs.transform.position = new Vector3(_crs.transform.position.x, _crs.transform.position.y, _crs.transform.position.z);
            //    }
            //}
            //else
            //{
            //    CurFocusedObj = null;
            //    if (showHandCrsr)
            //    {
            //        _crs.transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized);// * 2F);
            //        _crs.transform.position = new Vector3(_crs.transform.position.x, _crs.transform.position.y, _crs.transform.position.z);
            //    }
            //}
        }
    }

    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        _handTracked = false;
        CurFocusedObj = null;
        if (showHandCrsr)
        {
            _crs.SetActive(false);
        }
    }
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        if (CurFocusedObj != null)
        {
            CurFocusedObj.SendMessage("Select");
        }
    }
    void Start()
    {
        if (showHandCrsr)
        {
            _crs = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _crs.transform.localScale = new Vector3(0.04F, 0.04F, 0.04F);
            _crs.GetComponent<Renderer>().material.color = Color.green;
            _crs.GetComponent<Collider>().enabled = false;
            _crs.transform.parent = transform;
            _crs.SetActive(false);
        }
    }

    //private void InteractionManager_SourcePressed(InteractionSourceState state)
    //{
    //    if (CurFocusedObj != null)
    //    {
    //        CurFocusedObj.SendMessage("Select");
    //    }
    //}

    //private void InteractionManager_SourceUpdated(InteractionSourceState state)
    //{
    //    RaycastHit hitInfo;
    //    Vector3 handGesturePos;
    //    if (state.sourcePose.TryGetPosition(out handGesturePos))
    //    {
    //        var handGestureDirection = handGesturePos - Camera.main.transform.position;
    //        Physics.Raycast(Camera.main.transform.position, handGestureDirection, out hitInfo);
    //        if (hitInfo.collider != null)
    //        {
    //            CurFocusedObj = hitInfo.collider.gameObject;
    //            if (CurFocusedObj != null)
    //            {
    //                CurFocusedObj.SendMessage("Highlight", this as HandInteractionMaster);
    //            }
    //            if (showHandCrsr)
    //            {
    //                _crs.transform.position = Camera.main.transform.position + (handGestureDirection.normalized) * hitInfo.distance * 0.95F;
    //            }
    //        }
    //        else
    //        {
    //            CurFocusedObj = null;
    //            if (showHandCrsr)
    //            {
    //                _crs.transform.position = Camera.main.transform.position + (handGestureDirection.normalized * 2F);
    //            }
    //        }
    //    }
    //}

    //private void InteractionManager_SourceLost(InteractionSourceState state)
    //{
    //    _handTracked = false;
    //    CurFocusedObj = null;
    //    if (showHandCrsr)
    //    {
    //        _crs.SetActive(false);
    //    }
    //}

    //private void InteractionManager_SourceDetected(InteractionSourceState state)
    //{
    //    _handTracked = true;
    //    if (showHandCrsr)
    //    {
    //        _crs.SetActive(true);
    //    }
    //}

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (CurFocusedObj != null)
        {
            CurFocusedObj.SendMessage("Select");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}