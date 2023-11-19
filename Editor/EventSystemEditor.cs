using DrizzleEvents;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EventSystemEditor
    {
        [MenuItem("GameObject/Drizzle Events/Create Global Events Manager", priority = 1)]
        private static void CreateEventSystemObject()
        {
            var newEventSystem = new GameObject("Drizzle Events Manager", typeof(EventManager));
            newEventSystem.transform.SetParent(Selection.activeTransform);
        }
        
    }
}