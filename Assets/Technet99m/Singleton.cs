using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;
        public static T Ins { get { return instance; } }

        private void Awake()
        {
            if (Ins == null)
                instance = (T)FindObjectOfType(typeof(T));
            else
            {
                Debug.LogError($"Instance of {nameof(instance)} already exists. Destroying");
                Destroy(this);
            }
        }
    }
}
