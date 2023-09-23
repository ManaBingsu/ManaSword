using ManaSword.ModuleSystem;
using ManaSword.Physics.Time;
using ManaSword.Proxy.Unity.Component.ModuleSystem;

using UnityEngine;

namespace ManaSword
{
    [RequireComponent(typeof(CoreProxy))]
    public class Character : MonoBehaviour
    {
        [SerializeField]
        protected Timer timer;
        public Timer Timer => timer;

        [SerializeField]
        protected CoreProxy coreProxy;

        private void Reset()
        {
            coreProxy = GetComponent<CoreProxy>();
        }

        private void Awake()
        {
            if (coreProxy == null)
                coreProxy = GetComponent<CoreProxy>();
        }
    }
}

