using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrizzleEvents
{
    [AddComponentMenu("Drizzle Events/Event System")]
    [DefaultExecutionOrder(-1000)]
    public class EventSystem : MonoBehaviour
    {
        public static EventSystem Instance { get; private set; }
        private Dictionary<Type, List<object>> _subscribers;

        /*
         * When an event handler is called, it may trigger additional events that need to be processed.
         * Consequently, those events could have handlers that also fire events leading to many layers
         * of events firing as the system process all the events and handlers.
         *
         * This property sets a maximum depth on those calls to ensure the message system doesn't lock up.
         *
         * Any messages received at depth n will be the last processed, and a warning will be logged to indicate that the depth may need to be increased.
         */
        [Min(1)]
        [SerializeField]
        private int eventDepth = 10;

        [SerializeField] private bool logEvents;
        public bool LogEvents => logEvents;

        private Queue<Action> _deferredQueueA;
        private Queue<Action> _deferredQueueB;
        private Queue<Action> _currentDeferredQueue;
        private Queue<Action> _nextDeferredQueue;

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this);
                return;
            }

            _deferredQueueA = new Queue<Action>(1000);
            _deferredQueueB = new Queue<Action>(1000);
            _currentDeferredQueue = _deferredQueueA;
            _nextDeferredQueue = _deferredQueueB;
            
            Instance = this; 
            _subscribers = new Dictionary<Type, List<object>>();
        }

        private void Update()
        {
            for (var level = 0; level < eventDepth; level++)
            {
                if (_currentDeferredQueue.Count == 0)
                {
                    break;
                }
                
                var currentQueue = _currentDeferredQueue;
                _currentDeferredQueue = _nextDeferredQueue;
                _nextDeferredQueue = currentQueue;

                for (var i = 0; i < currentQueue.Count; i++)
                {
                    currentQueue.Dequeue()();
                }

                if (_currentDeferredQueue.Count == 0)
                {
                    break;
                }
            }

            if (_currentDeferredQueue.Count > 0)
            {
                Debug.LogWarning($"Max event depth {eventDepth} was reached and {_currentDeferredQueue.Count} were received. These will be processed on the next updated");
            }
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

                _currentDeferredQueue.Enqueue(() =>
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

                _currentDeferredQueue.Enqueue(() =>
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
