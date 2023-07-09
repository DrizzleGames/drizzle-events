using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrizzleEvents
{
    [AddComponentMenu("Drizzle Events/Event System")]
    [DefaultExecutionOrder(-1000)]
    public class EventSystem : MonoBehaviour
    {
        private Dictionary<Type, List<object>> _subscribers;
        [SerializeField] private bool logEvents;
        public bool LogEvents => logEvents;

        protected virtual void Awake()
        {
            _subscribers = new Dictionary<Type, List<object>>();
        }

        public Action Subscribe<T>(Action<T> handler) where T : IEventWithArgs
        {
            AddHandlerToDictionary<T>(handler);
            return () => Unsubscribe(handler);
        }
        
        public Action Subscribe<T>(Action handler) where T : IEventNoArgs
        {
            AddHandlerToDictionary<T>(handler);
            return () => Unsubscribe<T>(handler);
        }
    
        public void Publish<T>(T message) where T : IEventWithArgs
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                if (logEvents)
                {
                    Debug.Log($"[PUBLISH 0]: {message}");
                }
                return;
            }
            
            if (logEvents)
            {
                Debug.Log($"[PUBLISH {_subscribers[typeof(T)].Count}]: {message}");
            }
        
            foreach (var sub in _subscribers[typeof(T)])
            {
                if (sub == null)
                {
                    Debug.LogError("Subscriber function is null");
                    continue;
                }

                try
                {
                    ((Action<T>) sub)(message);
                }
                catch (Exception e)
                {
                    if (logEvents)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
        
        public void Publish<T>() where T : IEventNoArgs
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                if (logEvents)
                {
                    Debug.Log($"[PUBLISH 0]: {typeof(T)}");
                }
                return;
            }
            
            if (logEvents)
            {
                Debug.Log($"[PUBLISH {_subscribers[typeof(T)].Count}]: {typeof(T)}");
            }
        
            foreach (var sub in _subscribers[typeof(T)])
            {
                if (sub == null)
                {
                    Debug.LogError("Subscriber function is null");
                    continue;
                }
                
                try
                {
                    ((Action) sub)();
                }
                catch (Exception e)
                {
                    if (logEvents)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
        
        private void Unsubscribe<T>(Action<T> handler) where T : IEventWithArgs
        {
            var subList = _subscribers.GetValueOrDefault(typeof(T), default);
            subList?.Remove(handler);
        }
        
        private void Unsubscribe<T>(Action handler) where T : IEventNoArgs
        {
            var subList = _subscribers.GetValueOrDefault(typeof(T), default);
            subList?.Remove(handler);
        }

        private void AddHandlerToDictionary<T>(object handler)
        {
            var subList = _subscribers.GetValueOrDefault(typeof(T), default);
        
            if (subList == null)
            {
                subList = new List<object>();
                _subscribers.Add(typeof(T), subList); 
            }
        
            subList.Add(handler);
        }

    }
}
