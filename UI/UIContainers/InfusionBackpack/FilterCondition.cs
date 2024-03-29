﻿using System;
using System.Collections.Generic;

namespace Loita.UI.UIContainers.InfusionBackpack
{
    internal class FilterCondition
    {
        private Dictionary<string, bool> conditions = new Dictionary<string, bool>();

        public event Action<string> OnConditionChanged;

        public bool this[string key]
        {
            get => conditions.ContainsKey(key) && conditions[key];
            set
            {
                if (conditions.ContainsKey(key))
                    conditions[key] = value;
                else
                    conditions.Add(key, value);
                OnConditionChanged?.Invoke(key);
            }
        }

        public void ForEach(Action<string, bool> action)
        {
            foreach (var c in conditions)
            {
                action(c.Key, c.Value);
            }
        }
    }
}