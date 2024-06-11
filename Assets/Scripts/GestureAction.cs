// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA;

namespace Academy
{
    /// <summary>
    /// GestureAction performs custom actions based on 
    /// which gesture is being performed.
    /// </summary>
    public class GestureAction : MonoBehaviour, INavigationHandler, IManipulationHandler, ISpeechHandler
    {
        [Tooltip("Rotation max speed controls amount of rotation.")]
        [SerializeField]
        private float RotationSensitivity = 10.0f;
        public TextMesh txt;
        //for ATC
        public GameObject loader;
        public GameObject cursor,child;
        //custom code
        public static bool isAdjusting = false;


        //private void Start()
        //{
        //    //    string activeScene = SceneManager.GetActiveScene().name;
        //    //    Debug.Log(activeScene + OldPositions.AirportPos.ToString());
        //    //    if (activeScene == "MapView")
        //    //    {
        //    //        if (OldPositions.MapPos != new Vector3(0.0f, 0.0f, 0.0f))
        //    //        {
        //    //            this.transform.position = OldPositions.MapPos;
        //    //            loader.SetActive(true);
        //    //            cursor.SetActive(false);
        //    //        }
        //    //    }
        //    //    else if (activeScene == "ModelExplorer")
        //    //    {
        //    //        if (OldPositions.AirportPos != new Vector3(0.0f, 0.0f, 0.0f))
        //    //        {
        //    //            this.transform.position = OldPositions.AirportPos;
        //    //            loader.SetActive(true);
        //    //            cursor.SetActive(false);
        //    //        }
        //    //    }
        //    try
        //    {
        //        string[] ids = WorldAnchorManager.Instance.AnchorStore.GetAllIds();
        //        string name = null;
        //        string nameChild = null;

        //        if (ids.Length > 1)
        //        {
        //            name = WorldAnchorManager.Instance.AttachAnchor(this.gameObject);
        //            //nameChild = WorldAnchorManager.Instance.AttachAnchor(child.gameObject);
        //        }
        //        if (name != null)
        //        {
        //            loader.SetActive(true);
        //            cursor.SetActive(false);
        //            txt.text = "Anchor from gesture script " + name + " ids " + ids.Length + " child " + nameChild;
        //        }
        //        else
        //        {
        //            txt.text = "No Anchor from gesture script " + " ids " + ids.Length;
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        txt.text = e.ToString();
        //    }
            
        //}
        public static bool adjusting
        {
            get
            {
                return isAdjusting;
            }
            set
            {
                isAdjusting = value;
            }
        }
        //

        private bool isNavigationEnabled = false;//true for rotating
        public bool IsNavigationEnabled
        {
            get { return isNavigationEnabled; }
            set { isNavigationEnabled = value; }
        }

        private Vector3 manipulationOriginalPosition = Vector3.zero;

        void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
        }

        void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
        {
            if (isNavigationEnabled)
            {
                /* TODO: DEVELOPER CODING EXERCISE 2.c */

                // 2.c: Calculate a float rotationFactor based on eventData's NormalizedOffset.x multiplied by RotationSensitivity.
                // This will help control the amount of rotation.


                // 2.c: transform.Rotate around the Y axis using rotationFactor.


                // 2.c: Calculate a float rotationFactor based on eventData's NormalizedOffset.x multiplied by RotationSensitivity.
                // This will help control the amount of rotation.
                float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;

                // 2.c: transform.Rotate around the Y axis using rotationFactor.
                transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
            }
        }

        void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            try
            {
                if (!isNavigationEnabled)
                {
                    InputManager.Instance.PushModalInputHandler(gameObject);

                    manipulationOriginalPosition = transform.position;

                    //WorldAnchorManager.Instance.RemoveAnchor(this.gameObject);
                    //WorldAnchorManager.Instance.RemoveAnchor(child);
                    WorldAnchor a = this.GetComponent<WorldAnchor>();
                    if (a != null)
                    {
                        Destroy(a);
                    }
                }
            }
            catch(Exception e)
            {
                txt.text = e.ToString();
            }
            
        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (!isNavigationEnabled)
            {
                /* TODO: DEVELOPER CODING EXERCISE 4.a */
                // 4.a: Make this transform's position be the manipulationOriginalPosition + eventData.CumulativeDelta
                transform.position = manipulationOriginalPosition + eventData.CumulativeDelta;

                if (this.gameObject.name == "Airport(New)")
                {
                    child.transform.position = new Vector3(this.transform.position.x - 0.14f, this.transform.position.y + 1.0618f, this.transform.position.z + 2.8f);
                }
                else
                {
                    child.transform.position = new Vector3(this.transform.position.x + 0.1f, this.transform.position.y - 0.041f, this.transform.position.z + 0.38f);
                }

            }
        }

        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {
            try
            {
                InputManager.Instance.PopModalInputHandler();
                //ATC Loader
                loader.SetActive(true);
                cursor.SetActive(false);
                //if (this.gameObject.name == "Airport(New)")
                //{
                //    child.transform.position = new Vector3(this.transform.position.x - 0.14f, this.transform.position.y + 1.0618f, this.transform.position.z + 2.8f);
                //}
                //else
                //{
                //    child.transform.position = new Vector3(this.transform.position.x + 0.1f, this.transform.position.y - 0.041f, this.transform.position.z + 0.38f);
                //}
                //WorldAnchorManager.Instance.AttachAnchor(this.gameObject, this.gameObject.name);
                //WorldAnchorManager.Instance.AttachAnchor(child.gameObject, child.gameObject.name);
                //string activeScene = SceneManager.GetActiveScene().name;

                //if (activeScene == "MapView")
                //{
                //    OldPositions.MapPos = this.transform.position;
                //}
                //else if (activeScene == "ModelExplorer")
                //{
                //    OldPositions.AirportPos = this.transform.position;
                //}
                GameObject a = this.gameObject;
                a.AddComponent<WorldAnchor>();
                BoxCollider bc = this.GetComponent<BoxCollider>();
                Destroy(bc);
            }
            catch(Exception e)
            {
                txt.text = e.ToString();
            }
            
        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
        {
            if (eventData.RecognizedText.ToLower().Equals("move"/*astronaut"*/))
            {
                isNavigationEnabled = false;
            }
            else if (eventData.RecognizedText.ToLower().Equals("rotate"/*astronaut"*/))
            {
                isNavigationEnabled = true;
            }
            else
            {
                return;
            }

            eventData.Use();
        }
    }
}