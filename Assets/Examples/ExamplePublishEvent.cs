using DrizzleEvents;
using UnityEngine;

namespace Examples
{
    public interface ExampleButtonPressedEvent : IEvent{}
    
    public class ExamplePublishEvent : EventBehaviour
    {
        public void ButtonPressed()
        {
            Publish<ExampleButtonPressedEvent>();
        }
    }
}
