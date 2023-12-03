using System;
using UnityEngine;

namespace DarkHavoc.ServiceLocatorComponents
{
    public abstract class Service<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected virtual bool DonDestroyOnLoad => false;
        protected bool _canRemoveService;

        protected virtual void Awake()
        {
            if (!ServiceLocator.Instance.TryToRegisterService(this as T))
            {
                Destroy(gameObject);
                return;
            }

            _canRemoveService = true;
            if (DonDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_canRemoveService) ServiceLocator.Instance.RemoveService(this as T);
        }
    }
}