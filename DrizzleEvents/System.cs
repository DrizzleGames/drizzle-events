using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DrizzleEvents
{
    public class System : MonoBehaviour
    {
        public static System Instance { get; private set; }
        private Dictionary<Type, List<object>> _subscribers;

        [SerializeField] private bool logEvents;
        public bool LogEvents => logEvents;

        private bool _activeQueueIsA;
        private Queue<Action> _deferredQueueA;
        private Queue<Action> _deferredQueueB;
        private Queue<Action> _currentDeferredEventQueue;

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this);
                return;
            }

            _activeQueueIsA = true;
            _deferredQueueA = new Queue<Action>(1000);
            _deferredQueueB = new Queue<Action>(1000);
            _currentDeferredEventQueue = _deferredQueueA;
            
            Instance = this; 
            _subscribers = new Dictionary<Type, List<object>>();
        }

        private void Update()
        {
            var currentQueue = _currentDeferredEventQueue;
            if (_activeQueueIsA)
            {
                _activeQueueIsA = false;
                _deferredQueueB.Clear();
                _currentDeferredEventQueue = _deferredQueueB;
            }
            else
            {
                _activeQueueIsA = true;
                _deferredQueueA.Clear();
                _currentDeferredEventQueue = _deferredQueueA;
            }
            
            foreach (var action in currentQueue)
            {
                action();
            }
        }

        public Action Subscribe<T>(Action<T> handler) where T : IEventArg
        {
            AddHandlerToDictionary<T>(handler);
            return () => Unsubscribe(handler);
        }
        
        public Action Subscribe<T>(Action handler) where T : IEvent
        {
            AddHandlerToDictionary<T>(handler);
            return () => Unsubscribe<T>(handler);
        }
    
        public void Publish<T>(T message) where T : IEventArg
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

                _currentDeferredEventQueue.Enqueue(() =>
                {
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
                });
            }
        }
        
        public void Publish<T>() where T : IEvent
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

                _currentDeferredEventQueue.Enqueue(() =>
                {
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
                });
            }
        }
        
        private void Unsubscribe<T>(Action<T> handler) where T : IEventArg
        {
            var subList = _subscribers.GetValueOrDefault(typeof(T), default);
            subList?.Remove(handler);
        }
        
        private void Unsubscribe<T>(Action handler) where T : IEvent
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
