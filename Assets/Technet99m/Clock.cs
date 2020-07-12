using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using System.Collections;

namespace Technet99m
{
    public class Clock : MonoBehaviour
    {
        public static long delta;
        public static event Action deltaActualized;
        public static event Action firstDeltaActualized;

        private static bool first = true;
        private void Start()
        {
            StartCoroutine(GetTime());
            TickingMachine.TenthTick += () => StartCoroutine(GetTime());
        }

        public static IEnumerator GetTime()
        {
            UnityWebRequest uwr = UnityWebRequest.Get("https://worldtimeapi.org/api/timezone/Europe/London");
            uwr.timeout = 5;
            yield return uwr.SendWebRequest();
            delta = JsonConvert.DeserializeObject<MyTime>(uwr.downloadHandler.text).datetime.Ticks - DateTime.Now.Ticks;
            Debug.Log("Delta Time actualized");
            if(first)
            {
                first = false;
                firstDeltaActualized?.Invoke();
                yield break;
            }
            deltaActualized?.Invoke();
        }

        [Serializable]
        public class MyTime
        {
            public DateTime datetime;
        }
    }
}