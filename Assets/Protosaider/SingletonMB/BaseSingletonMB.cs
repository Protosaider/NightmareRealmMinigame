using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

//I think i take it from yaSingleton
namespace Protosaider
{
	
	/// <summary>
	/// Inherit from this base class to create a singleton.
	/// e.g. public class MyClassName : Singleton<MyClassName> {}
	/// </summary>
	[DisallowMultipleComponent]
	//public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	public abstract class BaseSingletonMB : MonoBehaviour
	{
		internal abstract void CreateInstance();

		// Reduce the visibility of OnEnable;
		private void OnEnable()
		{
	#if UNITY_EDITOR
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
				return;
	#endif
			AllSingletons.Add(this);
		}

		// Reduce the visibility of OnDisable;
		private void OnDisable() { }


		protected static SingletonMBGlobalMB GlobalMB => SingletonMBGlobalMB.Instance;

		protected virtual void Initialize()
		{
			GlobalMB.DestroyEvent += Deinitialize;

			GlobalMB.FixedUpdateEvent += OnFixedUpdate;
			GlobalMB.UpdateEvent += OnUpdate;
			GlobalMB.LateUpdateEvent += OnLateUpdate;

			Debug.Log("[SingletonMB] was initialized");
		}

		//Use it instead of OnApplicationQuit and OnDestroy
		protected virtual void Deinitialize() { }

		public virtual void OnFixedUpdate() { }
		public virtual void OnUpdate() { }
		public virtual void OnLateUpdate() { }


		protected Coroutine StartCoroutine(IEnumerator routine) => GlobalMB.StartCoroutine(routine);
		protected Coroutine StartCoroutine(String methodName) => GlobalMB.StartCoroutine(methodName);
		protected void StopCoroutine(Coroutine routine) => GlobalMB.StopCoroutine(routine);
		protected void StopCoroutine(IEnumerator routine) => GlobalMB.StopCoroutine(routine);
		protected void StopCoroutine(String methodName) => GlobalMB.StopCoroutine(methodName);
		protected void StopAllCoroutines() => GlobalMB.StopAllCoroutines();


		private static readonly List<BaseSingletonMB> AllSingletons = new List<BaseSingletonMB>();

		//protected static Boolean SetParent<T>(Transform parent = null) where T : Singleton
		//{
		//	var instance = AllSingletons.FirstOrDefault(s => s.GetType() == typeof(T)) as T;

		//	if (!instance)
		//		return false;

		//	instance.gameObject.transform.SetParent(parent ?? SingletonMBGlobalMB.Instance.transform);

		//	return true;
		//}

		protected static T GetOrCreate<T>() where T : BaseSingletonMB
		{
			var instance = AllSingletons.FirstOrDefault(s => s.GetType() == typeof(T)) as T;

			if (instance)
				return instance;

			Object[] objects = s_findInactiveInstances
				? Resources.FindObjectsOfTypeAll(typeof(T))
				: FindObjectsOfType(typeof(T));

			if (objects == null || objects.Length < 1)
			{
				GameObject singleton = new GameObject
				{
					name = $"{typeof(T)} [Singleton]"
				};
				instance = singleton.AddComponent<T>();
				instance.Initialize(); //TODO !!! Check if it works
				instance.gameObject.transform.SetParent(SingletonMBGlobalMB.Instance.transform);
				////Debug.LogWarningFormat("[Singleton] An Instance of '{0}' is needed in the scene, so '{1}' was created.", typeof(T), singleton.name);
			}
			else if (objects.Length >= 1)
			{
				instance = objects[0] as T;

				if (objects.Length > 1)
				{
					Debug.LogWarningFormat("[Singleton] {0} instances of '{1}'!", objects.Length, typeof(T));

					if (s_destroyMultipleInstances)
					{
						for (Int32 i = 1; i < objects.Length; i++)
						{
							Debug.LogWarningFormat("[Singleton] Deleting extra '{0}' instance attached to '{1}'", typeof(T),
								objects[i].name);
							Destroy(objects[i]);
						}
					}
				}

				return instance;
			}

			return instance;
		}



		[SerializeField]
		private static Boolean s_findInactiveInstances = true;

		// Whether or not destroy other singleton instances if any. Should be set in Init().
		[SerializeField]
		private static Boolean s_destroyMultipleInstances = true;



		//  private void Awake()
		//  {
		//      if (s_isLazy)
		//	return;

		//lock (_lock)
		//      {
		//          if (!s_isInstantiated)
		//          {
		//              Instance = this as T;
		//          }
		//          else if (s_destroyMultipleInstances && Instance.GetInstanceID() != GetInstanceID())
		//          {
		//              Debug.LogWarningFormat("[Singleton] Deleting extra '{0}' instance attached to '{1}'", typeof(T), name);
		//              Destroy(this);
		//          }
		//      }
		//  }


		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeSingletonsOnLoad()
		{
			Debug.Log($"Initialize SingletonsMB");

			foreach (var singleton in AllSingletons)
			{
				singleton.CreateInstance();
			}
		}

	}


