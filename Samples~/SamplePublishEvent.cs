namespace DrizzleEvents.Samples
{
    public interface SampleButtonPressedEvent : IEventNoArgs{}
    
    public class SamplePublishEvent : EventBehaviour
    {
        public void ButtonPressed()
        {
            PublishGlobal<SampleButtonPressedEvent>();
        }
    }
}
