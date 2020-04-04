using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static Singleton<T> instance;
        public static Singleton<T> Ins { get { return instance; } }

        private void Awake()
        {
            if (Ins == null)
                instance = this;
            else
            {
                Debug.LogError($"Instance of {nameof(instance)} already exists. Destroying");
                Destroy(this);
            }
        }
    }
}