	/*
		// thread safety
		[NotNull]
	    private static readonly System.Object _lock = new System.Object();

		[SerializeField]
		private static Boolean s_findInactiveInstances = true;
	    // Whether or not this object should persist when loading new scenes. Should be set in Init().
		[SerializeField]
		private static Boolean s_dontDestroyOnLoad;
	    // Whether or not destroy other singleton instances if any. Should be set in Init().
		[SerializeField]
		private static Boolean s_destroyMultipleInstances = true;
		[SerializeField]
		private static Boolean s_isLazy;

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
	                Debug.LogWarningFormat("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.", typeof(T));
	                return null;
	            }
	            lock (_lock)
	            {
					if (s_isInstantiated)
						return s_instance;

					Object[] objects = s_findInactiveInstances ? Resources.FindObjectsOfTypeAll(typeof(T)) : FindObjectsOfType(typeof(T));

					if (objects == null || objects.Length < 1)
					{
						GameObject singleton = new GameObject
						{
							name = $"{typeof(T)} [Singleton]"
						};
						Instance = singleton.AddComponent<T>();
						Debug.LogWarningFormat("[Singleton] An Instance of '{0}' is needed in the scene, so '{1}' was created{2}", typeof(T), singleton.name, s_dontDestroyOnLoad ? " with DontDestoryOnLoad." : ".");
					}
					else if (objects.Length >= 1)
					{
						Instance = objects[0] as T;

						if (objects.Length > 1)
						{
							Debug.LogWarningFormat("[Singleton] {0} instances of '{1}'!", objects.Length, typeof(T));
							if (s_destroyMultipleInstances)
							{
								for (Int32 i = 1; i < objects.Length; i++)
								{
									Debug.LogWarningFormat("[Singleton] Deleting extra '{0}' instance attached to '{1}'", typeof(T), objects[i].name);
									Destroy(objects[i]);
								}
							}
						}
						return s_instance;
					}

					return s_instance;
	            }
	        }

	        protected set
	        {
	            s_instance = value;
	            s_isInstantiated = true;
	            s_instance.AwakeSingleton();

				if (s_dontDestroyOnLoad)
					DontDestroyOnLoad(s_instance.gameObject);
			}
	    }

	    // if Lazy = false and gameObject is active this will set instance
	    // unless instance was called by another Awake method
	    private void Awake()
	    {
	        if (s_isLazy)
				return;

			lock (_lock)
	        {
	            if (!s_isInstantiated)
	            {
	                Instance = this as T;
	            }
	            else if (s_destroyMultipleInstances && Instance.GetInstanceID() != GetInstanceID())
	            {
	                Debug.LogWarningFormat("[Singleton] Deleting extra '{0}' instance attached to '{1}'", typeof(T), name);
	                Destroy(this);
	            }
	        }
	    }

	    // this might be called for inactive singletons before Awake if FindInactive = true
	    protected virtual void AwakeSingleton() { }

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
	 */

}