using UnityEngine;
using UnityEngine.Networking;
using System;
using Cysharp.Threading.Tasks;

public class GetServerTime : MonoBehaviour
{
    // URL to your Firebase function
    private string serverTimeUrl = "https://getservertime-gbcj4ggvba-uc.a.run.app";

    void Start()
    {
    }
    public async UniTask GetServerTimeFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverTimeUrl))
        {
            await webRequest.SendWebRequest().ToUniTask();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string responseText = webRequest.downloadHandler.text;
                Debug.Log("Server Time Response: " + responseText);

                // Deserialize the JSON response into a C# object
                ServerTimeResponse serverTimeResponse = JsonUtility.FromJson<ServerTimeResponse>(responseText);

                if (serverTimeResponse != null && serverTimeResponse.time != null)
                {
                    long seconds = serverTimeResponse.time._seconds;
                    int nanoseconds = serverTimeResponse.time._nanoseconds;
                    DateTime serverTime = DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
                    serverTime = serverTime.AddTicks(nanoseconds / 100);  // Convert nanoseconds to ticks

                    Debug.Log("Server Time: " + serverTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    Debug.LogError("Invalid time data received.");
                }
            }
            else
            {
                Debug.LogError("Failed to get server time: " + webRequest.error);
            }
        }
    }

    public async UniTaskVoid GetTime()
    {
        await GetServerTimeFromServer();
    }

    [System.Serializable]
    public class ServerTimeResponse
    {
        public ServerTime time;
    }

    [System.Serializable]
    public class ServerTime
    {
        public long _seconds; 
        public int _nanoseconds;
    }
}
