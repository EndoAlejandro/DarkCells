using System;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action<bool> OnSetInputEnabled;

        [ContextMenu("Activate Input")]
        private void ActivateInput() => OnSetInputEnabled?.Invoke(true);

        [ContextMenu("Deactivate Input")]
        private void DeactivateInput() => OnSetInputEnabled?.Invoke(false);
    }
}
