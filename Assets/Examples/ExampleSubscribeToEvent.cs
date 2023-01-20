using DrizzleEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Examples
{
    
    public class ExampleSubscribeToEvent : EventBehaviour
    {
        private Text _textField;
        private int _pressCount;
        private void Start()
        {
            _textField = GetComponent<Text>();
            Subscribe<ExampleButtonPressedEvent>(ButtonPressedHandler);
        }

        private void ButtonPressedHandler()
        {
            _pressCount += 1;
            _textField.text = $"Button Press Counter: {_pressCount}";
        }
        
    }
}
