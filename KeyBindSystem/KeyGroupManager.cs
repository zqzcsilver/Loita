using System;
using System.Collections.Generic;

using Loita.UI.UIContainers.DebugUI.DebugItems;

using Microsoft.Xna.Framework;

namespace Loita.KeyBindSystem
{
    internal class KeyGroupManager
    {
        public static KeyGroupManager Instance => Loita.KeyGroupManager;
        private Dictionary<string, KeyGroup> _keyGroups;

        public KeyGroupManager()
        {
            _keyGroups = new Dictionary<string, KeyGroup>();
        }

        public bool RegisterKeyGroup(KeyGroup keyGroup)
        {
            if (keyGroup == null || _keyGroups.ContainsKey(keyGroup.Name))
                return false;
            _keyGroups.Add(keyGroup.Name, keyGroup);
            LoggerItem.WriteLine($"[Loita:Key Group Manager]按键{keyGroup.Name}被注册，热键为{keyGroup}");
            return true;
        }

        public KeyGroup GetKeyGroup(string name)
        {
            if (!_keyGroups.ContainsKey(name))
                return null;
            return _keyGroups[name];
        }

        public void Update(GameTime gt)
        {
            foreach (var kg in _keyGroups.Values)
            {
                kg.Update(gt);
            }
        }
    }
}