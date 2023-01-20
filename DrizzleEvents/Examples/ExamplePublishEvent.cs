namespace DrizzleEvents.Examples
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
