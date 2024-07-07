#if UNITY_EDITOR
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
 
        [MenuItem(itemName: "Assets/Create/Drizzle Events/New Event Behavior", isValidateFunction: false, priority: 1)]
        public static void NewEventBehaviour()
        {
            CreateFile("NewEventBehaviour", "New Event Behaviour");
        }
 
        [MenuItem(itemName: "Assets/Create/Drizzle Events/New Event (With Args)", isValidateFunction: false, priority: 2)]
        public static void NewArgsEvent()
        {
            CreateFile("NewArgsEvent", "NewArgsEvent");
        }
 
        [MenuItem(itemName: "Assets/Create/Drizzle Events/New Event (No Args)", isValidateFunction: false, priority: 3)]
        public static void NewNoArgsEvent()
        {
            CreateFile("NewNoArgsEvent", "NewNoArgsEvent");
        }
        
        private const string TemplatesFolder = "Packages/com.drizzlegames.drizzle-events/Editor/ScriptTemplates/";
        private static void CreateFile(string templateFilename, string filename)
        {
            var path = $"{TemplatesFolder}{templateFilename}.cs.txt";
            var newFileName = $"{filename}.cs";
            
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, newFileName);
        }
    }
}
#endif