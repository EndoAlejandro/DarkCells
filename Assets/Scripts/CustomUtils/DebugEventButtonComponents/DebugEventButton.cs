using System;
using UnityEngine;
using UnityEngine.Events;

namespace DarkHavoc.CustomUtils.DebugEventButtonComponents
{
    [Serializable]
    public class DebugEventButton
    {
        [Button(nameof(InvokeEvents))]
        [SerializeField] private bool _;

        public UnityEvent events;
        public void InvokeEvents() => events?.Invoke();
    }
}