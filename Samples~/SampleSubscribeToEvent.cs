using UnityEngine.UI;

namespace DrizzleEvents.Samples
{
    
    public class SampleSubscribeToEvent : EventBehaviour
    {
        private Text _textField;
        private int _pressCount;
        private void Start()
        {
            _textField = GetComponent<Text>();
            SubscribeGlobal<SampleButtonPressedEvent>(ButtonPressedHandler);
        }

        private void ButtonPressedHandler()
        {
            _pressCount += 1;
            _textField.text = $"Button Press Counter: {_pressCount}";
        }
        
    }
}
