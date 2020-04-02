using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class TickingMachine : MonoBehaviour
    {
        public static ulong ticks;
        [SerializeField] float tickTime;
        public static event System.Action EveryTick;
        public static event System.Action TenthTick;
        float time;
        void Start()
        {
            time = 0;
        }
        void Update()
        {
            time += Time.deltaTime;
            if(time>tickTime)
            {
                time -= tickTime;
                OneMoreTick();
            }
        }
        void OneMoreTick()
        {
            ticks++;
            EveryTick?.Invoke();
            if (ticks % 10 == 0)
                TenthTick?.Invoke();
        }
    }
}