﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

namespace HoloLensHandTracking
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public class TrackingHands : MonoBehaviour
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        public GameObject TrackingObject;
        public TextMesh StatusText;
        //public Color DefaultColor = Color.green;
        //public Color TapColor = Color.blue;
        //public Color HoldColor = Color.red;
        private bool pressed;

        private HashSet<uint> trackedHands = new HashSet<uint>();
        private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
        private GestureRecognizer gestureRecognizer;
        private uint activeId;
        private GameObject manObj, thisTrail;
        private GazeManager gazeObj;
        private GameObject cam;
        public GameObject cursor;

        void Awake()
        {
            pressed = false;
            InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
            InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
            InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;

            //gestureRecognizer = new GestureRecognizer();
            //gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
            //gestureRecognizer.Tapped += GestureRecognizerTapped;
            //gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;
            //gestureRecognizer.HoldCompleted += GestureRecognizer_HoldCompleted;
            //gestureRecognizer.HoldCanceled += GestureRecognizer_HoldCanceled;
            //gestureRecognizer.StartCapturingGestures();
            StatusText.text = "READY";

            manObj = GameObject.Find("InputManager");
            gazeObj = new GazeManager();
            
            //StatusText.text = gazeObj.GazeTransform.ToString();
            //gazeObj.MaxGazeCollisionDistance = 11;
            //gazeObj.GazeTransform = trackingObject[id].transform;
        }
        private void Start()
        {
            cam = GameObject.Find("MixedRealityCamera");
            gazeObj = (GazeManager)manObj.GetComponent<GazeManager>();
            StatusText.text = gazeObj.GazeTransform.ToString();
        }
        //void ChangeObjectColor(GameObject obj, Color color)
        //{
        //    var rend = obj.GetComponentInChildren<Renderer>();
        //    if (rend)
        //    {
        //        rend.material.color = color;
        //        Debug.LogFormat("Color Change: {0}", color.ToString());
        //    }
        //}


        //private void GestureRecognizer_HoldStarted(HoldStartedEventArgs args)
        //{
        //    uint id = args.source.id;
        //    //StatusText.text =  $"HoldStarted - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if (trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], HoldColor);
        //        //StatusText.text += "-TRACKED";
        //    }
        //}

        //private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
        //{
        //    uint id = args.source.id;
        //    //StatusText.text = $"HoldCompleted - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if (trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], DefaultColor);
        //        //StatusText.text += "-TRACKED";
        //    }
        //}

        //private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)
        //{
        //    uint id = args.source.id;
        //    //StatusText.text = $"HoldCanceled - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if (trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], DefaultColor);
        //        //StatusText.text += "-TRACKED";
        //    }
        //}

        //private void GestureRecognizerTapped(TappedEventArgs args)
        //{
        //    uint id = args.source.id;
        //    //StatusText.text = $"Tapped - Kind:{args.source.kind.ToString()} - Id:{id}";
        //    if (trackingObject.ContainsKey(activeId))
        //    {
        //        ChangeObjectColor(trackingObject[activeId], TapColor);
        //        //StatusText.text += "-TRACKED";
        //    }
        //}


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
            //obj.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            Vector3 pos, objPos= new Vector3();
            Quaternion rot, objRot = new Quaternion();
            if (args.state.sourcePose.TryGetPosition(out pos))
            {
                Vector3 updatedPos = new Vector3(pos.x, pos.y, pos.z + 1.0f);
                objPos = updatedPos;
                obj.transform.position = updatedPos;
                //StatusText.text = gazeObj.GazeTransform.ToString();//"x:" + updatedPos.x + ",y:" + updatedPos.y + ",z:" + updatedPos.z;
                StatusText.text = pos.ToString();
            }
            if (args.state.sourcePose.TryGetRotation(out rot))
            {
                obj.transform.rotation = rot;
                objRot = rot;
            }
            //gazeObj.GazeTransform = obj.transform;
            //gazeObj.OnPreRaycast();
            trackingObject.Add(id, obj);
        }

        private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
        {
            uint id = args.state.source.id;
            Vector3 pos;
            Quaternion rot;
            RaycastHit hitInfo;
            
            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackingObject.ContainsKey(id))
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        //var handGestureDirection = pos - cam.transform.position;

                        //Vector3 updatedPos = new Vector3(pos.x, pos.y, pos.z + 1.0f);

                        //trackingObject[id].transform.position = updatedPos;// + (handGestureDirection.normalized);

                        //StatusText.text = pos.ToString();
                        //StatusText.text = gazeObj.GazeTransform.ToString();//"x:" + updatedPos.x + ",y:" + updatedPos.y + ",z:" + updatedPos.z;

                        //trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedPos + (handGestureDirection.normalized * 0.55F);
                        //trackingObject[id].transform.position = new Vector3(trackingObject[id].transform.position.x, trackingObject[id].transform.position.y, trackingObject[id].transform.position.z);


                        var handGestureDirection = pos - Camera.main.transform.position;
                        gazeObj.GazeTransform.position = handGestureDirection;

                        //gazeObj.Rays[0].Direction = handGestureDirection

                        Vector3 CamPos = pos;//Camera.main.transform.position;
                        Vector3 updatedCamPos = new Vector3(CamPos.x - 0.1f, CamPos.y + 0.2f, CamPos.z);
                        Physics.Raycast(updatedCamPos, handGestureDirection, out hitInfo);

                        trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized * 0.6F);
                        //trackingObject[id].transform.position = new Vector3(trackingObject[id].transform.position.x, trackingObject[id].transform.position.y, trackingObject[id].transform.position.z);
                        
                    }
                    if (args.state.sourcePose.TryGetRotation(out rot))
                    {
                        trackingObject[id].transform.rotation = rot;
                        //StatusText.text = "Hand Rotation" + rot.ToString();
                    }
                    //gazeObj.GazeTransform = trackingObject[id].transform;
                    gazeObj.GazeTransform.rotation = trackingObject[id].transform.rotation;
                    gazeObj.OnPreRaycast();
                }                    
            }
                //if(pressed == true)
                //{
                //    Transform ct = cursor.transform;
                //    thisTrail.transform.position = ct.transform.position;
                //}
                
                //StatusText.text = "Hand Rotation" + rot.ToString();
        }
        private void Update()
        {
            StatusText.text = "x: " + cam.transform.position.x.ToString() + "y: " + cam.transform.position.y.ToString() + "z: " + cam.transform.position.z.ToString();
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
            gazeObj.GazeTransform = cam.transform;
            StatusText.text = "Hand Lost";
        }
        private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
        {
            //Transform ct = cursor.transform;
            //StatusText.text = "Source Pressed, CTp: "+ ct.transform.position.ToString();
            pressed = true;
            //Transform ct = cursor.transform;
            //thisTrail = (GameObject)Instantiate(trailPrefab, ct.transform.position, ct.transform.rotation);
        }
        private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs args)
        {
            pressed = false;
            //StatusText.text = "Source Released";
        }
        void OnDestroy()
        {
            InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;
            InteractionManager.InteractionSourceReleased -= InteractionManager_InteractionSourceReleased;

            //gestureRecognizer.Tapped -= GestureRecognizerTapped;
            //gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;
            //gestureRecognizer.HoldCompleted -= GestureRecognizer_HoldCompleted;
            //gestureRecognizer.HoldCanceled -= GestureRecognizer_HoldCanceled;
            //gestureRecognizer.StopCapturingGestures();
        }
    }
}