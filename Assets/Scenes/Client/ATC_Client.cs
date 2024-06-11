using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

#if !UNITY_EDITOR
using System.Threading.Tasks;
#endif

public class ATC_Client : MonoBehaviour
{
#if !UNITY_EDITOR
    private bool _useUWP = true;
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
#endif

#if UNITY_EDITOR
    private bool _useUWP = false;
    System.Net.Sockets.TcpClient client;
    System.Net.Sockets.NetworkStream stream;
    private Thread exchangeThread;
#endif

    private Byte[] bytes = new Byte[256];
    private StreamWriter writer;
    private StreamReader reader;
    public string message;

    public ATC_Client(string m)
    {
        message = m;
    }

    public void Connect(string host, string port)
    {
        if (_useUWP)
        {
            ConnectUWP(host, port);
        }
        else
        {
            ConnectUnity(host, port);
        }
    }



#if UNITY_EDITOR
    private void ConnectUWP(string host, string port)
#else
    private async void ConnectUWP(string host, string port)
#endif
    {
#if UNITY_EDITOR
        errorStatus = "UWP TCP client used in Unity!";
#else
        try
        {
            if (exchangeTask != null) StopExchange();
        
            socket = new Windows.Networking.Sockets.StreamSocket();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
            await socket.ConnectAsync(serverHost, port);
        
            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            writer = new StreamWriter(streamOut) { AutoFlush = true };
        
            Stream streamIn = socket.InputStream.AsStreamForRead();
            reader = new StreamReader(streamIn);

            RestartExchange();
            successStatus = "Connected!";
        }
        catch (Exception e)
        {
            errorStatus = e.ToString();
        }
#endif
    }

    public void ConnectUnity(string host, string port)
    {
#if !UNITY_EDITOR
        errorStatus = "Unity TCP client used in UWP!";
#else
        try
        {
            if (exchangeThread != null) StopExchange();

            client = new System.Net.Sockets.TcpClient(host, Int32.Parse(port));
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            RestartExchange();
            successStatus = "Connected!";
        }
        catch (Exception e)
        {
            errorStatus = e.ToString();
        }
#endif
    }

    private bool exchanging = false;
    private bool exchangeStopRequested = false;
    private string lastPacket = null;

    private string errorStatus = null;
    private string warningStatus = null;
    private string successStatus = null;
    private string unknownStatus = null;

    public void RestartExchange()
    {
#if UNITY_EDITOR
        if (exchangeThread != null) StopExchange();
        exchangeStopRequested = false;
        exchangeThread = new System.Threading.Thread(ExchangePackets);
        exchangeThread.Start();
#else
        if (exchangeTask != null) StopExchange();
        exchangeStopRequested = false;
        exchangeTask = Task.Run(() => ExchangePackets());
#endif
    }

    public void Update()
    {
        if (lastPacket != null)
        {
            //ReportDataToTrackingManager(lastPacket);
        }

        if (errorStatus != null)
        {
            this.GetComponent<TextMesh>().text = errorStatus;
            errorStatus = null;
        }
        if (warningStatus != null)
        {
            this.GetComponent<TextMesh>().text = errorStatus;
            warningStatus = null;
        }
        if (successStatus != null)
        {
            this.GetComponent<TextMesh>().text = errorStatus;
            successStatus = null;
        }
        if (unknownStatus != null)
        {
            this.GetComponent<TextMesh>().text = errorStatus;
            unknownStatus = null;
        }
    }

    public void ExchangePackets()
    {
        while (!exchangeStopRequested)
        {
            if (writer == null || reader == null) continue;
            exchanging = true;

            writer.Write(message); //("X\n");
            //Debug.Log("Sent data!");
            Debug.Log("Sent data!" + message);
            string received = null;

#if UNITY_EDITOR
            byte[] bytes = new byte[client.SendBufferSize];
            int recv = 0;
            while (true)
            {
                recv = stream.Read(bytes, 0, client.SendBufferSize);
                received += Encoding.UTF8.GetString(bytes, 0, recv);
                if (received.EndsWith("\n")) break;
            }
#else
            received = reader.ReadLine();
#endif

            lastPacket = received;
            Debug.Log("Read data: " + received);

            exchanging = false;

            //new line added
            exchangeStopRequested = true;
        }
    }

    //private void ReportDataToTrackingManager(string data)
    //{
    //    if (data == null)
    //    {
    //        Debug.Log("Received a frame but data was null");
    //        return;
    //    }

    //    var parts = data.Split(';');
    //    foreach (var part in parts)
    //    {
    //        ReportStringToTrackingManager(part);
    //    }
    //}

    //private void ReportStringToTrackingManager(string rigidBodyString)
    //{
    //    var parts = rigidBodyString.Split(':');
    //    var positionData = parts[1].Split(',');
    //    var rotationData = parts[2].Split(',');

    //    int id = Int32.Parse(parts[0]);
    //    float x = float.Parse(positionData[0]);
    //    float y = float.Parse(positionData[1]);
    //    float z = float.Parse(positionData[2]);
    //    float qx = float.Parse(rotationData[0]);
    //    float qy = float.Parse(rotationData[1]);
    //    float qz = float.Parse(rotationData[2]);
    //    float qw = float.Parse(rotationData[3]);

    //    Vector3 position = new Vector3(x, y, z);
    //    Quaternion rotation = new Quaternion(qx, qy, qz, qw);

    //}

    public void StopExchange()
    {
        exchangeStopRequested = true;

#if UNITY_EDITOR
        if (exchangeThread != null)
        {
            exchangeThread.Abort();
            stream.Close();
            client.Close();
            writer.Close();
            reader.Close();

            stream = null;
            exchangeThread = null;
        }
#else
        if (exchangeTask != null) {
            exchangeTask.Wait();
            socket.Dispose();
            writer.Dispose();
            reader.Dispose();

            socket = null;
            exchangeTask = null;
        }
#endif
        writer = null;
        reader = null;
    }

    public void OnDestroy()
    {
        StopExchange();
    }

}