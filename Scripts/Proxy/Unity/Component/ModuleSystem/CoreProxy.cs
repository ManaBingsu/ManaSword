using ManaSword.ModuleSystem;
using UnityEngine;

namespace ManaSword.Proxy.Unity.Component.ModuleSystem
{
    public class CoreProxy : UnityComponentProxy
    {
        [SerializeField]
        private Core core = new Core();
        public Core Core => core;
    }
}

