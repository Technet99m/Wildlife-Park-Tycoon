﻿using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Technet99m
{
    public class Clock : MonoBehaviour
    {
        public static long ActualTime { get => delta + DateTime.Now.Ticks; }
        public static event Action deltaActualized;
        public static event Action firstDeltaActualized;

        private static long delta;
        private static bool first = true;
        private void Start()
        {
            StartCoroutine(GetTime());
            TickingMachine.TenthTick += () => StartCoroutine(GetTime());
        }

        public static IEnumerator GetTime()
        {
            //default Windows time server
            string[] ntpServers = { "0.pool.ntp.org" , "1.pool.ntp.org", "2.pool.ntp.org", "3.pool.ntp.org", "0.ua.pool.ntp.org", "1.ua.pool.ntp.org" };
            bool noErrors = false;
            while (!noErrors)
            {
                // NTP message size - 16 bytes of the digest (RFC 2030)
                var ntpData = new byte[48];

                //Setting the Leap Indicator, Version Number and Mode values
                ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServers[UnityEngine.Random.Range(0,ntpServers.Length)]).AddressList;

                //The UDP port number assigned to NTP is 123
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP uses UDP

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);

                    //Stops code hang if NTP is blocked
                    socket.ReceiveTimeout = 3000;
                    bool isError = false;
                    socket.Send(ntpData);
                    try
                    {
                        socket.Receive(ntpData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                        isError = true;
                    }
                    finally
                    {
                        socket.Close();
                    }
                    if (isError)
                        yield return null;
                }
                noErrors = true;
                //Offset to get to the "Transmit Timestamp" field (time at which the reply 
                //departed the server for the client, in 64-bit timestamp format."
                const byte serverReplyTime = 40;

                //Get the seconds part
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //Get the seconds fraction
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Convert From big-endian to little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //**UTC** time
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);
                // stackoverflow.com/a/3294698/162671
                uint SwapEndianness(ulong x)
                {
                    return (uint)(((x & 0x000000ff) << 24) +
                                   ((x & 0x0000ff00) << 8) +
                                   ((x & 0x00ff0000) >> 8) +
                                   ((x & 0xff000000) >> 24));
                }

                delta = networkDateTime.Ticks - DateTime.Now.Ticks;
                Debug.Log("Delta Time actualized");
                if (first)
                {
                    first = false;
                    firstDeltaActualized?.Invoke();
                    yield break;
                }
                deltaActualized?.Invoke();
            }
        }

        [Serializable]
        public class MyTime
        {
            public DateTime datetime;
        }
    }
}