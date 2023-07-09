using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrizzleEvents
{
    public class EventBehaviour : MonoBehaviour
    {
        private List<Action> _unsubs = new();
        private EventSystem _localEventSystem;

        public void Awake()
        {
            _localEventSystem = findLocalEventSystemInParent();
        }

        protected virtual void OnDestroy()
        {
            if (EventManager.Instance.LogEvents)
            {
                Debug.Log("Destroying subs");
            }
            foreach (var unsub in _unsubs)
            {
                unsub?.Invoke();
            }
        }

        protected void SubscribeGlobal<T>(Action<T> handler) where T : IEventWithArgs
        {
            _unsubs.Add(EventManager.Instance.Subscribe<T>(handler));
        }
        
        protected void SubscribeGlobal<T>(Action handler) where T : IEventNoArgs
        {
            _unsubs.Add(EventManager.Instance.Subscribe<T>(handler));
        }

        protected void PublishGlobal<T>(T message) where T : IEventWithArgs
        {
            EventManager.Instance.Publish(message); 
        }
        
        protected void PublishGlobal<T>() where T : IEventNoArgs
        {
            EventManager.Instance.Publish<T>(); 
        }
        
        protected void SubscribeLocal<T>(Action<T> handler) where T : IEventWithArgs
        {
            if (_localEventSystem != null)
            {
                _unsubs.Add(_localEventSystem.Subscribe<T>(handler));
            }
            else
            {
                Debug.LogError("Failed to subscribe local - local event system is null");
            }
        }
        
        protected void SubscribeLocal<T>(Action handler) where T : IEventNoArgs
        {
            if (_localEventSystem != null)
            {
                _unsubs.Add(EventManager.Instance.Subscribe<T>(handler));
            }
            else
            {
                Debug.LogError("Failed to subscribe local - local event system is null");
            }
        }

        protected void PublishLocal<T>(T message) where T : IEventWithArgs
        {
            if (_localEventSystem != null)
            {
                _localEventSystem.Publish(message); 
            }
            else
            {
                Debug.LogError("Failed to publish local - local event system is null");
            }
        }
        
        protected void PublishLocal<T>() where T : IEventNoArgs
        {
            if (_localEventSystem != null)
            {
                _localEventSystem.Publish<T>(); 
            }
            else
            {
                Debug.LogError("Failed to publish local - local event system is null");
            }
        }
        
        private EventSystem findLocalEventSystemInParent()
        {
            var currentGameObject = gameObject;

            EventSystem eventSystem = null;
            while (currentGameObject != null)
            {
                eventSystem = gameObject.GetComponent<EventSystem>();

                if (eventSystem != null)
                {
                    break;
                }

                currentGameObject = currentGameObject.transform.parent.gameObject;
            }

            return eventSystem;
        }
    }
}