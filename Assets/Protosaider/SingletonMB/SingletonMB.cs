using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = System.Object;

//got it from unity3d.wiki
namespace Protosaider
{

	/// <summary>
	/// Inherit from this base class to create a singleton.
	/// e.g. public class MyClassName : SingletonMB<MyClassName> {}
	/// </summary>
	//public class SingletonMB<T> : Singleton<SingletonMB<T>> where T : Singleton<T>
	public class SingletonMB<T> : BaseSingletonMB where T : BaseSingletonMB
	{
		/*
		// Thanks to the attribute, this method is executed before any other MonoBehaviour
		// logic in the game.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RuntimeInitializeOnLoad()
	    {
			Debug.Log("In Runtime Initialization SingletonMB");
			var instance = FindObjectOfType<T>();

			if (instance == null)
				instance = new GameObject($"{typeof(T)} [Singleton]").AddComponent<T>();

			DontDestroyOnLoad(instance);

			Instance = instance;
		}

	    // This Awake() will be called immediately after AddComponent() execution
	    // in the RuntimeInitializeOnLoad(). In other words, before any other MonoBehaviour's
	    // in the scene will begin to initialize.
	    protected virtual void Awake()
		{
			// Initialize non-MonoBehaviour logic, etc.
			Debug.Log("SingletonMB.Awake()", this);
		}
		*/

		// thread safety
		[NotNull]
		private static readonly Object _lock = new Object();

		[SerializeField]
		private static Boolean s_isLazy;

		// Whether or not this object should persist when loading new scenes. Should be set in Init().
		[SerializeField]
		protected static Boolean s_dontDestroyOnLoad = true;

		// instead of heavy comparision (instance != null)
		// http://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
		private static Boolean s_isInstantiated;

		private static Boolean s_isApplicationQuitting;

		[CanBeNull]
		private static volatile T s_instance;

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		[CanBeNull]
		public static T Instance
		{
			get
			{
				if (s_isApplicationQuitting)
				{
					Debug.LogWarningFormat(
						"[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.",
						typeof(T));

					return null;
				}

				if (!s_isInstantiated)
					s_instance = GetOrCreate<T>();

				return s_instance;
			}
		}


		internal override void CreateInstance()
		{
			if (s_isInstantiated || s_isLazy)
			{
				return;
			}

			lock (_lock)
			{
				s_instance = GetOrCreate<T>();
				s_isInstantiated = true;

				if (s_dontDestroyOnLoad)
					DontDestroyOnLoad(s_instance.gameObject);

				Debug.LogWarningFormat("[Singleton] An Instance of '{0}' is needed in the scene, so '{1}' was created{2}",
					typeof(T), s_instance.gameObject.name, s_dontDestroyOnLoad ? " with DontDestoryOnLoad." : ".");
			}
		}

		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		protected virtual void OnDestroy()
		{
			s_isApplicationQuitting = true;
			s_isInstantiated = false;
		}

		protected virtual void OnApplicationQuit()
		{
			s_isApplicationQuitting = true;
			s_isInstantiated = false;
		}
	}

}