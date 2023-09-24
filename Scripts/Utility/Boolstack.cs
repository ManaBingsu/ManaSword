using System.Collections.Generic;

namespace ManaSword.Utility
{
    public class Boolstack
    {
        public bool Value
        {
            get
            {
                return senderGroups[true].Count > senderGroups[false].Count;
            }
        }

        protected Dictionary<bool, HashSet<IBoolstackSender>> senderGroups = new Dictionary<bool, HashSet<IBoolstackSender>>()
        {
            { false, new HashSet<IBoolstackSender>() },
            { true, new HashSet<IBoolstackSender>() }
        };

        public bool Regist(IBoolstackSender sender, bool value, out bool isToggled)
        {
            var senderGroup = senderGroups[value];
            if (senderGroup.Contains(sender))
            {
                UnityEngine.Debug.LogWarning($"Boolstack already has key");
                isToggled = false;
                return false;
            }

            var prevValue = Value;
            if (senderGroups[!value].Contains(sender))
            {
                Unregist(sender, !value, out isToggled);
            }
            else
            {
                senderGroup.Add(sender);
            }

            isToggled = prevValue != Value;
            return true;
        }

        public bool Unregist(IBoolstackSender sender, bool value, out bool isToggled)
        {
            var senderGroup = senderGroups[value];
            if (!senderGroup.Contains(sender))
            {
                UnityEngine.Debug.LogWarning($"Boolstack doesn't have key");
                isToggled = false;
                return false;
            }
            var prevValue = Value;
            senderGroup.Remove(sender);
            isToggled = prevValue != Value;
            return true;
        }
    }
}
