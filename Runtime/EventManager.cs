
using UnityEngine;

namespace DrizzleEvents
{
    [AddComponentMenu("Drizzle Events/Event Manager")]
    [DefaultExecutionOrder(-1000)]
    public class EventManager : EventSystem
    {
        public static EventSystem Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this) 
            { 
                Destroy(this);
                return;
            }

            Instance = this; 
        }
    }
}
