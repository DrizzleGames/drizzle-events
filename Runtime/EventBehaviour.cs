using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrizzleEvents
{
    public class EventBehaviour : MonoBehaviour
    {
        private List<Action> _unsubs = new();

        protected virtual void OnDestroy()
        {
            if (EventSystem.Instance.LogEvents)
            {
                Debug.Log("Destroying subs");
            }
            foreach (var unsub in _unsubs)
            {
                unsub?.Invoke();
            }
        }

        protected void Subscribe<T>(Action<T> handler) where T : IEventWithArgs
        {
            _unsubs.Add(EventSystem.Instance.Subscribe<T>(handler));
        }
        
        protected void Subscribe<T>(Action handler) where T : IEventNoArgs
        {
            _unsubs.Add(EventSystem.Instance.Subscribe<T>(handler));
        }

        protected void Publish<T>(T message) where T : IEventWithArgs
        {
            EventSystem.Instance.Publish(message); 
        }
        
        protected void Publish<T>() where T : IEventNoArgs
        {
            EventSystem.Instance.Publish<T>(); 
        }
    }
}