using UnityEngine;

namespace DarkHavoc.ServiceLocatorComponents
{
    public abstract class Service<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected virtual bool DonDestroyOnLoad => false;

        protected virtual void Awake()
        {
            RegisterService();
            if (DonDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        private void RegisterService() => ServiceLocator.Instance.AddService(this as T);
        protected virtual void OnDestroy() => UnregisterService();
        private void UnregisterService() => ServiceLocator.Instance.RemoveService(this as T);
    }
}