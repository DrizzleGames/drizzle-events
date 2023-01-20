namespace DrizzleEvents.Samples
{
    public interface SampleButtonPressedEvent : IEvent{}
    
    public class SamplePublishEvent : EventBehaviour
    {
        public void ButtonPressed()
        {
            Publish<SampleButtonPressedEvent>();
        }
    }
}
