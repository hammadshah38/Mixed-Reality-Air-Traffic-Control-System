using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

public class CustomHandInteraction : MonoBehaviour {

    public bool HandDetected
    {
        get { return trackedHands.Count > 0; }
    }

    public GameObject TrackingObject;
    public TextMesh StatusText;

    private HashSet<uint> trackedHands = new HashSet<uint>();
    private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();

    private uint activeId;

    void Awake()
    {
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;
        StatusText.text = "READY";
    }
    private void Start()
    {
    }


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

        Vector3 pos, objPos = new Vector3();
        Quaternion rot, objRot = new Quaternion();
        if (args.state.sourcePose.TryGetPosition(out pos))
        {
            Vector3 updatedPos = new Vector3(pos.x, pos.y, pos.z + 1.0f);
            objPos = updatedPos;
            obj.transform.position = updatedPos;
            StatusText.text = pos.ToString();
        }
        if (args.state.sourcePose.TryGetRotation(out rot))
        {
            obj.transform.rotation = rot;
            objRot = rot;
        }
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
                    var handGestureDirection = pos - Camera.main.transform.position;
                    Vector3 CamPos = pos;//Camera.main.transform.position;
                    Vector3 updatedCamPos = new Vector3(CamPos.x - 0.25f, CamPos.y + 0.5f, CamPos.z);
                    Physics.Raycast(updatedCamPos, handGestureDirection, out hitInfo);

                    if (hitInfo.collider != null)
                    {
                        trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized) * hitInfo.distance * 0.95F;

                        Debug.DrawRay(updatedCamPos, (updatedCamPos - trackingObject[id].transform.position), Color.white);
                    }
                    else
                    {
                        trackingObject[id].transform.position = /*Camera.main.transform.position*/updatedCamPos + (handGestureDirection.normalized * 2F);

                        Debug.DrawRay(updatedCamPos, (updatedCamPos - trackingObject[id].transform.position), Color.white);
                    }
                    trackingObject[id].transform.LookAt(Camera.main.transform);
                }
            }
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
        
        StatusText.text = "Hand Lost";
    }
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        //Transform ct = cursor.transform;
        //StatusText.text = "Source Pressed, CTp: "+ ct.transform.position.ToString();
        
        //Transform ct = cursor.transform;
        //thisTrail = (GameObject)Instantiate(trailPrefab, ct.transform.position, ct.transform.rotation);
    }
    private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs args)
    {
        
        //StatusText.text = "Source Released";
    }
    void OnDestroy()
    {
        InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourceReleased -= InteractionManager_InteractionSourceReleased;
    }
}
