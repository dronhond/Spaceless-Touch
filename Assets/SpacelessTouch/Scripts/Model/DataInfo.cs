using System;
using System.Collections.Generic;

namespace SpacelessTouch.Scripts.Model
{
    [Serializable]
    public class DataInfo
    {
        public int level;
        public readonly Dictionary<string, int> EventInfo = new();

        public DataInfo(int level = 1)
        {
            this.level = level;
        }

        public void SetEvent(string key, int value)
        {
            CheckEvent(key);
            EventInfo[key] = value;
        }

        private void CheckEvent(string key)
        {
            if (EventInfo.ContainsKey(key)) return;
            AddDefaultEvent(key);
        }

        public int GetEvent(string key)
        {
            if (EventInfo.TryGetValue(key, out var @event)) return @event;
            AddDefaultEvent(key);
            return EventInfo[key];
        }

        private void AddDefaultEvent(string key)
        {
            EventInfo.Add(key, 0);
        }
    }
}