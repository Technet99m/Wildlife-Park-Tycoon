using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class Utils
    {
        private static Camera cam;
        public static Camera Cam
        {
            get
            { 
                if (cam == null) cam = Camera.main;
                return cam;
            }
        }

        /// <summary>
        /// Same as Camera.ScreenToWorldPoint but with z = 0
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 ScreenToWorldPoint(Vector3 point)
        {
            var tmp = Cam.ScreenToWorldPoint(point);
            return new Vector3(tmp.x, tmp.y, 0);
        }
        public static Ray ScreenPointToRay(Vector3 point)
        {
            return Cam.ScreenPointToRay(point);
        }
        public static Vector3 WorldToScreenPoint(Vector3 point)
        {
            return Cam.WorldToScreenPoint(point);
        }
        public static Vector2 GetCameraSizeInWorldSpace()
        {
            if (!Cam.orthographic)
                return default;
            return Cam.ViewportToWorldPoint(new Vector3(1, 1)) - Cam.ViewportToWorldPoint(new Vector3(0f, 0f));
        }
        /// <summary>
        /// Performs action in delay
        /// </summary>
        /// <param name="action">action to be performed</param>
        /// <param name="time">delay in secons</param>
        public static void InvokeAfterDelay(System.Action action, float time)
        {
            FunctionsInvoker.AddActionWithDelay(action, time, false);
        }
        /// <summary>
        /// Performs action in delay
        /// </summary>
        /// <param name="action">action to be performed</param>
        /// <param name="time">delay in secons</param>
        /// <param name="useUnscaledTime">using uscaled time or scaled</param>
        public static void InvokeAfterDelay(System.Action action, float time, bool useUnscaledTime)
        {
            FunctionsInvoker.AddActionWithDelay(action, time, useUnscaledTime);
        }
        /// <summary>
        /// Performs action every frame till it returns true
        /// </summary>
        /// <param name="action">action to be performed</param>
        public static void AddUpdateAction(System.Func<bool> action)
        {
            FunctionsInvoker.AddUpdateAction(action);
        }
        /// <summary>
        /// Performs action after ticks
        /// </summary>
        /// <param name="action">action to be performed</param>
        /// <param name="ticks">delay in ticks</param>
        public static void AddTickAction(System.Action action, int ticks)
        {
            FunctionsInvoker.AddTickAction(action,ticks);
        }
    }
}