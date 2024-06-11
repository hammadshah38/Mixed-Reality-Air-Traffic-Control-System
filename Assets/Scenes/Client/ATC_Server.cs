using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
#endif

//Able to act as a reciever 
public class ATC_Server : MonoBehaviour
{
    public String _input = "Waiting";

#if !UNITY_EDITOR
        StreamSocket socket;
        StreamSocketListener listener;
        String port;
        String message;
#endif

    // Use this for initialization
    void Start()
    {
#if !UNITY_EDITOR
        try
        {
            listener = new StreamSocketListener();
            port = "9090";
            listener.ConnectionReceived += Listener_ConnectionReceived;
            listener.Control.KeepAlive = false;

            Listener_Start();
        }
        catch (Exception e)
        {
            Debug.Log("Error in Start: " + e.Message);
            //String in = "Error in Start: " + e.Message;
            //_input = in;
        }
#endif
    }

#if !UNITY_EDITOR
    private async void Listener_Start()
    {
        Debug.Log("Listener started");
        try
        {
            await listener.BindServiceNameAsync(port);
        }
        catch (Exception e)
        {
            Debug.Log("Error in Listener Start: " + e.Message);
            //String in = "Error in Listener Start: " + e.Message;
            //_input = in;
        }

        Debug.Log("Listening");
    }

    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
    {
        Debug.Log("Connection received");

        try
        {
            while (true) 
            {

                using (var dw = new DataWriter(args.Socket.OutputStream))
                {
                    dw.WriteString("Sent from Hololens");
                    await dw.StoreAsync();
                    dw.DetachStream();
                }  
                    
                using (var dr = new DataReader(args.Socket.InputStream))
                {
                    dr.InputStreamOptions = InputStreamOptions.Partial;
                    await dr.LoadAsync(70);
                    var input = dr.ReadString(70);
                    Debug.Log("received: " + input);
                    _input = input;
                    string[] a = _input.Split('d');
                    _input = a[0];
                }
            }
            
        }
        catch (Exception e)
        {
            Debug.Log("disconnected!!!!!!!! " + e);
            //String in = "disconnected!!!!!!!! " + e.Message;
            //_input = in;
        }

    }

#endif

    void Update()
    {
        this.GetComponent<TextMesh>().text = _input;
        string a = "000056565";

        Debug.Log(Convert.ToInt32(a));
    }
}