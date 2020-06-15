using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class FunctionsInvoker
    {
        private static GameObject globalGO;
        private static List<DelayAction> actionsWithDelay;
        private static List<UpdateAction> updateActions;
        private static List<TickAction> tickActions;
        private class GlobalTimer:MonoBehaviour
        {
            private void Awake()
            {
                TickingMachine.EveryTick += CheckTickActions;
            }
            private void Update()
            {
                for(int i = 0;i< actionsWithDelay.Count;i++)
                {
                    actionsWithDelay[i].timer -= actionsWithDelay[i].useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    if (actionsWithDelay[i].timer < 0)
                    {
                        try
                        {
                            actionsWithDelay[i].action();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                        finally
                        {
                            actionsWithDelay.RemoveAt(i);
                            i--;
                        }
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
            private void CheckTickActions()
            {
                for (int i = 0; i < tickActions.Count; i++)
                {
                    tickActions[i].ticks--;
                    if (tickActions[i].ticks <= 0)
                    {
                        try
                        {
                            tickActions[i].action();
                        }
                        catch(System.Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                        finally
                        {
                            tickActions.RemoveAt(i);
                            i--;
                        }
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
        private class TickAction
        {
            public int ticks;
            public System.Action action;
        }
        static void InitIfNeed()
        {
            if (globalGO == null)
            {
                globalGO = new GameObject("GlobalTimer", typeof(GlobalTimer));
                actionsWithDelay = new List<DelayAction>();
                updateActions = new List<UpdateAction>();
                tickActions = new List<TickAction>();
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
        public static void AddTickAction(System.Action action, int ticks)
        {
            InitIfNeed();
            tickActions.Add(new TickAction() { ticks = ticks, action = action });
        }
    }
    
}