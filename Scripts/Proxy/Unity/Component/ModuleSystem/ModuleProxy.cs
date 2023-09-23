using ManaSword.ModuleSystem;
using ManaSword.SaveLoadSystem;
using ManaSword.Utility;
using UnityEngine;

namespace ManaSword.Proxy.Unity.Component.ModuleSystem
{
    public abstract class ModuleProxy : UnityComponentProxy, IBoolstackSender, ISaveLoadEntity
    {
        [Header("Reference")]
        [SerializeField]
        protected CoreProxy coreProxy;

        [Header("Regist setting")]
        [SerializeField]
        protected bool runOnlyPassiveEventWhenRegist;

        [SerializeField]
        protected bool tryReplace;

        [Header("Inactive setting")]
        [SerializeField]
        protected bool inactive;
        [SerializeField]
        protected bool runOnlyPassiveEventWhenInactive;

        public abstract Module Module { get; }

        [SerializeField]
        protected bool isEditorDebugMode;
        public bool IsEditorDebugMode => isEditorDebugMode;

        protected void Awake()
        {
            if (Module.Inactived == null)
                Module.SetInactived(new Boolstack());

            if (isEditorDebugMode && (Application.isPlaying && Application.isEditor))
            {
                RegistModuleToCoreInEditor();
            }
        }

        protected void RegistModuleToCoreInEditor()
        {
            if (coreProxy != null)
            {
                if (!tryReplace)
                {
                    if (!Module.TryAddToCore(coreProxy.Core, runOnlyPassiveEventWhenRegist))
                    {
                        UnityEngine.Debug.LogWarning($"Core already has key : {gameObject.name}|{Module.Name}");
                    }
                    else
                    {
                        Module.SetInactive(this, inactive, runOnlyPassiveEventWhenInactive);
                    }
                }
                else
                {
                    Module.ReplaceModuleInCore(coreProxy.Core, runOnlyPassiveEventWhenRegist);
                    Module.SetInactive(this, inactive, runOnlyPassiveEventWhenInactive);
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"CoreProxy is null : {gameObject.name}|{Module.Name}");
            }
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

#if UNITY_EDITOR
        protected bool isPrevInactiveInitalized;
        protected bool prevInactive;

        protected void OnEnable()
        {
            if (!isPrevInactiveInitalized)
            {
                prevInactive = inactive;
                isPrevInactiveInitalized = true;
            }
        }

        protected void OnValidate()
        {
            if (!Application.isPlaying || !isPrevInactiveInitalized)
                return;

            if (prevInactive != inactive)
            {
                if (Module.Inactived == null)
                    Module.SetInactived(new Boolstack());

                Module.SetInactive(this, inactive, runOnlyPassiveEventWhenInactive);

                prevInactive = inactive;
            }
        }
    }
#endif
}
