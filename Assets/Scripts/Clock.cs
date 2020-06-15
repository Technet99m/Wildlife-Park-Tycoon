using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class Clock : MonoBehaviour
{
    public static long delta;
    private void Start()
    {
        StartCoroutine(GetTime());
    }

    public static IEnumerator GetTime()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://worldtimeapi.org/api/timezone/Europe/London");
        yield return uwr.SendWebRequest();
        delta = JsonConvert.DeserializeObject<MyTime>(uwr.downloadHandler.text).datetime.Ticks - DateTime.Now.Ticks;

    }

    [Serializable]
    public class MyTime
    {
        public DateTime datetime;
    }
}
