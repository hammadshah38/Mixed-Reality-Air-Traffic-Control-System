using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR.WSA.WebCam;

/// <summary>
/// Manages taking and saving photos.
/// </summary>
/// 
//public class PhotoManager : MonoBehaviour
//{
//    public TextMesh Info;
//    public bool ShowHolograms = true;
//    public bool AutoStart = true;
//    PhotoCapture photoCaptureObject = null;

//    void Start()
//    {
//        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

//        // Create a PhotoCapture object
//        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
//            photoCaptureObject = captureObject;
//            CameraParameters cameraParameters = new CameraParameters();
//            cameraParameters.hologramOpacity = 0.0f;
//            cameraParameters.cameraResolutionWidth = cameraResolution.width;
//            cameraParameters.cameraResolutionHeight = cameraResolution.height;
//            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

//            // Activate the camera
//            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
//                // Take a picture
//                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
//            });
//        });
//    }
//}
public class PhotoManager : MonoBehaviour
{
    /// <summary>
    /// Displays status information of the camera.
    /// </summary>
    public TextMesh Info, Info1;
    private HashSet<uint> trackedHands = new HashSet<uint>(); private uint activeId;
    /// <summary>
    /// Whether or not to show holograms in the photo.
    /// </summary>
    public bool ShowHolograms = true;

    /// <summary>
    /// Whether or not to start the camera immediatly.
    /// </summary>
    public bool AutoStart = true;

    /// <summary>
    /// Actual camera instance.
    /// </summary>
    private PhotoCapture capture;

    /// <summary>
    /// True, if the camera is ready to take photos.
    /// </summary>
    private bool isReady = false;

    /// <summary>
    /// The path to the image in the applications local folder.
    /// </summary>
    private string currentImagePath;

    /// <summary>
    /// The path to the users picture folder.
    /// </summary>
    private string pictureFolderPath;

    void Awake()
    {
        //photo.StartCamera();
        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;

        Assert.IsNotNull(Info, "The PhotoManager requires a text mesh.");

        Info.text = "Camera off";

        if (AutoStart)
            StartCamera();

#if NETFX_CORE
        
        GetPicturesFolderAsync();
#endif
    }
    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        uint id = args.state.source.id;

        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }
        trackedHands.Add(id);
        activeId = id;

        //Info.text = "Hand Detected";
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        
    }
    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        uint id = args.state.source.id;

        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        if (trackedHands.Contains(id))
        {
            trackedHands.Remove(id);
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
        }
    }
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        TakePhoto();
    }
    private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs args)
    {

    }
    private void Start()
    {
        

    }

    /// <summary>
    /// Starts the photo mode.
    /// </summary>
    public void StartCamera()
    {
        if (isReady)
        {
            Debug.Log("Camera is already running.");
            return;
        }

        PhotoCapture.CreateAsync(ShowHolograms, OnPhotoCaptureCreated);
    }

    /// <summary>
    /// Take a photo and save it to a temporary application folder.
    /// </summary>
    public void TakePhoto()
    {
        if (isReady)
        {
            Info.text = "Taking Photo";
            string file = string.Format(@"Image_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg", DateTime.Now);
            currentImagePath = System.IO.Path.Combine(Application.persistentDataPath, file);
            capture.TakePhotoAsync(currentImagePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            //StopCamera();
            //isReady = false;
            //StartCamera();
        }
        else
        {
            Info1.text = "The camera is not yet ready.";
            Debug.LogWarning("The camera is not yet ready.");
        }
    }

    /// <summary>
    /// Stop the photo mode.
    /// </summary>
    public void StopCamera()
    {
        if (isReady)
        {
            capture.StopPhotoModeAsync(OnPhotoModeStopped);
        }
    }

#if NETFX_CORE

    private async void GetPicturesFolderAsync() 
    {
        Info1.text = "Getting Folder Async";
        Windows.Storage.StorageLibrary picturesStorage = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        pictureFolderPath = picturesStorage.SaveFolder.Path;
    }

#endif

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        capture = captureObject;

        Resolution resolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();

        CameraParameters c = new CameraParameters(WebCamMode.PhotoMode);
        c.hologramOpacity = 1.0f;
        c.cameraResolutionWidth = resolution.width;
        c.cameraResolutionHeight = resolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        capture.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        isReady = result.success;
        Info.text = isReady ? "Camera ready" : "Camera failed to start";
    }

    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {

#if NETFX_CORE
            try 
            {
                if(pictureFolderPath != null)
                {
                    System.IO.File.Move(currentImagePath, System.IO.Path.Combine(pictureFolderPath, "Camera Roll", System.IO.Path.GetFileName(currentImagePath)));
                    Info1.text = "Saved photo in camera roll";
                    capture.StopPhotoModeAsync(OnPhotoModeStopped);
                }
                else 
                {
                    Info1.text = "Saved photo to temp";
                }
            } 
            catch(Exception e) 
            {
                Info.text = "Failed to move to camera roll";
                Debug.Log(e.Message);
            }
#else
            Info1.text = "Saved photo";
            capture.StopPhotoModeAsync(OnPhotoModeStopped);
            Debug.Log("Saved image at " + currentImagePath);
#endif

        }
        else
        {
            Info1.text = "Failed to save photo";
            Debug.LogError(string.Format("Failed to save photo to disk ({0})", result.hResult));
        }
    }

    private void OnPhotoModeStopped(PhotoCapture.PhotoCaptureResult result)
    {
        capture.Dispose();
        capture = null;
        isReady = false;

        Info.text = "Camera off";

        StartCamera();
    }

}