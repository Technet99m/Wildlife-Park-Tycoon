using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class TickingMachine : MonoBehaviour
    {
        public static ulong ticks;
        public static event System.Action EveryTick;
        public static event System.Action TenthTick;

        [SerializeField] float tickTime;
        private float time;
        private void Start()
        {
            time = 0;
        }
        private void Update()
        {
            time += Time.deltaTime;
            if(time>tickTime)
            {
                time -= tickTime;
                OneMoreTick();
            }
        }
        public static void OneMoreTick()
        {
            ticks++;
            EveryTick?.Invoke();
            if (ticks % 10 == 0)
                TenthTick?.Invoke();
        }
    }
}