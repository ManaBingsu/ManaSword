using System.Collections.Generic;

namespace ManaSword.ModuleSystem
{
    [System.Serializable]
    public class Core
    {
        protected Dictionary<string, Module> moduleDict = new Dictionary<string, Module>();
        public Dictionary<string, Module> ModuleDict => moduleDict;

        public bool TryAddModule(Module module)
        {
            if (moduleDict.ContainsKey(module.Name))
                return false;

            UnityEngine.Debug.LogWarning($"{module.Name} is added");
            moduleDict.Add(module.Name, module);
            return true;
        }

        public void ReplaceModule(Module module)
        {
            if (!moduleDict.ContainsKey(module.Name))
                TryAddModule(module);
            else
                moduleDict[module.Name] = module;
        }

        public bool TryRemoveModule(Module module)
        {
            if (!moduleDict.ContainsKey(module.Name))
                return false;

            moduleDict.Remove(module.Name);
            return true;
        }

        public bool TryGetModule(string moduleName, out Module module)
        {
            module = null;
            if (moduleDict.ContainsKey(moduleName))
            {
                module = moduleDict[moduleName];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
