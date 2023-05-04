using System;
using System.Collections.Generic;

namespace YooAsset.Editor
{
    internal static class EventModule
    {
        private static Dictionary<Event, HashSet<Action>> _EventMap = new Dictionary<Event, HashSet<Action>>();
        public static void AddListener(Event evt, Action action)
        {
            if(_EventMap.ContainsKey(evt) == false)
                _EventMap.Add(evt, new HashSet<Action>());
            _EventMap[evt].Add(action);
        }
        
        public static void BroadCast(Event evt)
        {
            if(_EventMap.ContainsKey(evt) == false)
                return;
            foreach (var action in _EventMap[evt])
            {
                action?.Invoke();
            }
        }
        
    }
}