using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class Pull<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T[] array;
        static int index;
        [SerializeField] int size;
        [SerializeField] GameObject Ref;
        void Start()
        {
            array = new T[size];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Instantiate(Ref, transform).GetComponent<T>();
                array[i].gameObject.SetActive(false);
            }
        }
        public static T Next()
        {
            index = (index + 1) % array.Length;
            return array[index];
        }
    }
}
