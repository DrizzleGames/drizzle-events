using UnityEngine.SceneManagement;

namespace DrizzleEvents
{

    /**
     * Awake only fires when the object is instantiated.
     *
     * This class allows us to bind events in SceneStart instead of Awake and will fire every scene load including Awake.
     */
    public abstract class EventBehaviourSingleton<T> : EventBehaviour where T : EventBehaviourSingleton<T>
    {
        public static T Instance { get; private set; }

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                SceneManager.sceneLoaded += OnSceneLoaded;
                DontDestroyOnLoad(gameObject);
                base.Awake();
            } else {
                Destroy(gameObject);
            }
        }

        protected override void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            base.OnDestroy();
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneStart();
        }

        protected virtual void SceneStart() { }

    }
}