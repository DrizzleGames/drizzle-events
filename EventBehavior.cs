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
            if (System.Instance.LogEvents)
            {
                Debug.Log("Destroying subs");
            }
            foreach (var unsub in _unsubs)
            {
                unsub?.Invoke();
            }
        }

        protected void Subscribe<T>(Action<T> handler) where T : IEventArg
        {
            _unsubs.Add(System.Instance.Subscribe<T>(handler));
        }
        
        protected void Subscribe<T>(Action handler) where T : IEvent
        {
            _unsubs.Add(System.Instance.Subscribe<T>(handler));
        }

        protected void Publish<T>(T message) where T : IEventArg
        {
            System.Instance.Publish(message); 
        }
        
        protected void Publish<T>() where T : IEvent
        {
            System.Instance.Publish<T>(); 
        }
    }
}