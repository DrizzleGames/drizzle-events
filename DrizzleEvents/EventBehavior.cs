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
            Debug.Log("Destroying subs");
            foreach (var unsub in _unsubs)
            {
                unsub?.Invoke();
            }
        }

        protected void Subscribe<T>(Action<T> handler) where T : IEventArg
        {
            _unsubs.Add(MessageSystem.Instance.Subscribe<T>(handler));
        }
        
        protected void Subscribe<T>(Action handler) where T : IEventNoArg
        {
            _unsubs.Add(MessageSystem.Instance.Subscribe<T>(handler));
        }

        protected void Publish<T>(T message) where T : IEventArg
        {
            MessageSystem.Instance.Publish(message); 
        }
        
        protected void Publish<T>() where T : IEventNoArg
        {
            MessageSystem.Instance.Publish<T>(); 
        }
    }
}