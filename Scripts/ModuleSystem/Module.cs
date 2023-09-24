using System;
using ManaSword.SaveLoadSystem;
using ManaSword.Utility;

namespace ManaSword.ModuleSystem
{
    [System.Serializable]
    public abstract class Module : ISaveLoadEntity, IBoolstackSender
    {
        protected Core core;
        public Core Core => core;

        public string Name => GetType().ToString();

        public bool IsEditorDebugMode => throw new NotImplementedException();

        protected Boolstack inactived = new Boolstack();
        public Boolstack Inactived => inactived;
        public Boolstack SetInactived(Boolstack inactived) => this.inactived = inactived;

        public Action AddToCoreAdditionalAction;
        public Action AddToCoreEssentialAction;
        public Action RemoveFromCoreAdditionalAction;
        public Action RemoveFromCoreEssentialAction;
        public Action InactiveAdditionalEvent;
        public Action InactiveEssentialEvent;
        public Action ActiveAdditionalEvent;
        public Action ActiveEssentialEvent;

        #region Add
        public bool TryAddToCore(Core core, bool runOnlyPassive)
        {
            if (core.TryAddModule(this))
            {
                this.core = core;

                if (!runOnlyPassive)
                    RunAddToCoreAdditionalEvent();
                RunAddToCoreEssentialEvent();

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void RunAddToCoreAdditionalEvent()
        {
            if (inactived.Value)
                return;

            AddToCoreAdditionalAction?.Invoke();
        }

        protected void RunAddToCoreEssentialEvent()
        {
            if (inactived.Value)
                return;

            RunAddToCoreEssentialEventByModule();
            AddToCoreEssentialAction?.Invoke();
        }
        protected virtual void RunAddToCoreEssentialEventByModule() { }
        #endregion

        #region Replace
        public void ReplaceModuleInCore(Core core, bool runOnlyPassive)
        {
            core.ReplaceModule(this);
            this.core = core;

            if (!runOnlyPassive)
                RunAddToCoreAdditionalEvent();
            RunAddToCoreEssentialEvent();
        }
        #endregion

        #region Inactive
        public void SetInactive(IBoolstackSender sender, bool inactive, bool runOnlyPassive)
        {
            if (inactived.Regist(sender, inactive, out bool isToggled))
            {
                if (isToggled)
                {
                    if (!runOnlyPassive)
                        RunInactiveAdditionalEvent();
                    RunInactiveEssentialEvent();
                }
            }
        }

        protected void RunInactiveAdditionalEvent()
        {
            if (inactived.Value)
            {
                InactiveAdditionalEvent?.Invoke();
            }
            else
            {
                ActiveAdditionalEvent?.Invoke();
            }
        }

        protected void RunInactiveEssentialEvent()
        {
            if (inactived.Value)
            {
                RunInactiveEssentialEventByModule();
                InactiveEssentialEvent?.Invoke();
            }
            else
            {
                RunActiveEssentialEventByModule();
                ActiveEssentialEvent?.Invoke();
            }
        }
        protected virtual void RunInactiveEssentialEventByModule() { }
        protected virtual void RunActiveEssentialEventByModule() { }

        #endregion
        
        #region Remove
        public bool TryRemoveFromCore(Core core, bool runOnlyPassive)
        {
            if (core.TryRemoveModule(this))
            {
                if (!runOnlyPassive)
                    RunRemoveFromCoreAdditionalEvent();
                RunRemoveFromCoreEssentialEvent();

                this.core = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void RunRemoveFromCoreAdditionalEvent()
        {
            RemoveFromCoreAdditionalAction?.Invoke();
        }
        protected void RunRemoveFromCoreEssentialEvent()
        {
            RunRemoveFromCoreModuleEssentialEvent();
            RemoveFromCoreEssentialAction?.Invoke();
        }
        protected virtual void RunRemoveFromCoreModuleEssentialEvent() { }
        #endregion

        public virtual void Initalize() { }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
