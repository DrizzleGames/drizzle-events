using DrizzleEvents;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EventSystemEditor
    {
        [MenuItem("GameObject/Drizzle Events/Create Event System", priority = 1)]
        private static void CreateEventSystemObject()
        {
            var newEventSystem = new GameObject("Drizzle Events System", typeof(EventSystem));
            newEventSystem.transform.SetParent(Selection.activeTransform);
        }
        
    }
}