using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class FunctionsInvoker
    {
        static GameObject globalGO;
        static List<DelayAction> actionsWithDelay;
        static List<UpdateAction> updateActions;
        private class GlobalTimer:MonoBehaviour
        {
            private void Update()
            {
                for(int i = 0;i< actionsWithDelay.Count;i++)
                {
                    actionsWithDelay[i].timer -= actionsWithDelay[i].useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    if (actionsWithDelay[i].timer < 0)
                    {
                        actionsWithDelay[i].action();
                        actionsWithDelay.RemoveAt(i);
                        i--;
                    }
                }
                for(int i = 0; i< updateActions.Count;i++)
                {
                    if (updateActions[i].action())
                    {
                        updateActions.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        private class DelayAction
        {
            public System.Action action;
            public bool useUnscaledTime;
            public float timer;
        }
        private class UpdateAction
        {
            public System.Func<bool> action;
        }
        static void InitIfNeed()
        {
            if (globalGO == null)
            {
                globalGO = new GameObject("GlobalTimer", typeof(GlobalTimer));
                actionsWithDelay = new List<DelayAction>();
                updateActions = new List<UpdateAction>();
            }
        }
        public static void AddActionWithDelay(System.Action action, float time, bool useUnscaledTime)
        {
            InitIfNeed();
            actionsWithDelay.Add(new DelayAction() { action = action, timer = time, useUnscaledTime= useUnscaledTime});
        }
        public static void AddUpdateAction(System.Func<bool> action)
        {
            InitIfNeed();
            updateActions.Add(new UpdateAction() { action = action });
        }
    }
    
}