using UnityEngine;
using UnityEngine.Events;

namespace Technet99m
{
    public class AnimationAction : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent[] actions;


        public void Action(int i)
        {
            if (i < actions.Length)
                actions[i]?.Invoke();
        }
    }
}
